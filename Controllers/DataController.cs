using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using edgerest.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using edgerest;

namespace edgerest.Controllers
{
    public class DataController : Controller
    {


        [HttpGet("data/{id}")]
        public string GetData(int id)
        {
            return "";
        }

        
        [HttpGet("data")]
        public List<DataClass> GetData()
        {
            
            Console.WriteLine("Insert SampleData into Redis");
            for (var i = 0; i< 100; i++){
               
               var json =Simulation.CreateSampleTempJsonString();
               RedisHelper.Insert("SensorA", json);
              // Console.WriteLine(json);

            }
            RedisHelper.InsertModuleConfig("{\"machine.temperature\": \"SensorA\", \"machine.pressure\": \"SensorA\"}");
            var config = RedisHelper.GetModuleConfig();
               
            return FilterData(config);
        }

        private static List<DataClass> FilterData(string config){
            var ctoken = JObject.Parse(config);
            Console.WriteLine($"Config: {ctoken}");
            //List<Tuple<string, List<dynamic>>> Data = new List<Tuple<string, List<dynamic>>>();
            List<DataClass> Data = new List<DataClass>();
            foreach (var j in ctoken.Properties()){
                string sensorname = j.Value.ToString();
                //Console.WriteLine($"SensorName: {sensorname}");
                string filter = j.Name.ToString();
                //Console.WriteLine($"Filter: {filter}");
                List <dynamic> datapoints = new List<dynamic>();
                var data = RedisHelper.GetSensorData(sensorname, DateTime.Now.Ticks, DateTime.Now.AddDays(-100).Ticks, 90);
                
                foreach (var d in data){
                    //Console.WriteLine($"Raw: {d}");
                    var json = JArray.Parse(d);
                    //Console.WriteLine($"Json: {json}");
                    string filteredval = (string)json[0].SelectToken(filter);
                    //Console.WriteLine("Filtered: " + filteredval);
                    datapoints.Add(filteredval);
                }
               
                Data.Add(new DataClass(){ SensorName= sensorname,Filter=filter, SensorValues = datapoints});
                Console.WriteLine($"Sensor: {sensorname} -  DataPoints: {datapoints.Count}");

                //
            }
            return Data;
        }
        
    }

    
}
