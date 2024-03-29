using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using Engine.Constants;

namespace Engine.BO {

    public class BaseBO {
        public int? Id {get; set;}
        public bool IsValid() => Id != null && Id != 0;
    }

    public class ImageData
    {
        [JsonIgnore]
        public byte[]? Bytes { get; set; }
        [JsonIgnore]
        public string? B64 => Bytes != null ? AddB64Header( Convert.ToBase64String(Bytes) ) : string.Empty;
        [JsonIgnore]
        public string? Hex { get
            {
                if(Bytes != null )
                {                    
                    return BitConverter.ToString(Bytes).Replace("-", "");
                } else
                {
                    return null;
                }                
            }
        }

        public string? Url { get; set; }

        public ImageData() => Bytes = null;

        public ImageData(byte[] bytes) => Bytes = bytes;

        public ImageData(string? b64) => 
            Bytes = string.IsNullOrEmpty(b64) ? null : Convert.FromBase64String(RemoveHeaderB64(b64));

        public static byte[] GetBytesFromUrl(string url) => Utils.GetImage(url);

        private static string RemoveHeaderB64(string b64)
        {
            var indexOf = b64.IndexOf(",");

            if(indexOf == -1)
            {
                return b64;
            } else
            {
                return b64.Substring(indexOf + 1);
            }            
        }

        private static string AddB64Header(string b64)
        {
            var indexOf = b64.IndexOf(",");

            if (indexOf != -1)
            {
                return b64;
            }
            else
            {
                return $"data:image/png;base64,{b64}";
            }
        }

    }

    public class ControlAccess  : BaseBO{
        public string? Name {get; set;}
        public string? LastName {get; set;}
        public ImageData? Image { get; set; }
        public Position? Job {get; set;}
        public List<AccessLevel>? AccessLevels {get; set;}
        public Shift? Shift {get; set;}
        public Card? Card { get; set; }
        public string? Status { get; set; }
    }

    public class Job : BaseBO {
        public string? Name {get; set;}
        public string? Description {get; set;}
    }

    public class Position : Job {
        public int? PositionId {get; set;}
        public string? Alias {get; set;}
        public Departament? Departament {get; set;}

        public bool IsValidPosition() => IsValid() && PositionId != null && PositionId != 0;
    }

    public class AccessLevel : BaseBO {        
        public string? Name {get; set;}
        [JsonIgnore]
        public string? Status {get; set;}                                   
    }

    public class EmployeeAccessLevel : AccessLevel {
        public int? EmployeeId {get; set;}
        public bool IsValidEmployeeAccessLevel() => IsValid() && EmployeeId != null && EmployeeId != 0;

        public static List<AccessLevel> GetAccessLevels(List<EmployeeAccessLevel> levels) => 
            levels.Select(x => (AccessLevel)x).ToList();
    }

    public class Departament : BaseBO {
        public string? Name {get; set;}
        public string? Code {get; set;}
    }

    public class Card : BaseBO
    {
        public string? Key { get; set; }
        public string? Status { get; set; }

        public Card()
        {
            Key = null;
            Status = null;
        }

    }

    public class CardEmployee : Card {
        public delegate ControlAccess? FindEmployee(int? employeeId);
        private FindEmployee? GetEmployee { get; set; }

        [JsonIgnore]
        public int? Employee {get; set;}

        [JsonPropertyName("employee")]
        public ControlAccess? EmployeeDetails => GetEmployee != null ? GetEmployee(Employee) : null;

        public void SetEmployeeFinder(FindEmployee employeeCallback) => GetEmployee = employeeCallback;

        public CardEmployee(FindEmployee? getEmployee, int? employee)
        {
            GetEmployee = getEmployee;
            Employee = employee;
        }

        public CardEmployee(Card card, int? employee)
        {
            Id = card.Id;
            Key = card.Key;
            Employee = employee;
        }

        public CardEmployee(int? employee)
        {
            Employee = employee;
        }

        public CardEmployee()
        {
            Employee = null;
        }

        public CardEmployee UnbindEmployee() => new(this, null);        

    }

    public class Check : BaseBO {
        public delegate Device? FindDevice(int? deviceId);
        private FindDevice? GetDevice { get; set; }

        [JsonIgnore]
        public int? Device { get; set; }

        public DateTime? CheckDt {get; set;}
        public string? Type {get; set;}
        public CardEmployee? Card {get; set;}        

        [JsonPropertyName("device")]
        public Device? DeviceDetails => GetDevice != null ? GetDevice(Device) : null;

        public void SetDeviceFinder(FindDevice deviceCallback) => GetDevice = deviceCallback;
    }

    public class AccessCheck
    {
        public Check? Check { get; set; }
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }

    public class Device : BaseBO
    {        

        public string? Name { get; set; }
        public string? Status { get; set; }

        [JsonPropertyName("last_update")]
        public DateTime? LastUpdate { get; set; }

        public int? Activations { get; set; }
        public int? Unsuccessful { get; set; }  
    }

    public class Shift : BaseBO {
        public string? Name {get; set;}
        public TimeSpan? InTime {get; set;}
        public TimeSpan? OutTime {get; set;}
        public TimeSpan? LunchTime {get; set;}
        public int? DayCount {get; set;}

        public Shift() {
            Id = null;
            Name = null;
            InTime = null;
            OutTime = null;
            LunchTime = null;
            DayCount = null;
        }

        public Shift(string? inTime, string? outTime) {
            InTime = ConvertTime(inTime);
            OutTime = ConvertTime(outTime);
        }

        public static TimeSpan ConvertTime(string? timeExpression) {
            try {
                if(timeExpression != null) {
                    return TimeSpan.Parse(timeExpression);
                } else {
                    return new TimeSpan();
                }
            } catch {
                return new TimeSpan();
            }
        }
    }

}