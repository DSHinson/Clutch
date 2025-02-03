using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Write.ToDisk
{
    public class ToDiskBinaryWriter : IBinaryWriter , IDisposable
    {
        private FileStream? _fileStream;
        public async Task<bool> WriteAsync(byte[] data, string tableName)
        {
            if(_fileStream is null)
            {
                Open(tableName);
            }
            try
            {
                // Use ReadOnlyMemory<byte> to avoid allocations and improve performance
                // The memory-based overload of WriteAsync is optimized for memory buffers,
                // and avoids creating temporary arrays, which is why it's preferred over
                // the byte[] overload for efficiency.
                ReadOnlyMemory<byte> memoryBuffer = new ReadOnlyMemory<byte>(data);
                await _fileStream.WriteAsync(memoryBuffer);
                return true; // Return true if write was successful
            }
            catch (Exception)
            {
                // TODO:Handle error or logging
                return false; // Return false if an error occurs
            }
        }

        private void Open(string path)
        {
            // Open or create the file for writing
            _fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        }

        public void Dispose()
        {
            _fileStream?.Close();
            _fileStream?.Dispose();
        }
    }
}
