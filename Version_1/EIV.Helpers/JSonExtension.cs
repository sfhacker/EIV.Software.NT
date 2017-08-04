/// <summary>
/// 
/// </summary>
namespace EIV.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    // Install-Package Newtonsoft.Json
    public static class JSonExtension
    {
        private static string errorMessage = string.Empty;

        public static string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }

        // OData Entities to JSon string
        // Only one entity
        // Maybe 'T' is not needed ??? mmm
        // TODO: Revise (it requires the OData.Client libraries!!!)
        //public static string EntityToJSon(this object entity)
        public static string EntityToJSon<T>(this object entity) where T : class
        {
            if (entity == null)
            {
                return null;
            }

            Type classType = typeof(T);
            Type entityType = entity.GetType();

            // Paranoic!
            if (!classType.Equals(entityType))
            {
                return null;
            }

            // yyyy-MM-dd : OLingo likes this format!
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd"; // "dd/MM/yyyy";
            // settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ContractResolver = new EmptyCollectionContractResolver();
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            string json = JsonConvert.SerializeObject(entity, Formatting.Indented, settings);

            return json;
        }

        public static string DictionaryToJSon(this IDictionary<string, object> source)
        {
            if (source == null)
            {
                return null;
            }
            // try ... catch here!
            JObject content = JObject.FromObject(source);
            if (content != null)
            {
                return content.ToString();
            }

            return null;
        }

        // Always an array
        // The controller should always send an array, regardless of the no of items
        // For example, GET http://........ (Id=1)
        // It should return an array
        public static IList<T> JSonToEntities<T>(this string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            if (json.Length < 2)
            {
                return null;
            }

            errorMessage = string.Empty;

            JObject testObj = null;
            try
            {
                testObj = JObject.Parse(json);
            }
            catch (JsonReaderException readerEx)
            {
                errorMessage = readerEx.Message;

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return null;
            }
            if (testObj == null)
            {
                return null;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            /* settings.DateFormatString = "YYYY-MM-DD"; */
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ContractResolver = new EmptyCollectionContractResolver();
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            JToken jsonValue = testObj.GetValue("value");
            if (jsonValue == null)
            {
                T entity = JsonConvert.DeserializeObject<T>(json, settings);

                if (entity == null)
                {
                    return null;
                }

                IList<T> rst = new List<T>();
                rst.Add(entity);

                return rst;
            }
            if (jsonValue.Type == JTokenType.Array)
            {
                IList<T> entityList = JsonConvert.DeserializeObject<IList<T>>(jsonValue.ToString(), settings);

                return entityList;
            }

            return null;
        }

        public static bool JSonValidate(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            if (json.Length < 2)
            {
                return false;
            }

            errorMessage = string.Empty;

            JObject testObj = null;
            try
            {
                testObj = JObject.Parse(json);
            }
            catch (JsonReaderException readerEx)
            {
                errorMessage = readerEx.Message;

                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return false;
            }
            if (testObj == null)
            {
                return false;
            }

            return true;
        }
    }
    internal sealed class EmptyCollectionContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            Predicate<object> shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => (shouldSerialize == null || shouldSerialize(obj)) && !IsEmptyCollection(property, obj);
            return property;
        }

        private bool IsEmptyCollection(JsonProperty property, object target)
        {
            var value = property.ValueProvider.GetValue(target);
            var collection = value as ICollection;
            if (collection != null && collection.Count == 0)
                return true;

            if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                return false;

            var countProp = property.PropertyType.GetProperty("Count");
            if (countProp == null)
                return false;

            var count = (int)countProp.GetValue(value, null);
            return count == 0;
        }
    }
}