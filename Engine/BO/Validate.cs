using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO
{
    /// <summary>
    /// Clase de validaciones
    /// </summary>
    public class Validate
    {
        private static Validate _instance = null;
        private Validate() { }
        //Singleton initiation
        public static Validate Instance
        {
            get
            {
                if (_instance == null) _instance = new Validate();
                return _instance;
            }
        }
        #region General Methods

        public object getDefaultIfNull(string obj, TypeCode typeCode)
        {
            //If object is dbnull then return the default for that type.
            //Otherwise just return the orginal value.
            object obj2 = obj;
            if (obj == "")
            {
                switch (typeCode)
                {
                    case TypeCode.Int32:
                        obj2 = 0;
                        break;
                    case TypeCode.Double:
                        obj2 = 0;
                        break;
                    case TypeCode.String:
                        obj2 = string.Empty;
                        break;
                    case TypeCode.Boolean:
                        obj2 = false;
                        break;
                    case TypeCode.DateTime:
                        obj2 = new DateTime();
                        break;
                    case TypeCode.Int64:
                        obj2 = 0;
                        break;
                    default:
                        break;
                }
            }
            return obj2;
        }

        #endregion

        #region DAL Methods
        /// <summary>
        /// Checks if an object coming back from the database is dbnull.  If it is this returns the default
        /// value for that type of object.
        /// </summary>
        /// <param name="obj">Object to check for null.</param>
        /// <param name="typeCode">Type of object, used to determine what the default value is.</param>
        /// <returns>Either the object passed in or the default value.</returns>
        public object getDefaultIfDBNull(object obj, TypeCode typeCode)
        {
            //If object is dbnull then return the default for that type.
            //Otherwise just return the orginal value.
            if (obj == DBNull.Value)
            {
                switch (typeCode)
                {
                    case TypeCode.Int32:
                        obj = 0;
                        break;
                    case TypeCode.Double:
                        obj = 0;
                        break;
                    case TypeCode.String:
                        obj = "";
                        break;
                    case TypeCode.Boolean:
                        obj = false;
                        break;
                    case TypeCode.DateTime:
                        obj = new DateTime();
                        break;
                    case TypeCode.Int64:
                        obj = 0;
                        break;                    
                    default:
                        break;
                }
            }
            return obj;
        }

        public string getDefaultStringIfDBNull(object obj)
        {
            return Convert.ToString(getDefaultIfDBNull(obj, TypeCode.String));
        }
        public int getDefaultIntIfDBNull(object obj)
        {
            return Convert.ToInt32(getDefaultIfDBNull(obj, TypeCode.Int32));
        }

        public double getDefaultDoubleIfDBNull(object obj)
        {
            return Convert.ToDouble(getDefaultIfDBNull(obj, TypeCode.Int32));
        }

        public decimal getDefaultDecimalIfDBNull(object obj)
        {
            return Convert.ToDecimal(getDefaultIfDBNull(obj, TypeCode.Int32));
        }

        public DateTime getDefaultDateIfDBNull(object obj)
        {
            return Convert.ToDateTime(getDefaultIfDBNull(obj, TypeCode.DateTime));
        }

        public TimeSpan getDefaultTimeSpanIfDBNull(object obj) 
        {
            if (obj != null && obj.GetType() == typeof(TimeSpan)) {                
                return (TimeSpan)obj;
            } else {
                return new TimeSpan();
            }            
        }

        public long getDefaultLongIfDBNull(object obj)
        {
            return Convert.ToInt64(getDefaultIfDBNull(obj, TypeCode.DateTime));
        }

        public object getNullFromDate(DateTime valor)
        {
            if (valor == DateTime.MinValue)
                return DBNull.Value;
            else
                return valor;
        }

        public object getNullFromInt(int valor)
        {
            if (valor <= 0)
                return DBNull.Value;
            else
                return valor;
        }

        #endregion
    }
}