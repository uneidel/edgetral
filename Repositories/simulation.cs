namespace edgerest
{
    using System;
    internal class Simulation
    {
        internal static string CreateSampleTempJsonString(){
            var rand = new Random(); 
            string sample = 
            String.Format("[{{\"machine\":{{\"temperature\":{0},\"pressure\":{1}}},\"ambient\":{{\"temperature\":{2},\"humidity\":{3}}},\"timeCreated\":\"2018-01-28T12:00:58.395665Z\"}}]",
            rand.Next(20,30), rand.Next(990, 1100), rand.Next(30,40), rand.Next(50,100));

            return sample;
        }
    }

}