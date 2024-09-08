using ServerLibrary.SyntaxTree;
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

            var tableName = ParseIdentifier();

            WhereClause whereClause = null;
            if (Match(TokenType.Keyword, "WHERE"))
            {
                whereClause = ParseWhereClause();
            }

            return new SelectStatement(_tokenizer.getQuery(), columns, tableName, whereClause);
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
            return new InsertStatement(_tokenizer.getQuery(),tableName, columns, values);
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

            return new DeleteStatement(_tokenizer.getQuery(), tableName, whereClause);
        }

        public UpdateStatement ParseUpdate() 
        {
            Expect(TokenType.Keyword, "UPDATE");

            var tableName = ParseIdentifier();

            Expect(TokenType.Keyword, "SET");

            var setClauses = ParseSetClauses();

            WhereClause whereClause = null;
            if (Match(TokenType.Keyword, "WHERE"))
            {
                whereClause = ParseWhereClause();
            }

            return new UpdateStatement(tableName, setClauses, whereClause);
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
                }

                
            } while (_currentToken.Value !="FROM" && _currentToken.Value != "VALUES");

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
            Expect(TokenType.Operator, "=");
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
            _currentToken = _tokenizer.GetNextToken();
            return value;
        }
        private Dictionary<string, string> ParseSetClauses()
        {
            var setClauses = new Dictionary<string, string>();

            do
            {
                // Parse column name
                string column = ParseIdentifier();

                // Expect '=' sign
                Expect(TokenType.Operator, "=");

                // Parse the value (it can be a literal or numeric)
                string value = ParseLiteral();

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
