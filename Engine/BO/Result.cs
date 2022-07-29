using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Engine.BO {
    public class Result {
        public string? Status {get; set;}
        public string? Message {get; set;}
        public object? Data {get; set;} = null;
        public object? Data2 {get; set;} = null;
    }

    public class ResultInsert : Result
    {
        public InsertStatus InsertDetails { get; set; } = new InsertStatus(new BaseBO());
    }

    public class InsertStatus : BaseBO
    {
        public string ObjectType => FromObject != null? FromObject.ToString() : "NOT ASSOCIATED OBJECT";
        [JsonIgnore]
        public Type? FromObject { get; set; }
        public DateTime InsertDate { get; set; }

        public InsertStatus(BaseBO baseBO)
        {

            Id = baseBO.Id;
            FromObject = baseBO.GetType();
            InsertDate = DateTime.Now;
        }

        public InsertStatus()
        {
            Id = null;
            FromObject = typeof(object);
            InsertDate = DateTime.Now;
        }
    }
}