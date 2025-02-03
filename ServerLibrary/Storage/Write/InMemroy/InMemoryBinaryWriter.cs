using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Write.InMemroy
{
    public class InMemoryBinaryWriter : IBinaryWriter
    {
        public Task<bool> WriteAsync(byte[] data, string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
