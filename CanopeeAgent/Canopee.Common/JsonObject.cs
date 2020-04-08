using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Canopee.Common
{
    public class JsonObject : Dictionary<string, object>
    {
        public static JsonObject LoadFromFile(string filePath)
        {
            string jsonFileContent = File.ReadAllText(filePath);
            return Load(JsonDocument.Parse(jsonFileContent).RootElement);
        }

        public static JsonObject LoadFromJsonDocument(JsonDocument document)
        {
            return Load(document.RootElement);
        }
        
        private static JsonObject Load(in JsonElement element)
        {
            var toReturn = new JsonObject();
            foreach (var jsonProperty in element.EnumerateObject())
            {
                toReturn[jsonProperty.Name] = GetObjectFromJsonProperty(jsonProperty);
            }
            return toReturn;
        }
        
        private static object GetObjectFromJsonProperty(JsonProperty jsonProperty)
        {
            switch (jsonProperty.Value.ValueKind)
            {
                case JsonValueKind.Number:
                {
                    var value = jsonProperty.Value.GetRawText();
                    if (short.TryParse(value, out var shortValue))
                    {
                        return shortValue;
                    }
                    
                    if (int.TryParse(value, out var intValue))
                    {
                        return intValue;
                    }
                    
                    if (Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var floatValue))
                    {
                        return floatValue;
                    }
                    
                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var doubleValue))
                    {
                        return doubleValue;
                    }
                    
                    if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var decimalValue))
                    {
                        return decimalValue;
                    }
                    break;
                }
                case JsonValueKind.False:
                case JsonValueKind.True:
                {
                    return jsonProperty.Value.GetBoolean();
                }
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                {
                    return null;
                }
                case JsonValueKind.Array:
                {
                    var list = new List<object>();
                    foreach (JsonElement element in jsonProperty.Value.EnumerateArray())
                    {
                        list.Add(GetObjectFromJsonElement(element));
                    }

                    return list;
                }
                case JsonValueKind.Object:
                {
                    return GetObjectFromJsonElement(jsonProperty.Value);
                }
                case JsonValueKind.String:
                default:
                {
                    return jsonProperty.Value.GetString();
                }
            }
            throw new NotSupportedException($"Property {jsonProperty.Name}:{jsonProperty.Value.GetRawText()} of type {jsonProperty.Value.ValueKind} is not supported");
        }

        private static object GetObjectFromJsonElement(in JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Number:
                {
                    var value = element.GetRawText();
                    if (short.TryParse(value, out var shortValue))
                    {
                        return shortValue;
                    }
                    else if (int.TryParse(value, out var intValue))
                    {
                        return intValue;
                    }
                    else if (Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var floatValue))
                    {
                        return floatValue;
                    }
                    else if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var doubleValue))
                    {
                        return doubleValue;
                    }
                    else if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                        out var decimalValue))
                    {
                        return decimalValue;
                    }

                    break;
                }
                case JsonValueKind.False:
                case JsonValueKind.True:
                {
                    return element.GetBoolean();
                }
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                {
                    return null;
                }
                case JsonValueKind.Array:
                {
                    var list = new List<object>();
                    foreach (JsonElement childElement in element.EnumerateArray())
                    {
                        list.Add(GetObjectFromJsonElement(childElement));
                    }
                    return list;
                }
                case JsonValueKind.Object:
                {
                    return JsonObject.Load(element);
                }
                case JsonValueKind.String:
                default:
                {
                    return element.GetString();
                }
            }
            throw new NotSupportedException($"Element {element.GetRawText()} of type {element.ValueKind} is not supported.");
        }

        public T GetProperty<T>(string propertyName)
        {
            if (TryGetValue(propertyName, out object valueAsObject))
            {
                if (valueAsObject == null)
                    return default(T);
                
                if (valueAsObject is T valueAsExpectedType)
                {
                    return valueAsExpectedType;
                } 
                
                if (valueAsObject is string)
                {
                    try
                    {
                        if (typeof(T) == typeof(DateTime))
                        {
                            var date = DateTime.Parse(valueAsObject.ToString());
                            if (date is T expectedDate)
                            {
                                return expectedDate;
                            }
                        }
                        else
                        {
                            return JsonSerializer.Deserialize<T>(valueAsObject.ToString());    
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}");
                    }
                }

                if (IsNumeric(valueAsObject))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<T>(valueAsObject.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}");
                    }
                }

                throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}");
            }
            throw new ArgumentOutOfRangeException($"Property {propertyName} not found ! ");
        }

        public bool IsNumeric(object objToTest)
        {
            return objToTest is float ||
                   objToTest is double ||
                   objToTest is long ||
                   objToTest is int ||
                   objToTest is short ||
                   objToTest is ulong ||
                   objToTest is uint ||
                   objToTest is ushort;
        }
        
        public bool TryGetProperty<T>(string propertyName, out T propertyValue)
        {
            try
            {
                propertyValue = GetProperty<T>(propertyName);
                return true;
            }
            catch (Exception ex)
            {
                propertyValue = default(T);
                //swallow exception and do nothing
            }
            return false;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = false
            });
        }

        public void SetProperty(string propertyName, object propertyValue)
        {
            this[propertyName] = propertyValue;
        }

        public void WriteTo(string filePath)
        {
            File.WriteAllText(filePath, this.ToString());
        }

        public static JsonObject CleanDocument(JsonObject config)
        {
            return LoadFromJsonDocument(JsonDocument.Parse(config.ToString()));
        }
    }
}