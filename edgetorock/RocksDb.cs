
namespace edgetorock
{
    using System;
    using System.IO;
    using RocksDbSharp;

    internal class RocksHelper{
       
       internal static RocksDb _db = null; 
       
         internal static void setup(){
            
            string path = Environment.ExpandEnvironmentVariables(Path.Combine(Path.GetTempPath(), "edgerockdb"));
            var bbto = new BlockBasedTableOptions()
                .SetFilterPolicy(BloomFilterPolicy.Create(10, false))
                .SetWholeKeyFiltering(false)
                ;
            var options = new DbOptions()
                .SetCreateIfMissing(true)
                .SetCreateMissingColumnFamilies(true)
                ;
            var columnFamilies = new ColumnFamilies
            {
                { "default", new ColumnFamilyOptions().OptimizeForPointLookup(256) },
                { "test", new ColumnFamilyOptions()
                    //.SetWriteBufferSize(writeBufferSize)
                    //.SetMaxWriteBufferNumber(maxWriteBufferNumber)
                    //.SetMinWriteBufferNumberToMerge(minWriteBufferNumberToMerge)
                    .SetMemtableHugePageSize(2 * 1024 * 1024)
                     .SetPrefixExtractor(SliceTransform.CreateFixedPrefix((ulong)8))
                    .SetBlockBasedTableFactory(bbto)
                },
            };
            
            _db = RocksDb.Open(options, path, columnFamilies);
            
        }
    }
}
 