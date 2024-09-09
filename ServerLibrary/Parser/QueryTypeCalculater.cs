using OneOf;
using ServerLibrary.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Parser
{
    public class QueryTypeCalculater
    {
        public OneOf<InsertStatement, SelectStatement, DeleteStatement, UpdateStatement, CreateTableStatement> DetermineQueryType(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("SQL query cannot be null or empty.", nameof(sql));
            }

            sql = sql.Trim().ToUpper();
            ServerLibrary.Tokenizer.Tokenizer tokenizer = new ServerLibrary.Tokenizer.Tokenizer(sql);
            SqlParser parser = new SqlParser(tokenizer);

            if (sql.StartsWith("INSERT"))
            {
                return parser.ParseInsert();
            }
            else if (sql.StartsWith("SELECT"))
            {
                return parser.ParseSelect();
            }
            else if (sql.StartsWith("UPDATE"))
            {
                return parser.ParseUpdate();
            }
            else if (sql.StartsWith("DELETE"))
            {
                return parser.ParseDelete();
            }
            else if (sql.StartsWith("CREATE TABLE"))
            {
                return parser.ParseCreateTable();
            }
            else
            {
                throw new NotSupportedException("Unsupported SQL query type.");
            }
        }
    }
}
