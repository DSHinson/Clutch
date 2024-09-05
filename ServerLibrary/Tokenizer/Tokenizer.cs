using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Tokenizer
{
    public class Tokenizer
    {
        private readonly string _sql;
        private int _position;

        public Tokenizer(string sql)
        {
            _sql = sql ?? throw new ArgumentNullException(nameof(sql));
            _position = 0;
        }

        public Token GetNextToken() 
        {
            if (_position >= _sql.Length)
            {
                return new Token(TokenType.EndOfFile, string.Empty);
            }
            char current = _sql[_position];

            if(char.IsLetter(current) )
            { 
                string word = ReadWhile(char.IsLetterOrDigit);

                if (IsKeyword(word))
                {
                    return new Token(TokenType.Keyword, word.ToUpper());
                }
                return new Token(TokenType.Identifier, word);
            }
            if (char.IsNumber(current))
            {
                string word = ReadWhile(char.IsLetterOrDigit);
                return new Token(TokenType.Literal, word);
            }

            if (current == '=')
            { 
                _position++;
                return new Token(TokenType.Operator, "=");
            }
            if (current == ',')
            {
                _position++;
                return new Token(TokenType.Comma, ",");
            }
            if (current == '(')
            {
                _position++;
                return new Token(TokenType.OpenParenthesis, "(");
            }
            if (current == ')')
            {
                _position++;
                return new Token(TokenType.CloseParenthesis, ")");
            }
            if (char.IsWhiteSpace(current))
            {
                _position++;
                return new Token(TokenType.Whitespace, " ");
            }
            if (current == '\'')
            {
                _position++;
                string literal = ReadWhile(c => c != '\'');
                _position++; // Consume closing quote
                return new Token(TokenType.Literal, literal);
            }

            throw new Exception($"Unexpected character: {current}");

        }

        public void movePastSpecialChar(char remove)
        {
            int index = _sql.IndexOf(remove); // Find the index of the first occurrence of the char
            if (index != -1)
            { // If the char exists in the string
                _position = index;
            }
        }

        private string ReadWhile(Func<char, bool> predicate)
        { 
            int start = _position;
            while (_position < _sql.Length && predicate(_sql[_position]))
            { 
                _position++;
            }
            return _sql.Substring(start,_position - start);
        }

        private bool IsKeyword(string word)
        {
            return word.ToUpper() switch
            {
                "SELECT" => true,
                "FROM" => true,
                "WHERE" => true,
                "INSERT" => true,
                "INTO" => true,
                "VALUES" => true,
                _ => false,
            };
        }
    }
}
