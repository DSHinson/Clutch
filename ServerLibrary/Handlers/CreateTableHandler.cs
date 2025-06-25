using ServerLibrary.Statements;
using ServerLibrary.Storage.Write;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Handlers
{
    /// <summary>
    /// Handles the execution of CREATE TABLE statements, including writing the table file
    /// and registering it in the system metadata table.
    /// </summary>
    public class CreateTableHandler
    {
        private readonly CreateTableStatement _createTableStatement;
        private readonly IBinaryWriter _binaryWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTableHandler"/> class.
        /// </summary>
        /// <param name="createTableStatement">The parsed CREATE TABLE statement.</param>
        /// <param name="binaryWriter">The binary writer to use for persisting the table.</param>
        public CreateTableHandler(CreateTableStatement createTableStatement, IBinaryWriter binaryWriter)
        {
            _createTableStatement = createTableStatement ?? throw new ArgumentNullException(nameof(createTableStatement));
            _binaryWriter = binaryWriter ?? throw new ArgumentNullException(nameof(binaryWriter));
        }

        /// <summary>
        /// Executes the CREATE TABLE statement by writing an empty table file and
        /// recording the table name in the system metadata table.
        /// </summary>
        /// <returns>True if both operations were successful; otherwise, false.</returns>
        public async Task<bool> ExecuteStatement()
        {
            // Convert the table name to a byte array using UTF-8 encoding
            byte[] tableNameBytes = Encoding.UTF8.GetBytes(_createTableStatement.TableName);

            _binaryWriter.SetWriteTable(_createTableStatement.TableName);

            // Create an empty file for the actual table
            bool tableCreated = await _binaryWriter.WriteAsync(Array.Empty<byte>());

            _binaryWriter.SetWriteTable("System_UserTables");

            // Update the system table with the new table name
            bool metadataUpdated = await _binaryWriter.WriteAsync(tableNameBytes);

            return tableCreated && metadataUpdated;
        }
    }
}
