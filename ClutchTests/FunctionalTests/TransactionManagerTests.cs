using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ServerLibrary.Storage.Transaction;

namespace ClutchTests.FunctionalTests
{
    [TestFixture]
    public class TransactionManagerTests
    {
        private TransactionManager _transactionManager;

        [SetUp]
        public void SetUp()
        {
            // Reset TransactionManager before each test
            _transactionManager = TransactionManager.GetInstance();
        }

        [Test]
        public void Lock_Should_Lock_And_Throw_If_Table_Is_Already_Locked()
        {
            // Arrange
            string tableName = "TestTable1";

            // Lock the table in one task
            _transactionManager.Lock(tableName);

            // Act & Assert: Try to lock the table again, expecting an exception
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _transactionManager.Lock(tableName)
            );
            Assert.AreEqual($"Table '{tableName}' is already locked.", exception.Message);
        }

        [Test]
        public async Task AwaitTableFreeAsync_Should_Await_If_Locked()
        {
            // Arrange
            string tableName = "TestTable2";

            // Lock the table in one task
            _transactionManager.Lock(tableName);

            // Act: Start a task that will await until the table is free
            var task = Task.Run(async () =>
            {
                await _transactionManager.AwaitTableFreeAsync(tableName);
                return "Table is free!";
            });

            // Act: Immediately release the lock after a delay
            await Task.Delay(200);  // Ensure the task is running and waiting
            _transactionManager.ReleaseLock(tableName);

            // Assert: Ensure the task completes successfully when the table is free
            var result = await task;
            Assert.AreEqual("Table is free!", result);
        }

        [Test]
        public void ReleaseLock_Should_Release_Lock_And_Allow_Another_Lock()
        {
            // Arrange
            string tableName = "TestTable3";

            // Lock the table
            _transactionManager.Lock(tableName);

            // Release the lock
            _transactionManager.ReleaseLock(tableName);

            // Act: Lock the table again after it was released
            Assert.DoesNotThrow(() => _transactionManager.Lock(tableName));
        }

        [Test]
        public void ReleaseLock_Should_Remove_Lock_From_Dictionary()
        {
            // Arrange
            string tableName = "TestTable4";

            // Lock the table
            _transactionManager.Lock(tableName);

            // Act: Release the lock
            _transactionManager.ReleaseLock(tableName);

            // Assert: Lock should be removed and no longer exist in the dictionary
            // NOTE: Direct access to LockDict is not possible, so this checks if it doesn't throw errors
            Assert.DoesNotThrow(() => _transactionManager.Lock(tableName));
        }

        [Test]
        public void Lock_Should_Be_Released_When_Transaction_Is_Completed()
        {
            // Arrange
            string tableName = "TestTable5";

            // Lock the table
            _transactionManager.Lock(tableName);

            // Act: Simulate a task processing the locked table
            var task = Task.Run(async () =>
            {
                await Task.Delay(100);  // Simulate work
                _transactionManager.ReleaseLock(tableName);
            });

            // Wait for the task to finish and release the lock
            task.Wait();

            // Assert: Ensure that the lock is released after the task is completed
            Assert.DoesNotThrow(() => _transactionManager.Lock(tableName));
        }

        [Test]
        public async Task AwaitTableFreeAsync_Should_Allow_Access_When_Free()
        {
            // Arrange
            string tableName = "TestTable6";

            // Ensure no lock exists and await free table
            await _transactionManager.AwaitTableFreeAsync(tableName);

            // Act: Lock the table
            _transactionManager.Lock(tableName);

            // Assert: The table should now be locked
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _transactionManager.Lock(tableName)
            );
            Assert.AreEqual($"Table '{tableName}' is already locked.", exception.Message);
        }
    }
}
