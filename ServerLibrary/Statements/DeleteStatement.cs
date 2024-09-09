using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class DeleteStatement
    {
        public string Query { get; }
        public string TableName { get; }
        public WhereClause WhereClause { get; }

        public DeleteStatement(string query,  string tableName, WhereClause whereClause)
        {
            Query = query;
            TableName = tableName;
            WhereClause = whereClause;
        }

        public void Execute()
        {
            // Execution logic would go here, interacting with a database, etc.
        }
    }
}
