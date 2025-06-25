using ServerLibrary.Statements;
using ServerLibrary.Storage.Write;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Handlers
{

    
    internal class CreateTableHandler
    {
        private readonly CreateTableStatement _createTableStatement;
        private readonly IBinaryWriter _binaryWriter;

        public CreateTableHandler(CreateTableStatement createTableStatement, IBinaryWriter binaryWriter)
        {
            _createTableStatement = createTableStatement ?? throw new ArgumentNullException(nameof(createTableStatement));
            _binaryWriter = binaryWriter ?? throw new ArgumentNullException(nameof(binaryWriter));
        }

        public async Task ExecuteStatement()
        {
            await _binaryWriter.WriteAsync([], _createTableStatement.TableName);
            
        }
    }
}
