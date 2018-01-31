/*
namespace edgetorock
{
    using System;
    using System.IO;
    using RocksDbSharp;

    internal class RocksHelper{
       
       internal static RocksDb _db = null; 
       
         internal static void setup(){
            
            string path = "/tmp/edgetorock";

            Console.WriteLine("Debug1 ");
            var bbto = new BlockBasedTableOptions()
                .SetFilterPolicy(BloomFilterPolicy.Create(10, false))
                .SetWholeKeyFiltering(false)
                ;
            var options = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true)
                
                ;
                Console.WriteLine("Debug1 ");
            var columnFamilies = new ColumnFamilies
            {
                { "default", new ColumnFamilyOptions().OptimizeForPointLookup(256) },
                { "test", new ColumnFamilyOptions()
                    .SetMemtableHugePageSize(2 * 1024 * 1024)
                    .SetPrefixExtractor(SliceTransform.CreateFixedPrefix((ulong)8))
                    .SetBlockBasedTableFactory(bbto)
                },
            };
            Console.WriteLine("Debug3 ");
            try
            {
                _db = RocksDb.Open(options, path, columnFamilies);
            }
            catch(Exception ex)
            { 
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
    }
}
 

  */