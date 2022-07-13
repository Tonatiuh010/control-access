using System;
using Engine.BO;
using Newtonsoft.Json.Linq;

namespace Classes {
    public class JsonProperty<T> : ParamProperty<T> {
        private JObject Object {get; set;}

        public JsonProperty(string name, JObject jObj){
            Name = name;
            Object = jObj;
        }
        
        public override void SetValue(T t)
        {                    
            try {
                Data = t;
            } catch {
                Data = default(T);
            }
        }
    }
}