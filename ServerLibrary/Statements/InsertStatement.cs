using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class InsertStatement
    {
        public string Query { get; }
        public List<string> Columns { get; }
        public List<string> Values { get; }
        public string TableName { get; }

        public InsertStatement(string Query,string tableName, List<string> columns, List<string> values)
        {
            if (columns.Count != values.Count)
            {
                throw new ArgumentException("The number of columns must match the number of values.");
            }

            TableName = tableName;
            Columns = columns;
            Values = values;
        }

        public void Execute()
        {
            // Execution logic would go here, interacting with a database, etc.
        }

        public override string ToString()
        {
            var columns = string.Join(", ", Columns);
            var values = string.Join(", ", Values.Select(v => $"'{v}'"));
            return $"INSERT INTO {TableName} ({columns}) VALUES ({values});";
        }
    }
}
