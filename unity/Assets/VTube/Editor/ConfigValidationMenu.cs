#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VTube.ConfigValidation;

namespace VTube.Editor
{
    public static class ConfigValidationMenu
    {
        [MenuItem("VTube/Validate Configs")]
        private static void ValidateConfigs()
        {
            var projectRoot = ConfigValidationRunner.TryResolveProjectRoot();
            if (string.IsNullOrWhiteSpace(projectRoot) || !Directory.Exists(projectRoot))
            {
                projectRoot = EditorUtility.OpenFolderPanel("Select vtproj folder", "", "");
            }

            if (string.IsNullOrWhiteSpace(projectRoot))
            {
                Debug.LogWarning("[ConfigValidation] Validation cancelled: no project root selected.");
                return;
            }

            var workspacePath = ConfigValidationRunner.TryResolveWorkspacePath();
            if (string.IsNullOrWhiteSpace(workspacePath))
            {
                workspacePath = EditorUtility.OpenFilePanel("Select workspace.json (optional)", projectRoot, "json");
            }
            if (string.IsNullOrWhiteSpace(workspacePath))
            {
                workspacePath = null;
            }

            List<JsonSchemaValidator.ValidationError> errors = ConfigValidationRunner.ValidateProject(projectRoot, workspacePath);
            if (errors.Count == 0)
            {
                Debug.Log("[ConfigValidation] Configs validated successfully.");
                return;
            }

            Debug.LogError($"[ConfigValidation] {errors.Count} error(s) found:");
            foreach (var error in errors)
            {
                Debug.LogError($"[ConfigValidation] {error}");
            }
        }
    }
}
#endif
