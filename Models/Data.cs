namespace edgerest.Models
{
    using System;
    using System.Collections.Generic;
    public class DataClass{
            public string SensorName { get; set; }
            public string Filter{get;set;}
            public List<dynamic> SensorValues { get; set; }
        }
}