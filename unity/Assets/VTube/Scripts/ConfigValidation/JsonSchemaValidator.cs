using System;
using System.Collections.Generic;
using System.Text.Json;
using com.IvanMurzak.Unity.MCP.Installer.SimpleJSON;

namespace VTube.ConfigValidation
{
    public static class JsonSchemaValidator
    {
        public sealed class ValidationError
        {
            public ValidationError(string path, string message)
            {
                Path = path;
                Message = message;
            }

            public string Path { get; }
            public string Message { get; }

            public override string ToString()
            {
                return $"{Path}: {Message}";
            }
        }

        public static List<ValidationError> Validate(string jsonText, string schemaText)
        {
            var errors = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(schemaText))
            {
                errors.Add(new ValidationError("$", "Schema text is empty"));
                return errors;
            }

            JSONNode schemaNode;
            try
            {
                schemaNode = JSONNode.Parse(schemaText);
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError("$", $"Schema parse error: {ex.Message}"));
                return errors;
            }

            JSONNode instanceNode;
            try
            {
                instanceNode = JSONNode.Parse(jsonText);
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError("$", BuildParseError(jsonText, ex)));
                return errors;
            }

            ValidateNode(instanceNode, schemaNode, "$", errors);
            return errors;
        }

        private static void ValidateNode(JSONNode instance, JSONNode schema, string path, List<ValidationError> errors)
        {
            if (schema == null)
            {
                return;
            }

            var typeNode = schema["type"];
            if (typeNode != null && typeNode.IsString)
            {
                var expectedType = typeNode.Value;
                if (!TypeMatches(instance, expectedType))
                {
                    errors.Add(new ValidationError(path, $"Expected type '{expectedType}', got '{DescribeType(instance)}'"));
                    return;
                }
            }

            var enumNode = schema["enum"] as JSONArray;
            if (enumNode != null && enumNode.Count > 0)
            {
                var matched = false;
                foreach (var item in enumNode)
                {
                    if (JsonEquals(instance, item))
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    errors.Add(new ValidationError(path, "Value is not in enum"));
                    return;
                }
            }

            if (instance != null && instance.IsNumber)
            {
                var minNode = schema["minimum"];
                var maxNode = schema["maximum"];
                var value = instance.AsDouble;
                if (minNode != null && minNode.IsNumber && value < minNode.AsDouble)
                {
                    errors.Add(new ValidationError(path, $"Value {value} is less than minimum {minNode.AsDouble}"));
                }
                if (maxNode != null && maxNode.IsNumber && value > maxNode.AsDouble)
                {
                    errors.Add(new ValidationError(path, $"Value {value} is greater than maximum {maxNode.AsDouble}"));
                }
            }

            if (instance != null && instance.IsString)
            {
                var minLenNode = schema["minLength"];
                var maxLenNode = schema["maxLength"];
                var value = instance.Value ?? string.Empty;
                if (minLenNode != null && minLenNode.IsNumber && value.Length < minLenNode.AsInt)
                {
                    errors.Add(new ValidationError(path, $"String length {value.Length} is less than minLength {minLenNode.AsInt}"));
                }
                if (maxLenNode != null && maxLenNode.IsNumber && value.Length > maxLenNode.AsInt)
                {
                    errors.Add(new ValidationError(path, $"String length {value.Length} is greater than maxLength {maxLenNode.AsInt}"));
                }
            }

            if (instance == null)
            {
                return;
            }

            if (instance.IsObject)
            {
                ValidateObject(instance.AsObject, schema, path, errors);
                return;
            }

            if (instance.IsArray)
            {
                ValidateArray(instance.AsArray, schema, path, errors);
            }
        }

        private static void ValidateObject(JSONObject obj, JSONNode schema, string path, List<ValidationError> errors)
        {
            var requiredNode = schema["required"] as JSONArray;
            if (requiredNode != null)
            {
                foreach (var entry in requiredNode)
                {
                    var key = entry.Value;
                    if (!obj.HasKey(key))
                    {
                        errors.Add(new ValidationError(path, $"Missing required property '{key}'"));
                    }
                }
            }

            var propertiesNode = schema["properties"] as JSONObject;
            var additionalNode = schema["additionalProperties"];
            var additionalAllowed = true;
            JSONNode additionalSchema = null;

            if (additionalNode != null)
            {
                if (additionalNode.IsBoolean)
                {
                    additionalAllowed = additionalNode.AsBool;
                }
                else if (additionalNode.IsObject)
                {
                    additionalSchema = additionalNode;
                }
            }

            foreach (var kv in obj)
            {
                var key = kv.Key;
                var value = kv.Value;
                if (propertiesNode != null && propertiesNode.HasKey(key))
                {
                    ValidateNode(value, propertiesNode[key], AppendPath(path, key), errors);
                    continue;
                }

                if (additionalSchema != null)
                {
                    ValidateNode(value, additionalSchema, AppendPath(path, key), errors);
                    continue;
                }

                if (!additionalAllowed)
                {
                    errors.Add(new ValidationError(AppendPath(path, key), "Property is not allowed"));
                }
            }
        }

        private static void ValidateArray(JSONArray array, JSONNode schema, string path, List<ValidationError> errors)
        {
            var itemsSchema = schema["items"];
            if (itemsSchema == null)
            {
                return;
            }

            for (var i = 0; i < array.Count; i++)
            {
                ValidateNode(array[i], itemsSchema, $"{path}[{i}]", errors);
            }
        }

        private static bool TypeMatches(JSONNode node, string expectedType)
        {
            switch (expectedType)
            {
                case "object":
                    return node != null && node.IsObject;
                case "array":
                    return node != null && node.IsArray;
                case "string":
                    return node != null && node.IsString;
                case "number":
                    return node != null && node.IsNumber;
                case "integer":
                    if (node == null || !node.IsNumber)
                    {
                        return false;
                    }
                    var value = node.AsDouble;
                    return Math.Abs(value - Math.Round(value)) < 0.0000001;
                case "boolean":
                    return node != null && node.IsBoolean;
                case "null":
                    return node == null || node.IsNull;
                default:
                    return true;
            }
        }

        private static string DescribeType(JSONNode node)
        {
            if (node == null || node.IsNull)
            {
                return "null";
            }
            if (node.IsObject)
            {
                return "object";
            }
            if (node.IsArray)
            {
                return "array";
            }
            if (node.IsString)
            {
                return "string";
            }
            if (node.IsNumber)
            {
                return "number";
            }
            if (node.IsBoolean)
            {
                return "boolean";
            }
            return "unknown";
        }

        private static bool JsonEquals(JSONNode left, JSONNode right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            if (left.IsNumber && right.IsNumber)
            {
                return Math.Abs(left.AsDouble - right.AsDouble) < 0.0000001;
            }
            if (left.IsString && right.IsString)
            {
                return string.Equals(left.Value, right.Value, StringComparison.Ordinal);
            }
            if (left.IsBoolean && right.IsBoolean)
            {
                return left.AsBool == right.AsBool;
            }
            if (left.IsNull && right.IsNull)
            {
                return true;
            }
            return false;
        }

        private static string AppendPath(string path, string key)
        {
            return string.IsNullOrEmpty(path) ? key : $"{path}.{key}";
        }

        private static string BuildParseError(string jsonText, Exception ex)
        {
            try
            {
                JsonDocument.Parse(jsonText);
            }
            catch (JsonException jsonEx)
            {
                var line = jsonEx.LineNumber + 1;
                var pos = jsonEx.BytePositionInLine + 1;
                return $"JSON parse error at line {line}, byte {pos}: {jsonEx.Message}";
            }

            return $"JSON parse error: {ex.Message}";
        }
    }
}
