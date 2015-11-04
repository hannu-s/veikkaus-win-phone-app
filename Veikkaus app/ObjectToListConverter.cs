using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veikkaus_app
{
    public class ObjectToListConverter<T> : CustomCreationConverter<List<T>> where T : new()
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var target = new List<T>();
            var jObject = JObject.Load(reader);
            var t = new T();
            serializer.Populate(jObject.CreateReader(), t);
            target.Add(t);

            return target;
        }

        public override List<T> Create(Type objectType)
        {
            return new List<T>();
        }
    }

    public class ObjectToArrayConverter<T> : CustomCreationConverter<List<T>> where T : new()
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<T> target = new List<T>();

            try
            {
                var jArray = JArray.Load(reader);
                serializer.Populate(jArray.CreateReader(), target);
            }
            catch (JsonReaderException)
            {
                var jObject = JObject.Load(reader);
                var t = new T();
                serializer.Populate(jObject.CreateReader(), t);
                target.Add(t);
            }

            return target;
        }

        public override List<T> Create(Type objectType)
        {
            return new List<T>();
        }
    }
}
