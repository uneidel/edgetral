
    using StackExchange.Redis;
    using System;

    internal class RedisHelper{

        internal static RedisKey configKey = "edgetral";
        internal static int expirationDays = -1;
        private static  IDatabase db=null;
        private static readonly string connectionString = "127.0.0.1";

        private static IDatabase DataBase {
            get
            {
                if (db == null)
                {
                    var conn =ConnectionMultiplexer.Connect(connectionString);
                    db = conn.GetDatabase();
                    
                }
                return db;
            }
        }


        internal static string GetModuleConfig(){
            var score = DataBase.StringGet(configKey);
            return score;
        }
        internal static void InsertModuleConfig(string jsonselect ){
            
            try
            {
                DataBase.StringSet(configKey, jsonselect);
                Console.WriteLine("SensorConfig inserted to Redis.");
            }
            catch(Exception ex){
                Console.WriteLine("Error: " + ex.Message);    
            }
            finally{
                
            }

        }
        internal static void DeleteModuleKey()
        {
            DataBase.KeyDelete(configKey);
            
        }
        internal static dynamic GetSensorData(string sensorName, long starttick, long endtick, int count){
            var score = DataBase.SortedSetRangeByScore(sensorName, starttick+100000, endtick,Exclude.None, Order.Ascending, 0, count, CommandFlags.None );
            return score;
        }
        internal static void Insert(string key, string value){
            
            try{

                RedisKey rkey = key;
                var tick=DateTime.Now.Ticks;
                RedisValue val = value;
                DataBase.SortedSetAdd(key, val, tick, CommandFlags.None);
                //Console.WriteLine("Message inserted to Redis.");
            }
            catch(Exception ex){
                Console.WriteLine("Error: " + ex.Message);    
            }
            finally{
               
            }
        }
    }
