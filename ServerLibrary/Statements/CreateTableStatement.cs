using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class CreateTableStatement
    {
        public string Query { get; }
        public string TableName { get; }
        public List<ColumnDefinition> Columns { get; }
        public List<TableConstraint> Constraints { get; }

        public CreateTableStatement(string query, string tableName, List<ColumnDefinition> columns, List<TableConstraint> constraints)
        {
            TableName = tableName;
            Columns = columns;
            Constraints = constraints;
            Query = query;
        }

        public override string ToString()
        {
            var columns = string.Join(", ", Columns.Select(c => c.ToString()));
            var constraints = Constraints.Count > 0 ? ", " + string.Join(", ", Constraints.Select(c => c.ToString())) : "";
            return $"CREATE TABLE {TableName} ({columns}{constraints});";
        }
    }
}
