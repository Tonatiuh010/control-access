using System;
using System.Reflection;

namespace Engine.BO {
    public abstract class ParamProperty<T> 
    {
        private T data;
        public string Name {get; set;}
        public Type Type => data.GetType();
        public T Data
        {
            get => data;
            set => SetValue(value);
        }

        public abstract void SetValue(T t);
    }    
}