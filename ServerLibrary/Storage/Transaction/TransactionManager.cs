using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.Transaction
{
    /// <summary>
    /// Manages table locks and ensures thread safety for concurrent operations on tables.
    /// Provides a singleton instance to control access to shared resources in a transactional manner.
    /// </summary>
    public class TransactionManager
    {
        // Static global lock dictionary to hold a lock for each table
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> LockDict = new ConcurrentDictionary<string, SemaphoreSlim>();

        // Singleton instance
        private static readonly Lazy<TransactionManager> _instance = new(() => new TransactionManager());

        // Private constructor to prevent direct instantiation
        private TransactionManager() { }

        /// <summary>
        /// Gets the singleton instance of the TransactionManager.
        /// </summary>
        /// <returns>Instance of TransactionManager.</returns>
        public static TransactionManager GetInstance()
        {
            return _instance.Value;
        }

        /// <summary>
        /// Awaits until the specified table is free, meaning no lock exists in the lock dictionary for that table.
        /// </summary>
        /// <param name="tableName">The name of the table to check for lock availability.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AwaitTableFreeAsync(string tableName)
        {
            SemaphoreSlim semaphore = null;
            try
            {
                // Wait for the semaphore, this will block until the table is free
                if (LockDict.TryGetValue(tableName, out semaphore))
                {
                    await semaphore.WaitAsync();
                }
                else
                {
                    // No lock exists, so you can immediately continue (or take some action)
                    return;
                }
            }
            catch (Exception)
            {
                // TODO:Handle potential errors if necessary
                throw;
            }
        }

        /// <summary>
        /// Locks the specified table by adding a lock for it in the lock dictionary, or throws an exception
        /// if the table is already locked by another process.
        /// </summary>
        /// <param name="tableName">The name of the table to lock.</param>
        /// <exception cref="InvalidOperationException">Thrown if the table is already locked.</exception>
        public void Lock(string tableName)
        {
            // Try to get the existing semaphore
            var semaphore = LockDict.GetOrAdd(tableName, _ => new SemaphoreSlim(1, 1));

            // If the semaphore is already locked (i.e., a wait is in progress), throw an exception
            if (!semaphore.Wait(0))
            {
                throw new InvalidOperationException($"Table '{tableName}' is already locked.");
            }
        }

        /// <summary>
        /// Releases the lock for the specified table by removing it from the lock dictionary.
        /// </summary>
        /// <param name="tableName">The name of the table to release the lock for.</param>
        public void ReleaseLock(string tableName)
        {
            // Try to remove the lock from the dictionary (if it exists)
            if (LockDict.TryRemove(tableName, out var semaphore))
            {
                // Release the semaphore to allow other threads to access the table
                semaphore?.Release();

                // Optionally, dispose of the semaphore
                semaphore?.Dispose();
            }
        }
    }
}



