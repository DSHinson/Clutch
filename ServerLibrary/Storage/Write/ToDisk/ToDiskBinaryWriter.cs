using System.IO;

namespace ServerLibrary.Storage.Write.ToDisk
{
    /// <summary>
    /// Writes binary data to disk in a file with the specified table name.
    /// Uses memory-efficient asynchronous methods and ensures controlled file stream usage.
    /// </summary>
    public class ToDiskBinaryWriter : IBinaryWriter, IDisposable
    {
        private FileStream? _fileStream;

        /// <summary>
        /// Asynchronously writes binary data to the specified table file.
        /// Opens the file if not already opened, then appends the data.
        /// </summary>
        /// <param name="data">The binary data to write.</param>
        /// <returns>True if the write was successful, otherwise false.</returns>
        public async Task<bool> WriteAsync(byte[] data)
        {
            try
            {
                if (_fileStream is null)
                {
                    throw new InvalidOperationException("No write table has been set. Call SetWriteTable() before writing.");
                }

                // Use ReadOnlyMemory<byte> to avoid allocations and improve performance.
                // The memory-based overload of WriteAsync is optimized for memory buffers,
                // and avoids creating temporary arrays, which is why it's preferred over
                // the byte[] overload for efficiency.
                ReadOnlyMemory<byte> memoryBuffer = new ReadOnlyMemory<byte>(data);
                await _fileStream.WriteAsync(memoryBuffer);
                await _fileStream.FlushAsync();
                return true; // Return true if write was successful
            }
            catch (Exception)
            {
                // TODO: Handle error or logging as needed
                return false; // Return false if an error occurs
            }
        }

        /// <summary>
        /// Opens the file for appending binary data.
        /// If the file does not exist, it will be created.
        /// </summary>
        /// <param name="path">The path or name of the table file.</param>
        private void Open(string path)
        {
            // Open the file in append mode to prevent overwriting existing data.
            // This also ensures that multiple writes append to the end of the file.
            _fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        /// <summary>
        /// Allows you to set the table you want to write, call on the first time you write and then every time you want to change the table
        /// </summary>
        /// <param name="tableName"></param>
        public void SetWriteTable(string tableName)
        {
            _fileStream?.Flush();
            _fileStream?.Close();
            _fileStream?.Dispose();
            _fileStream = null;
            Open(tableName);
        }

        /// <summary>
        /// Flushes and disposes of the file stream if it was opened.
        /// This should be called when the writer is no longer needed to release file handles.
        /// </summary>
        public void Dispose()
        {
            _fileStream?.Flush();
            _fileStream?.Dispose();
            _fileStream = null;
        }
    }
}
