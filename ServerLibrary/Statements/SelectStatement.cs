using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class SelectStatement
    {
        public string Query { get; }
        public List<string> Columns { get; }
        public string TableName { get; }
        public WhereClause WhereClause { get; }

        public SelectStatement(string query,List<string> columns, string tableName, WhereClause whereClause)
        {
            Query = query;
            Columns = columns;
            TableName = tableName;
            WhereClause = whereClause;
        }

        public void Execute()
        {
            // Execution logic would go here, interacting with a database, etc.
        }

    }
}
