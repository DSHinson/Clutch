using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Tokenizer
{
    public enum TokenType
    {
        Keyword,
        Identifier,
        Operator,
        Literal,
        Comma,
        Whitespace,
        EndOfFile,
    }
}
