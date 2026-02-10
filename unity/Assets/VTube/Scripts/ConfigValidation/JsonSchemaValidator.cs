using System;
using System.Collections.Generic;
using System.Text.Json;

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

            JsonDocument schemaDoc;
            try
            {
                schemaDoc = JsonDocument.Parse(schemaText);
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError("$", $"Schema parse error: {ex.Message}"));
                return errors;
            }

            using (schemaDoc)
            {
                JsonDocument instanceDoc;
                try
                {
                    instanceDoc = JsonDocument.Parse(jsonText);
                }
                catch (Exception ex)
                {
                    errors.Add(new ValidationError("$", BuildParseError(jsonText, ex)));
                    return errors;
                }

                using (instanceDoc)
                {
                    ValidateNode(instanceDoc.RootElement, schemaDoc.RootElement, "$", errors);
                }
            }
            return errors;
        }

        private static void ValidateNode(JsonElement instance, JsonElement schema, string path, List<ValidationError> errors)
        {
            if (schema.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            if (schema.TryGetProperty("type", out var typeNode) && typeNode.ValueKind == JsonValueKind.String)
            {
                var expectedType = typeNode.GetString();
                if (!string.IsNullOrWhiteSpace(expectedType) && !TypeMatches(instance, expectedType))
                {
                    errors.Add(new ValidationError(path, $"Expected type '{expectedType}', got '{DescribeType(instance)}'"));
                    return;
                }
            }

            if (schema.TryGetProperty("enum", out var enumNode) && enumNode.ValueKind == JsonValueKind.Array)
            {
                var matched = false;
                foreach (var item in enumNode.EnumerateArray())
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

            if (instance.ValueKind == JsonValueKind.Number)
            {
                if (schema.TryGetProperty("minimum", out var minNode) && minNode.ValueKind == JsonValueKind.Number)
                {
                    var value = instance.GetDouble();
                    var minValue = minNode.GetDouble();
                    if (value < minValue)
                    {
                        errors.Add(new ValidationError(path, $"Value {value} is less than minimum {minValue}"));
                    }
                }
                if (schema.TryGetProperty("maximum", out var maxNode) && maxNode.ValueKind == JsonValueKind.Number)
                {
                    var value = instance.GetDouble();
                    var maxValue = maxNode.GetDouble();
                    if (value > maxValue)
                    {
                        errors.Add(new ValidationError(path, $"Value {value} is greater than maximum {maxValue}"));
                    }
                }
            }

            if (instance.ValueKind == JsonValueKind.String)
            {
                var value = instance.GetString() ?? string.Empty;
                if (schema.TryGetProperty("minLength", out var minLenNode) && minLenNode.ValueKind == JsonValueKind.Number)
                {
                    var minLen = minLenNode.GetInt32();
                    if (value.Length < minLen)
                    {
                        errors.Add(new ValidationError(path, $"String length {value.Length} is less than minLength {minLen}"));
                    }
                }
                if (schema.TryGetProperty("maxLength", out var maxLenNode) && maxLenNode.ValueKind == JsonValueKind.Number)
                {
                    var maxLen = maxLenNode.GetInt32();
                    if (value.Length > maxLen)
                    {
                        errors.Add(new ValidationError(path, $"String length {value.Length} is greater than maxLength {maxLen}"));
                    }
                }
            }

            if (instance.ValueKind == JsonValueKind.Object)
            {
                ValidateObject(instance, schema, path, errors);
                return;
            }

            if (instance.ValueKind == JsonValueKind.Array)
            {
                ValidateArray(instance, schema, path, errors);
            }
        }

        private static void ValidateObject(JsonElement obj, JsonElement schema, string path, List<ValidationError> errors)
        {
            if (schema.TryGetProperty("required", out var requiredNode) && requiredNode.ValueKind == JsonValueKind.Array)
            {
                foreach (var entry in requiredNode.EnumerateArray())
                {
                    if (entry.ValueKind != JsonValueKind.String)
                    {
                        continue;
                    }

                    var key = entry.GetString();
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        continue;
                    }

                    if (!obj.TryGetProperty(key, out _))
                    {
                        errors.Add(new ValidationError(path, $"Missing required property '{key}'"));
                    }
                }
            }

            var hasProperties = schema.TryGetProperty("properties", out var propertiesNode) && propertiesNode.ValueKind == JsonValueKind.Object;
            var additionalAllowed = true;
            var hasAdditionalSchema = false;
            JsonElement additionalSchema = default;

            if (schema.TryGetProperty("additionalProperties", out var additionalNode))
            {
                if (additionalNode.ValueKind == JsonValueKind.False)
                {
                    additionalAllowed = false;
                }
                else if (additionalNode.ValueKind == JsonValueKind.Object)
                {
                    hasAdditionalSchema = true;
                    additionalSchema = additionalNode;
                }
            }

            foreach (var kv in obj.EnumerateObject())
            {
                var key = kv.Name;
                var value = kv.Value;
                if (hasProperties && propertiesNode.TryGetProperty(key, out var propertySchema))
                {
                    ValidateNode(value, propertySchema, AppendPath(path, key), errors);
                    continue;
                }

                if (hasAdditionalSchema)
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

        private static void ValidateArray(JsonElement array, JsonElement schema, string path, List<ValidationError> errors)
        {
            if (!schema.TryGetProperty("items", out var itemsSchema))
            {
                return;
            }

            var i = 0;
            foreach (var item in array.EnumerateArray())
            {
                ValidateNode(item, itemsSchema, $"{path}[{i}]", errors);
                i++;
            }
        }

        private static bool TypeMatches(JsonElement node, string expectedType)
        {
            switch (expectedType)
            {
                case "object":
                    return node.ValueKind == JsonValueKind.Object;
                case "array":
                    return node.ValueKind == JsonValueKind.Array;
                case "string":
                    return node.ValueKind == JsonValueKind.String;
                case "number":
                    return node.ValueKind == JsonValueKind.Number;
                case "integer":
                    if (node.ValueKind != JsonValueKind.Number)
                    {
                        return false;
                    }
                    if (node.TryGetInt64(out _))
                    {
                        return true;
                    }
                    var value = node.GetDouble();
                    return Math.Abs(value - Math.Round(value)) < 0.0000001;
                case "boolean":
                    return node.ValueKind == JsonValueKind.True || node.ValueKind == JsonValueKind.False;
                case "null":
                    return node.ValueKind == JsonValueKind.Null;
                default:
                    return true;
            }
        }

        private static string DescribeType(JsonElement node)
        {
            switch (node.ValueKind)
            {
                case JsonValueKind.Object:
                    return "object";
                case JsonValueKind.Array:
                    return "array";
                case JsonValueKind.String:
                    return "string";
                case JsonValueKind.Number:
                    return "number";
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return "boolean";
                case JsonValueKind.Null:
                    return "null";
                default:
                    return "unknown";
            }
        }

        private static bool JsonEquals(JsonElement left, JsonElement right)
        {
            if (left.ValueKind != right.ValueKind)
            {
                return false;
            }

            switch (left.ValueKind)
            {
                case JsonValueKind.Number:
                    return Math.Abs(left.GetDouble() - right.GetDouble()) < 0.0000001;
                case JsonValueKind.String:
                    return string.Equals(left.GetString(), right.GetString(), StringComparison.Ordinal);
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return left.GetBoolean() == right.GetBoolean();
                case JsonValueKind.Null:
                    return true;
                default:
                    return false;
            }
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
