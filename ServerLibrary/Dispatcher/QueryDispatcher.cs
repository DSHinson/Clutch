using ServerLibrary.Handlers;
using ServerLibrary.Parser;
using ServerLibrary.Statements;
using ServerLibrary.Storage.Transaction;
using ServerLibrary.Storage.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Dispatcher
{
    public class QueryDispatcher
    {
        private readonly TransactionManager _transactionManager;
        private readonly SqlParser _sqlParser;
        private readonly IBinaryWriter _binaryWriter;

        public QueryDispatcher(TransactionManager transactionManager, IBinaryWriter binaryWriter)
        {
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
            _binaryWriter = binaryWriter ?? throw new ArgumentNullException(nameof(binaryWriter));
        }

        public async Task ExecuteQueryAsync(string sql)
        {
            var queryType = QueryTypeCalculater.DetermineQueryType(sql);

            queryType.Switch(
              async (InsertStatement insert) => {

              },
              async (SelectStatement select) => {
              },
              async (DeleteStatement delete) => {
              },
              async (UpdateStatement update) => {
              },
              async (CreateTableStatement create) => {
                  //the create query handler is unique because the tabkle does not exists and there fore we dont have to worry about resource locking
                  CreateTableHandler createTableHandler = new CreateTableHandler(create, _binaryWriter);
                  await createTableHandler.ExecuteStatement();
              }
          );
        }
    }
}
