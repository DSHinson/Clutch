using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.SyntaxTree
{
    public class SelectStatement
    {
        public List<string> Columns { get; }
        public string TableName { get; }
        public WhereClause WhereClause { get; }

        public SelectStatement(List<string> columns, string tableName, WhereClause whereClause)
        {
            Columns = columns;
            TableName = tableName;
            WhereClause = whereClause;
        }

        // Method to execute or further manipulate the select statement
        public void Execute()
        {
            // Execution logic would go here, interacting with a database, etc.
        }

        public override string ToString()
        {
            var columns = string.Join(", ", Columns);
            var where = WhereClause != null ? $" WHERE {WhereClause}" : "";
            return $"SELECT {columns} FROM {TableName}{where}";
        }
    }
}
