using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class WhereClause
    {
        public string Column { get; }
        public string Operator { get; }
        public string Value { get; }

        public WhereClause(string column, string @operator, string value)
        {
            Column = column;
            Operator = @operator;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Column} {Operator} '{Value}'";
        }
    }

}
