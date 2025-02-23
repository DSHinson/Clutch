using ServerLibrary.Parser;
using ServerLibrary.Statements;
using ServerLibrary.Storage.Transaction;
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

        public QueryDispatcher(TransactionManager transactionManager)
        {
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
        }

        public async Task ExecuteQueryAsync(string sql)
        {
            var queryType = QueryTypeCalculater.DetermineQueryType(sql);

            queryType.Switch(
              (InsertStatement insert) => {
              },
              (SelectStatement select) => {
              },
              (DeleteStatement delete) => {
              },
              (UpdateStatement update) => {
              },
              (CreateTableStatement create) => {
              }
          );
        }
    }
}
