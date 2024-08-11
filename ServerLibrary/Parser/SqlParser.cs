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

            return new SelectStatement(columns, tableName, whereClause);
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

                columns.Add(ParseIdentifier());
            } while (_currentToken.Value !="FROM");

            return columns;
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
            while (_currentToken.Type == TokenType.Whitespace)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            var value = _currentToken.Value;
            Expect(TokenType.Identifier);
            _currentToken = _tokenizer.GetNextToken();
            return value;
        }

        private string ParseLiteral()
        {
            while (_currentToken.Type == TokenType.Whitespace)
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
