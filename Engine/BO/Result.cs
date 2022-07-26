using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.BO {
    public class Result {
        public string? Status {get; set;}
        public string? Message {get; set;}
        public object? Data {get; set;} = null;
        public object? Data2 {get; set;} = null;
        public object? Data3 {get; set;} = null;
        public string? Error {get; set; }
    }
}