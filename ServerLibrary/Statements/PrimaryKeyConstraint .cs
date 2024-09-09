using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class PrimaryKeyConstraint : TableConstraint
    {
        public List<string> Columns { get; }

        public PrimaryKeyConstraint(List<string> columns)
        {
            Columns = columns;
        }

        public override string ToString()
        {
            return $"PRIMARY KEY ({string.Join(", ", Columns)})";
        }
    }
}
