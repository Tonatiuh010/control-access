using Engine.DAL;
using CsvHelper;
using System;
using System.Globalization;
using Engine.Constants;
using Engine.BO;
using Engine.BL;
using DataSet.Map;
using DataSet.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace DataSet {
    public class Program
    {
        public static ControlAccessBL bl { get; set; } = new ControlAccessBL((a , b) => { });

        public static void Main(string[] args)
        {
            SetConfiguration();

            if(true/*args.Contains("full")*/)
            {
                CsvProcess departament = new(C.DepartamentsCsv, csv => {
                    csv.Context.RegisterClassMap<DepartamentMap>();
                    var x =  csv.GetRecords<Departament>();
                    foreach(var d in x.ToList())
                    {
                        bl.SetDepartament(d, C.PROCESS_USER);
                    }
                });

                CsvProcess job = new(C.JobsCsv, csv => {
                    csv.Context.RegisterClassMap<JobMap>();
                    var x = csv.GetRecords<Job>();
                    foreach (var j in x.ToList())
                    {   
                        bl.SetJob(j, C.PROCESS_USER);
                    }
                });

                CsvProcess access = new(C.AccessCsv, csv => {
                    csv.Context.RegisterClassMap<AccessLevelMap>();
                    var x = csv.GetRecords<AccessLevel>();
                    
                    foreach (var l in x.ToList())
                    {
                        bl.SetAccessLevel(l, C.PROCESS_USER);
                    }
                });

                CsvProcess position = new(C.PositionsCsv, csv => {
                    csv.Context.RegisterClassMap<PositionMap>();
                    var x = csv.GetRecords<Position>();

                    foreach(var p in x.ToList())
                    {
                        bl.SetPosition(p, C.GLOBAL_USER);
                    }
                });

                CsvProcess employee = new(C.EmployeeCsv, csv => {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    var x = csv.GetRecords<ControlAccess>();

                    foreach (var e in x.ToList()) {
                        e.Image.Bytes = Utils.GetImage(e.Image.Url);
                        e.Card.Id = e.Card.IsValid() ? e.Card.Id : null;
                        bl.SetEmployee(e, C.PROCESS_USER);
                    }
                });


                CsvProcess card = new(C.CardCsv, csv => {
                    csv.Context.RegisterClassMap<CardEmployeeMap>();
                    var x = csv.GetRecords<CardEmployee>();

                    foreach (var c in x.ToList())
                    {
                        bl.SetCard(c, C.PROCESS_USER);
                    }
                });
            }


            // Just send intervals
        }

        public static void SetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            var b = builder.Build();

            ControlAccessDAL.ConnString = b.GetConnectionString(C.CTL_ACC);
            ControlAccessDAL.SetOnConnectionException((ex, msg) => Console.WriteLine($"Error Opening connection {msg} - {ex.Message}"));
        }

    }

    public class CsvProcess
    {
        public delegate void CsvAction(CsvReader csv);

        public string? FileName { get; set; }
        public CsvAction OnRead { get; set; }
        public CsvProcess(string fileName, CsvAction onRead)
        {
            using var stream = new StreamReader($"./Source/{fileName}");
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
            OnRead = onRead;
            OnRead(csv);
        }

    }
}