using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Write
{
    public interface IBinaryWriter
    {
        // Asynchronously write raw bytes to the storage medium and return true if successful
        Task<bool> WriteAsync(byte[] data, string tableName);
    }
}
