using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.BO {

    public abstract class BaseBO {
        public int? Id {get; set;}
        public bool IsValid() => Id != null && Id != 0;
    }

    public class Employee  : BaseBO{
        public string? Name {get; set;}
        public string? LastName {get; set;}
        public string? ImageUrl { get; set; }
        public Position? Job {get; set;}
        public List<AccessLevel>? AccessLevels {get; set;}
        public Shift? Shift {get; set;}
        public Card? Card { get; set; }
        public string? Status { get; set; }
    }

    public class Job : BaseBO {
        public string? Name {get; set;}
        public string? Specialist {get; set;}
        public string? Description {get; set;}
    }

    public class Position : Job {
        public int? PositionId {get; set;}
        public string? Code { get; set;}
        public string? Alias {get; set;}
        public Departament? Departament {get; set;}

        public bool IsValidPosition() => IsValid() && PositionId != null && PositionId != 0;
    }

    public class AccessLevel : BaseBO {        
        public string? Name {get; set;}
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

    }

    public class CardEmployee : Card {
        public Employee? Employee {get; set;}
    }

    public class Check : BaseBO {
        public DateTime? CheckDt {get; set;}
        public string? Type {get; set;}
        public Card? Card {get; set;}
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

        private static TimeSpan ConvertTime(string? timeExpression) {
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