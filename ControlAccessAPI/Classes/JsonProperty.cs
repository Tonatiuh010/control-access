using System;
using Engine.BO;
using Newtonsoft.Json.Linq;
using Engine.BL.Delegates;

namespace Classes {
    public static class JsonProperty<T> {
        public static T? GetValue(string name, JObject jObj, Delegates.CallbackExceptionMsg? onMissingProperty = null) {
            T? result = default(T);

            try {
                var jKey = jObj[name];
                if(jKey != null) {
                    result = jKey.Value<T?>();
                }
            } catch (Exception ex){
                result = default(T);
                if(onMissingProperty != null) {
                    onMissingProperty(ex, $"Property {name} is missing");
                }
            }

            return result;
        }
    }

    public class RequestError {
        public string? Info {get; set;}
        public Exception? Exception {get; set;}
    
        public string FormatError {get { 
            string format = $"Error -> {Info}";

            if(Exception != null) {
                format = $"{format} - Details {Exception.Message}. \n Source {Exception.Source}. \n On {Exception.StackTrace}";
            }

            return format;
        }}

        public static string FormatErrors(List<RequestError> errors) {
            string result  = string.Empty;


            if(errors != null && errors.Count() > 0 ) {
                foreach(var err in errors) {
                    result += $"\n\t * {err.FormatError} ";
                }
            }

            return result;
        }
    }
}