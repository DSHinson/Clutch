using ServerLibrary.Statements;
using ServerLibrary.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Parser
{
    public class SqlParser
    {
        private readonly Tokenizer.Tokenizer _tokenizer;
        private Token _currentToken;

        public SqlParser(Tokenizer.Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _currentToken = _tokenizer.GetNextToken();
        }
        public SelectStatement ParseSelect()
        {
            // Expect and consume 'SELECT'
            Expect(TokenType.Keyword, "SELECT");

            var columns = ParseColumns();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "FROM");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var tableName = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            WhereClause whereClause = null;
            if (Match(TokenType.Keyword, "WHERE"))
            {
                whereClause = ParseWhereClause();
            }

            return new SelectStatement(_tokenizer.GetQuery(), columns, tableName, whereClause);
        }
        public InsertStatement ParseInsert()
        {
            // Step 1: Expect and consume "INSERT" keyword
            Expect(TokenType.Keyword, "INSERT");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "INTO");

            // Step 2: Get the table name
            string tableName = ParseIdentifier();

            // Step 3: Parse the list of columns
            var columns = ParseColumns();

            // Step 4: Expect and consume "VALUES" keyword
            Expect(TokenType.Keyword, "VALUES");

            // Step 5: Parse the list of values
            var values = ParseValueList();

            // Step 6: Ensure the number of columns matches the number of values
            if (columns.Count != values.Count)
            {
                throw new InvalidOperationException("The number of columns must match the number of values.");
            }

            // Step 7: Return an InsertStatement object
            return new InsertStatement(_tokenizer.GetQuery(),tableName, columns, values);
        }
        public DeleteStatement ParseDelete()
        {
            Expect(TokenType.Keyword, "DELETE");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "FROM");
            string tableName = ParseIdentifier();
            WhereClause whereClause = null;
            if (Match(TokenType.Keyword, "WHERE"))
            {
                whereClause = ParseWhereClause();
            }

            return new DeleteStatement(_tokenizer.GetQuery(), tableName, whereClause);
        }
        public UpdateStatement ParseUpdate() 
        {
            Expect(TokenType.Keyword, "UPDATE");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var tableName = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "SET");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var setClauses = ParseSetClauses();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            WhereClause whereClause = null;
            if (Match(TokenType.Keyword, "WHERE"))
            {
                whereClause = ParseWhereClause();
            }

            return new UpdateStatement(tableName, setClauses, whereClause);
        }
        public CreateTableStatement ParseCreateTable()
        {
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "CREATE");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Keyword, "TABLE");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var tableName = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.OpenParenthesis, "(");
            var columns = new List<ColumnDefinition>();
            var constraints = new List<TableConstraint>();
            ParseColumnsWithConstraints(ref columns,ref constraints);

            return new CreateTableStatement(_tokenizer.GetQuery(),tableName, columns, constraints);
        }
        private void ParseColumnsWithConstraints(ref List<ColumnDefinition> columns, ref List<TableConstraint> constraints) 
        {
            do
            {
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                if (_currentToken.Type == TokenType.Keyword && _currentToken.Value.ToUpper() == "PRIMARY")
                {
                    constraints.Add(ParsePrimaryKeyConstraint());
                }
                else if (_currentToken.Type == TokenType.Keyword && _currentToken.Value.ToUpper() == "CONSTRAINT")
                {
                    constraints.Add(ParseForeignKeyConstraint());
                }
                else
                {
                    columns.Add(ParseColumnDefinition());
                }

                if (_currentToken.Type == TokenType.Comma)
                {
                    _currentToken = _tokenizer.GetNextToken(); // Consume the comma
                }
                else
                {
                    break;
                }
            } while (true);
        }
        private ColumnDefinition ParseColumnDefinition()
        {
            List<string> constraints = new List<string>();


            // Parse column name
            string columnName = ParseIdentifier();

            // Parse data type
            string dataType = ParseDataType();

            // Parse data type constraint ie varchar(50)
            string dataTypeConstraint = ParseDataTypeConstraint();

            if (!string.IsNullOrEmpty(dataTypeConstraint))
            {
                constraints.Add(dataTypeConstraint);
            }

            //TODO: Handle default size contstraints for data types INT -> INT(11) etc.

            //

            if (_currentToken.Value == dataType || _currentToken.Type == TokenType.CloseParenthesis)
            {
                _currentToken = _tokenizer.GetNextToken();
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
            }

            // Parse any optional constraints (e.g., NOT NULL, UNIQUE, etc.)
           
            while (_currentToken.Type == TokenType.Keyword)
            {
                constraints.Add(_currentToken.Value.ToUpper());
                _currentToken = _tokenizer.GetNextToken();
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
            }

            return new ColumnDefinition(columnName, dataType, constraints);
        }
        private string ParseDataTypeConstraint()
        {
            string constraint = "";
            if (_currentToken.Type == TokenType.DataType)
            {
                //Still on the data type token, advance to the next token in the hopes of consuming an opening parenthesis
                _currentToken = _tokenizer.GetNextToken();
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
            }

            if (_currentToken.Type != TokenType.OpenParenthesis)
            {
                return constraint;
            }
            //Current token is '('
            while (_currentToken.Type != TokenType.CloseParenthesis)
            {
                //Consume the size value inside the parenthesis
                _currentToken = _tokenizer.GetNextToken();
                if (_currentToken.Type != TokenType.CloseParenthesis)
                {
                    constraint += _currentToken.Value; 
                }
            }
            return constraint;
        }
        private PrimaryKeyConstraint ParsePrimaryKeyConstraint()
        {
            // Expect "PRIMARY KEY" keywords
            Expect(TokenType.Keyword,"PRIMARY");
            Expect(TokenType.Keyword, "KEY");

            // Expect '('
            Expect(TokenType.OpenParenthesis,"(");

            // Parse column list
            var columns = ParseColumns();

            // Expect ')'
            Expect(TokenType.CloseParenthesis,")");

            return new PrimaryKeyConstraint(columns);
        }
        private ForeignKeyConstraint ParseForeignKeyConstraint()
        {
            Expect(TokenType.Keyword, "CONSTRAINT");

            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            //Get constraint name
            string contraintName = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Expect "CONSTRAINT" keywords
            Expect(TokenType.Keyword, "FOREIGN");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            
            Expect(TokenType.Keyword, "KEY");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Expect '('
            Expect(TokenType.OpenParenthesis,"(");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Parse the foreign key column
            string column = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Expect ')'
            Expect(TokenType.CloseParenthesis,")");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Expect "REFERENCES" keyword
            Expect(TokenType.Keyword, "REFERENCES");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            // Parse the referenced table and column
            string referencedTable = ParseIdentifier(); 
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.OpenParenthesis, "(");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            string referencedColumn = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.CloseParenthesis, ")");

            return new ForeignKeyConstraint(contraintName,column, referencedTable, referencedColumn);
        }
        private List<string> ParseColumns()
        {
            var columns = new List<string>();
            do
            {
                while(_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }

                // here we handle when the next token is already loded but it is a keyword
                if (_currentToken.Value != "FROM" && _currentToken.Value != "VALUES")
                {
                    columns.Add(ParseIdentifier());
                    //Expect columns to contain commas between them after moving past any white spaces
                    while (_currentToken.Type == TokenType.Whitespace)
                    {
                        _currentToken = _tokenizer.GetNextToken();
                    }
                    
                }
                // Check again if we hit the end of the column list
                if (_currentToken.Value != "FROM" && _currentToken.Value != "VALUES")
                {
                    //Move past the comma/ CloseParenthesis  if there is one
                    Expect(TokenType.Comma, TokenType.CloseParenthesis);
                }
            }
            while (_currentToken.Value !="FROM" && _currentToken.Value != "VALUES");

            return columns;
        }
        private List<string> ParseValueList()
        {
            var values = new List<string>();

            do
            {
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                if (_currentToken.Type == TokenType.OpenParenthesis)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                if (_currentToken.Type == TokenType.Literal)
                {
                    values.Add(_currentToken.Value);
                }
                else
                {
                    throw new InvalidOperationException($"Expected literal or numeric value, found '{_currentToken.Type}'.");
                }

                _currentToken = _tokenizer.GetNextToken();

                if (_currentToken.Type == TokenType.Comma)
                {
                    _currentToken = _tokenizer.GetNextToken(); // Consume the comma
                }
                else
                {
                    break;
                }
            } while (true);

            return values;
        }
        private WhereClause ParseWhereClause()
        {
            var column = ParseIdentifier();
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            Expect(TokenType.Operator, "=");
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var value = ParseLiteral();

            return new WhereClause(column,"=", value);
        }
        private string ParseIdentifier()
        {
            while (_currentToken.Type == TokenType.Whitespace || _currentToken.Type == TokenType.OpenParenthesis || _currentToken.Type == TokenType.CloseParenthesis)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var value = _currentToken.Value;
            Expect(TokenType.Identifier);
            //_currentToken = _tokenizer.GetNextToken();
            return value;
        }
        private string ParseDataType()
        {
            string value = "";
            do
            {
                
                while (_currentToken.Type == TokenType.Whitespace || _currentToken.Type == TokenType.OpenParenthesis || _currentToken.Type == TokenType.CloseParenthesis)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }

                value += _currentToken.Value;
                if (_currentToken.Type == TokenType.DataType)
                {
                    break;
                }
                Expect(TokenType.DataType);
                _currentToken = _tokenizer.GetNextToken();
            }
            while (_currentToken.Type != TokenType.Comma);
            return value;
        }
        private Dictionary<string, string> ParseSetClauses()
        {
            var setClauses = new Dictionary<string, string>();

            do
            {
                // Parse column name
                string column = ParseIdentifier();
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                // Expect '=' sign
                Expect(TokenType.Operator, "=");
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                // Parse the value (it can be a literal or numeric)
                string value = ParseLiteral();
                while (_currentToken.Type == TokenType.Whitespace)
                {
                    _currentToken = _tokenizer.GetNextToken();
                }
                // Add the column-value pair to the set clauses
                setClauses.Add(column, value);

                // Check if there is a comma (for multiple column-value pairs)
                if (_currentToken.Type == TokenType.Comma)
                {
                    _currentToken = _tokenizer.GetNextToken(); // Consume the comma
                }
                else
                {
                    break;
                }
            } while (true);

            return setClauses;
        }
        private string ParseLiteral()
        {
            while (_currentToken.Type == TokenType.Whitespace || _currentToken.Type == TokenType.OpenParenthesis || _currentToken.Type == TokenType.CloseParenthesis)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var value = _currentToken.Value;
            Expect(TokenType.Literal);
            return value;
        }
        private void Expect(TokenType type, string value = null)
        {
            if (_currentToken.Type != type || (value != null && _currentToken.Value != value))
            {
                throw new Exception($"Expected {type} '{value}', but found {_currentToken.Type} '{_currentToken.Value}'");
            }
            _currentToken = _tokenizer.GetNextToken();
        }
        private void Expect(params TokenType[] expectedTypes)
        {
            foreach (var type in expectedTypes)
            {
                if (_currentToken.Type == type)
                {
                    _currentToken = _tokenizer.GetNextToken();
                    return;
                }
            }

            throw new Exception(
                $"Expected {string.Join(" or ", expectedTypes)}, but found {_currentToken.Type} '{_currentToken.Value}'"
            );
        }

        private bool Match(TokenType type, string value = null)
        {
            if (_currentToken.Type == type && (value == null || _currentToken.Value == value))
            {
                _currentToken = _tokenizer.GetNextToken();
                return true;
            }
            return false;
        }
    }

}
