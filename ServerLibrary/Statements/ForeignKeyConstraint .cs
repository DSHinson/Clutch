using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class ForeignKeyConstraint : TableConstraint
    {
        public string Column { get; }
        public string ReferencedTable { get; }
        public string ReferencedColumn { get; }

        public ForeignKeyConstraint(string column, string referencedTable, string referencedColumn)
        {
            Column = column;
            ReferencedTable = referencedTable;
            ReferencedColumn = referencedColumn;
        }

        public override string ToString()
        {
            return $"FOREIGN KEY ({Column}) REFERENCES {ReferencedTable}({ReferencedColumn})";
        }
    }
}
