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

    public class CheckBase : BaseBO
    {
        public DateTime? CheckDt { get; set; }
        public string? Type { get; set; }        
        
    }

    public class Check : CheckBase {
        public delegate Device? FindDevice(int? deviceId);
        private FindDevice? GetDevice { get; set; }
        public CardEmployee? Card {get; set;}        

        [JsonIgnore]
        public int? Device { get; set; }

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

    public class CheckDetails : CheckBase
    {        
        public Position? Position { get; set; }


        public static List<IntervalDepto> GetChecksByDepto(List<CheckDetails> checks, DateTime pivot)
        {
            List<IntervalDepto> intervalDeptos = new ();          

            List<Period> periods = new()
            {
                new Period()
                {
                    From = pivot.AddHours(6),
                    To = pivot.AddHours(10)
                },
                new Period()
                {
                    From = pivot.AddHours(10),
                    To = pivot.AddHours(16)
                },
                new Period()
                {
                    From = pivot.AddHours(16),
                    To = pivot.AddHours(20)
                }
            };            

            foreach(var x in checks.GroupBy(x => x.Position.Departament.Code)) 
            {
                var list = x.ToList();
                var deptoInterval = new IntervalDepto()
                {
                    Name = x.Key
                };

                foreach(var p in periods)
                {
                    DeptoCounter deptoStats = new ();

                    var periodChecks = list.Where(
                        x => p.IsRange((DateTime)x.CheckDt)
                    ).ToList();

                    if(periodChecks.Count > 0)
                    {
                        var depto = periodChecks[0].Position?.Departament;

                        deptoStats.Period = p;
                        deptoStats.Departament = depto;
                        deptoStats.Checks = periodChecks.Count;
                    }

                    deptoInterval.Sets.Add(deptoStats);
                }

                intervalDeptos.Add(deptoInterval);
            }

            return intervalDeptos;
        }

    }

    public class Period
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public bool IsRange(DateTime dt) => dt >= From && dt <= To;
    }

    public class IntervalDepto
    {
        public string? Name { get; set; }
        public List<DeptoCounter> Sets { get; set; } = new ();

        public Period Period => new() {
            From = Sets.Min(x => x.Period.From),
            To = Sets.Max(x => x.Period.To)
        };        
    }

    public class DeptoCounter { 
        public Departament? Departament { get; set; }
        public Period Period { get; set; } = new();
        public int Checks { get; set; }
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