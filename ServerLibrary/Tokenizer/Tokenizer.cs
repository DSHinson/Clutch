using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Tokenizer
{
    /// <summary>
    /// The Tokenizer class is responsible for parsing an SQL string into individual tokens
    /// such as keywords, identifiers, literals, operators, and punctuation. It provides 
    /// functionality to traverse the SQL string and categorize segments of the string 
    /// into meaningful token types for further processing.
    /// 
    /// Features:
    /// - Supports recognizing SQL keywords (e.g., SELECT, FROM, WHERE).
    /// - Distinguishes between identifiers, data types, literals, operators, and punctuation.
    /// - Handles quoted string literals and skips whitespace.
    /// - Throws an exception on encountering unexpected characters.
    /// 
    /// Usage:
    /// 1. Create an instance of the Tokenizer with the SQL string.
    /// 2. Call GetNextToken repeatedly to retrieve tokens sequentially until EOF.
    /// </summary>
    public class Tokenizer
    {
        private readonly string _sql; // The SQL string to tokenize
        private int _position; // Current position in the SQL string

        /// <summary>
        /// Initializes the tokenizer with the given SQL string.
        /// </summary>
        /// <param name="sql">The SQL string to tokenize.</param>
        public Tokenizer(string sql)
        {
            _sql = sql ?? throw new ArgumentNullException(nameof(sql));
            _position = 0;
        }

        /// <summary>
        /// Retrieves the original SQL string.
        /// </summary>
        /// <returns>The SQL string passed to the tokenizer.</returns>
        public string GetQuery() => _sql;

        /// <summary>
        /// Retrieves the next token from the SQL string.
        /// </summary>
        /// <returns>A Token object representing the next segment of the SQL string.</returns>
        public Token GetNextToken()
        {
            if (_position >= _sql.Length)
            {
                // End of input
                return new Token(TokenType.EndOfFile, string.Empty);
            }

            char current = _sql[_position];

            // Handle identifiers, keywords, and data types
            if (char.IsLetter(current))
            {
                string word = ReadWhile(c => char.IsLetterOrDigit(c) || c == '_');

                if (IsKeyword(word))
                {
                    return new Token(TokenType.Keyword, word.ToUpper());
                }
                if (IsDataType(word))
                {
                    return new Token(TokenType.DataType, word.ToUpper());
                }
                return new Token(TokenType.Identifier, word);
            }

            // Handle numeric literals
            if (char.IsNumber(current))
            {
                string word = ReadWhile(char.IsLetterOrDigit);
                return new Token(TokenType.Literal, word);
            }

            // Handle single-character operators and punctuation
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
            if (current == ';')
            {
                _position++;
                return new Token(TokenType.EndOfFile, ";");
            }

            // Handle whitespace
            if (char.IsWhiteSpace(current))
            {
                _position++;
                return new Token(TokenType.Whitespace, " ");
            }

            // Handle string literals
            if (current == '\'')
            {
                _position++; // Consume opening quote
                string literal = ReadWhile(c => c != '\'');
                _position++; // Consume closing quote
                return new Token(TokenType.Literal, literal);
            }

            // Unexpected character
            throw new Exception($"Unexpected character: {current}");
        }

        /// <summary>
        /// Moves the tokenizer's position past the first occurrence of a specified character.
        /// </summary>
        /// <param name="remove">The character to skip past.</param>
        public void movePastSpecialChar(char remove)
        {
            int index = _sql.IndexOf(remove); // Find the index of the first occurrence of the char
            if (index != -1) // If the char exists in the string
            {
                _position = index;
            }
        }

        /// <summary>
        /// Reads characters while a predicate is true.
        /// </summary>
        /// <param name="predicate">The condition to continue reading.</param>
        /// <returns>A string of characters that match the predicate.</returns>
        private string ReadWhile(Func<char, bool> predicate)
        {
            int start = _position;
            while (_position < _sql.Length && predicate(_sql[_position]))
            {
                _position++;
            }
            return _sql.Substring(start, _position - start);
        }

        /// <summary>
        /// Determines if a word is an SQL keyword.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <returns>True if the word is a keyword; otherwise, false.</returns>
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
                "UPDATE" => true,
                "DELETE" => true,
                "SET" => true,
                "CREATE" => true,
                "TABLE" => true,
                "PRIMARY" => true,
                "KEY" => true,
                "NOT" => true,
                "NULL" => true,
                "UNIQUE" => true,
                "CONSTRAINT" => true,
                "FOREIGN" => true,
                "REFERENCES" => true,
                _ => false,
            };
        }

        /// <summary>
        /// Determines if a word is a data type.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <returns>True if the word is a data type; otherwise, false.</returns>
        private bool IsDataType(string word)
        {
            return word.ToUpper() switch
            {
                "DATE" => true,
                "BOOLEAN" => true,
                "FLOAT" => true,
                "VARCHAR" => true,
                "INT" => true,
                "DECIMAL" => true,
                _ => false,
            };
        }
    }
}
