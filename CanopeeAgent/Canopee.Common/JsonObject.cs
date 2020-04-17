using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Canopee.Common
{
    /// <summary>
    /// This class represent a json structure as an object, and allow some features as load from a text file, a JsonDocument, and  write a json to a file.
    /// This class was created to manage issues on managing configuration file easily and dynamically, but it is also useful for exchange between purpose.
    /// </summary>
    public class JsonObject : Dictionary<string, object>
    {
        /// <summary>
        /// Load a file that contains a json structure into a <see cref="JsonObject"/>
        /// </summary>
        /// <param name="filePath">the filePath to load</param>
        /// <returns>The <see cref="JsonObject"/> loaded</returns>
        public static JsonObject LoadFromFile(string filePath)
        {
            string jsonFileContent = File.ReadAllText(filePath);
            return Load(JsonDocument.Parse(jsonFileContent).RootElement);
        }

        /// <summary>
        /// Load a JsonDocument object into a <see cref="JsonObject"/></summary>
        /// <param name="document">the document to load</param>
        /// <returns>the <see cref="JsonObject"/> loaded</returns>
        public static JsonObject LoadFromJsonDocument(JsonDocument document)
        {
            return Load(document.RootElement);
        }
        
        /// <summary>
        /// Load all child JsonElement from this element into a <see cref="JsonObject"/>
        /// </summary>
        /// <param name="element">the root JsonElement to load from</param>
        /// <returns>the <see cref="JsonObject"/> loaded</returns>
        private static JsonObject Load(in JsonElement element)
        {
            var toReturn = new JsonObject();
            foreach (var jsonProperty in element.EnumerateObject())
            {
                toReturn[jsonProperty.Name] = GetObjectFromJsonProperty(jsonProperty);
            }
            return toReturn;
        }
        
        /// <summary>
        /// Get the C# object from a JsonProperty depending on the JsonValueKind :
        /// - Number will be converted into short, int, long, single, double, decimal (test are made in this order)
        /// - boolean will be return as boolean
        /// - null or undefined as null
        /// - array as a list of object, and each element will be converted in the C# corresponding object
        /// - object will be converted to a <see cref="JsonObject"/>
        /// - string to string
        /// The associated key will be the name of the json property, respecting the case
        /// </summary>
        /// <param name="jsonProperty">the JsonProperty to convert to C# object</param>
        /// <returns>an object that correspond to the JsonValueKind of the JsonProperty</returns>
        /// <exception cref="NotSupportedException"></exception>
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

        /// <summary>
        /// Get the corresponding C# object from a JsonElement depending on its JsonValueKind :
        /// - Number will be converted into short, int, long, single, double, decimal (test are made in this order)
        /// - boolean will be return as boolean
        /// - null or undefined as null
        /// - array as a list of object, and each element will be converted in the C# corresponding object
        /// - object will be converted to a <see cref="JsonObject"/>
        /// - string to string
        /// The associated key will be the name of the json property, respecting the case
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
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

        /// <summary>
        /// Get the value of a property stored in the JsonObject in the type expected.
        /// </summary>
        /// <param name="propertyName">the property you want to get the value</param>
        /// <typeparam name="T">the type of the value expected</typeparam>
        /// <returns>the value as type expected</returns>
        /// <exception cref="NotSupportedException">If no conversion of type can be done</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the property name doesn't exist in this <see cref="JsonObject"/></exception>
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
                        throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}", ex);
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
                        throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}", ex);
                    }
                }

                throw new NotSupportedException($"Property {propertyName} is not of type {typeof(T).FullName}");
            }
            throw new ArgumentOutOfRangeException($"Property {propertyName} not found ! ");
        }

        /// <summary>
        /// test if the is of numeric type
        /// </summary>
        /// <param name="objToTest">an object to test</param>
        /// <returns>True if the object is float, double, long, int, short, ulong, uint, ushort</returns>
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
        
        /// <summary>
        /// Try to get a property and return false if the property is not in the JSonObject
        /// </summary>
        /// <param name="propertyName">the property you want to get</param>
        /// <param name="propertyValue">the reference value you want to set from the property</param>
        /// <typeparam name="T">the type of the value expected</typeparam>
        /// <returns>true if the value is in it, false if the value is not</returns>
        public bool TryGetProperty<T>(string propertyName, out T propertyValue)
        {
            try
            {
                propertyValue = GetProperty<T>(propertyName);
                return true;
            }
            catch
            {
                propertyValue = default(T);
                //swallow exception and do nothing
            }
            return false;
        }

        /// <summary>
        /// Get the stringified version of the current <see cref="JsonObject"/> 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = false
            });
        }

        /// <summary>
        /// Set or Add a property to the current <see cref="JsonObject"/>
        /// </summary>
        /// <param name="propertyName">Name of the property to set</param>
        /// <param name="propertyValue">Value of the property to set</param>
        public void SetProperty(string propertyName, object propertyValue)
        {
            this[propertyName] = propertyValue;
        }

        /// <summary>
        /// Write the current <see cref="JsonObject"/> to the specified filepath
        /// </summary>
        /// <param name="filePath">the filepath to write to</param>
        public void WriteTo(string filePath)
        {
            File.WriteAllText(filePath, this.ToString());
        }

        /// <summary>
        /// Convert all JsonDocument that may have been loaded from a webcontext without correct conversion
        /// </summary>
        /// <param name="incompleteLoadedJsonObject">a JsonObject that may have not been loaded correctly (through <see cref="JsonObject.LoadFromFile"/> or <see cref="JsonObject.LoadFromJsonDocument"/></param>
        /// <returns>A clean and correctly load JsonObject with complete conversion of child element</returns>
        public static JsonObject CleanDocument(JsonObject incompleteLoadedJsonObject)
        {
            if(incompleteLoadedJsonObject != null)
                return LoadFromJsonDocument(JsonDocument.Parse(incompleteLoadedJsonObject.ToString()));
            return null;
        }
    }
}