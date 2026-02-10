using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VTube.ConfigValidation
{
    public static class ConfigValidationRunner
    {
        private const string SchemaFolderRelative = "VTube/Schemas";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ValidateOnStartup()
        {
            try
            {
                var projectRoot = TryResolveProjectRoot();
                if (string.IsNullOrWhiteSpace(projectRoot))
                {
                    Debug.LogWarning("[ConfigValidation] Project path not resolved. Set VT_PROJECT_PATH or --vtproj=<path> to enable runtime validation.");
                    return;
                }

                var workspacePath = TryResolveWorkspacePath();
                var errors = ValidateProject(projectRoot, workspacePath);
                if (errors.Count == 0)
                {
                    Debug.Log("[ConfigValidation] Configs validated successfully.");
                    return;
                }

                LogErrors(errors);
                StopPlaymodeOrQuit();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ConfigValidation] Unexpected error: {ex}");
                StopPlaymodeOrQuit();
            }
        }

        public static List<JsonSchemaValidator.ValidationError> ValidateProject(string projectRoot, string workspacePath)
        {
            var errors = new List<JsonSchemaValidator.ValidationError>();
            if (string.IsNullOrWhiteSpace(projectRoot) || !Directory.Exists(projectRoot))
            {
                errors.Add(new JsonSchemaValidator.ValidationError("$", $"Project root not found: {projectRoot}"));
                return errors;
            }

            var schemaRoot = Path.Combine(Application.streamingAssetsPath, SchemaFolderRelative);
            var schemaPaths = new Dictionary<string, string>
            {
                { "scene", Path.Combine(schemaRoot, "scene.schema.json") },
                { "outputs", Path.Combine(schemaRoot, "outputs.schema.json") },
                { "overlay", Path.Combine(schemaRoot, "overlay.schema.json") },
                { "workspace", Path.Combine(schemaRoot, "workspace.schema.json") }
            };

            errors.AddRange(ValidateFile(Path.Combine(projectRoot, "scene", "scene.json"), schemaPaths["scene"], "scene"));
            errors.AddRange(ValidateFile(Path.Combine(projectRoot, "outputs", "outputs.json"), schemaPaths["outputs"], "outputs"));

            var overlaysRoot = Path.Combine(projectRoot, "ui", "overlays");
            if (Directory.Exists(overlaysRoot))
            {
                var overlayDirs = Directory.GetDirectories(overlaysRoot);
                foreach (var overlayDir in overlayDirs)
                {
                    var overlayPath = Path.Combine(overlayDir, "overlay.json");
                    var overlayName = new DirectoryInfo(overlayDir).Name;
                    errors.AddRange(ValidateFile(overlayPath, schemaPaths["overlay"], $"overlay:{overlayName}"));
                }
            }
            else
            {
                errors.Add(new JsonSchemaValidator.ValidationError("$", $"Overlays folder not found: {overlaysRoot}"));
            }

            if (!string.IsNullOrWhiteSpace(workspacePath))
            {
                errors.AddRange(ValidateFile(workspacePath, schemaPaths["workspace"], "workspace"));
            }

            return errors;
        }

        public static string TryResolveProjectRoot()
        {
            var envPath = Environment.GetEnvironmentVariable("VT_PROJECT_PATH");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                return envPath;
            }

            var argPath = TryGetCommandLineArg("--vtproj");
            if (!string.IsNullOrWhiteSpace(argPath))
            {
                return argPath;
            }

#if UNITY_EDITOR
            var guess = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "examples", "vtproj-minimal", "vtproj"));
            if (Directory.Exists(guess))
            {
                return guess;
            }
#endif

            return null;
        }

        public static string TryResolveWorkspacePath()
        {
            var envPath = Environment.GetEnvironmentVariable("VT_WORKSPACE_PATH");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                return envPath;
            }

            return TryGetCommandLineArg("--workspace");
        }

        private static List<JsonSchemaValidator.ValidationError> ValidateFile(string jsonPath, string schemaPath, string label)
        {
            var errors = new List<JsonSchemaValidator.ValidationError>();
            if (!File.Exists(schemaPath))
            {
                errors.Add(new JsonSchemaValidator.ValidationError("$", $"Schema not found for {label}: {schemaPath}"));
                return errors;
            }

            if (!File.Exists(jsonPath))
            {
                errors.Add(new JsonSchemaValidator.ValidationError("$", $"Config not found for {label}: {jsonPath}"));
                return errors;
            }

            var schemaText = File.ReadAllText(schemaPath);
            var jsonText = File.ReadAllText(jsonPath);
            var validationErrors = JsonSchemaValidator.Validate(jsonText, schemaText);
            if (validationErrors.Count == 0)
            {
                return errors;
            }

            foreach (var error in validationErrors)
            {
                errors.Add(new JsonSchemaValidator.ValidationError(error.Path, $"{label}: {error.Message}"));
            }

            return errors;
        }

        private static void LogErrors(List<JsonSchemaValidator.ValidationError> errors)
        {
            Debug.LogError($"[ConfigValidation] {errors.Count} error(s) found:");
            foreach (var error in errors)
            {
                Debug.LogError($"[ConfigValidation] {error}");
            }
        }

        private static void StopPlaymodeOrQuit()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
#else
            Application.Quit(1);
#endif
        }

        private static string TryGetCommandLineArg(string key)
        {
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (arg.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
                {
                    return arg.Substring(key.Length + 1).Trim('"');
                }
            }
            return null;
        }
    }
}
