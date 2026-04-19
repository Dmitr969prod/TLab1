using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1
{
    public class Scanner
    {


        public List<Token> Analyze(string text)
        {
            var tokens = new List<Token>();

            int pos = 0;
            int line = 1;
            int col = 1;

            while (pos < text.Length)
            {
                char c = text[pos];
                int startCol = col;
                int startPos = pos;

                
                if (char.IsWhiteSpace(c))
                {
                    if (c == '\n')
                    {
                        line++;
                        col = 1;
                    }
                    else
                    {
                        col++;
                    }

                    pos++;
                    continue;
                }

                
                if (char.IsLetter(c))
                {
                    string lexeme = "";

                    while (pos < text.Length && char.IsLetterOrDigit(text[pos]))
                    {
                        lexeme += text[pos];
                        pos++;
                        col++;
                    }

                    TokenType type = TokenType.Identifier;

                    if (lexeme == "for")
                        type = TokenType.For;
                    else if (lexeme == "in")
                        type = TokenType.In;
                    else if (lexeme == "println")
                        type = TokenType.Println;

                    tokens.Add(new Token
                    {
                        Type = type,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });

                    continue;
                }

               
                if (char.IsDigit(c))
                {
                    string lexeme = "";

                    while (pos < text.Length && char.IsDigit(text[pos]))
                    {
                        lexeme += text[pos];
                        pos++;
                        col++;
                    }

                    tokens.Add(new Token
                    {
                        Type = TokenType.IntLiteral,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });

                    continue;
                }

                
                if (c == '.' && pos + 1 < text.Length && text[pos + 1] == '.')
                {
                    tokens.Add(new Token
                    {
                        Type = TokenType.Range,
                        Value = "..",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col + 1,
                        AbsoluteIndex = startPos
                    });

                    pos += 2;
                    col += 2;
                    continue;
                }

                
                TokenType? singleType = null;

                switch (c)
                {
                    case '(':
                        singleType = TokenType.LeftParen;
                        break;
                    case ')':
                        singleType = TokenType.RightParen;
                        break;
                    case '{':
                        singleType = TokenType.LeftBrace;
                        break;
                    case '}':
                        singleType = TokenType.RightBrace;
                        break;
                    case ';':
                        singleType = TokenType.Semicolon;
                        break;
                }

                if (singleType != null)
                {
                    tokens.Add(new Token
                    {
                        Type = singleType.Value,
                        Value = c.ToString(),
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });

                    pos++;
                    col++;
                    continue;
                }

                
                tokens.Add(new Token
                {
                    Type = TokenType.Error,
                    Value = c.ToString(),
                    Line = line,
                    StartPos = startCol,
                    EndPos = col,
                    AbsoluteIndex = startPos
                });

                pos++;
                col++;
            }

            return tokens;
        }
       
    
    }
}
