using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.SyntaxTree
{
    public class UpdateStatement
    {
        public Dictionary<string, string> SetClauses { get; }
        public string TableName { get; }
        public string Query { get; }
        public WhereClause WhereClause { get; }

        public UpdateStatement(string tableName, Dictionary<string, string> setClauses, WhereClause whereClause)
        {
            TableName = tableName;
            SetClauses = setClauses;
            WhereClause = whereClause;
        }
    }
}
