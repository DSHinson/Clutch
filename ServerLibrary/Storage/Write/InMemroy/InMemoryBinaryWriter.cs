using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Write.InMemroy
{
    public class InMemoryBinaryWriter : IBinaryWriter
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetWriteTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteAsync(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
