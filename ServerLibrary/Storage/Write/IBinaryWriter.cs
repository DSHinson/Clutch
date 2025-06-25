using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Write
{
    public interface IBinaryWriter:IDisposable
    {
        // Asynchronously write raw bytes to the storage medium and return true if successful
        Task<bool> WriteAsync(byte[] data);

        /// <summary>
        /// Allows you to set the table you want to write, call on the first time you write and then every time you want to change the table
        /// </summary>
        /// <param name="tableName"></param>
        void SetWriteTable(string tableName);
    }
}
