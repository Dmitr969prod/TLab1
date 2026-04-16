using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1
{
    public class Scanner
    {
        private static HashSet<string> keywords = new HashSet<string>()
        {
            "fun","val","var","if","else","when","while","for",
            "return","class","object","interface","package",
            "import","true","false","null"
        };

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

                
                if (c == '\n')
                {
                    line++;
                    col = 1;
                    pos++;
                    continue;
                }

                
                if (char.IsWhiteSpace(c))
                {
                    pos++;
                    col++;
                    continue;
                }

                
                if (char.IsLetter(c) || c == '_')
                {
                    string lexeme = "";

                    while (pos < text.Length &&
                           (char.IsLetterOrDigit(text[pos]) || text[pos] == '_'))
                    {
                        lexeme += text[pos];
                        pos++;
                        col++;
                    }

                    TokenType type = keywords.Contains(lexeme)
                        ? TokenType.Keyword
                        : TokenType.Identifier;

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
                    bool isFloat = false;

                    while (pos < text.Length && char.IsDigit(text[pos]))
                    {
                        lexeme += text[pos];
                        pos++;
                        col++;
                    }

                    if (pos < text.Length && text[pos] == '.')
                    {
                        isFloat = true;
                        lexeme += '.';
                        pos++;
                        col++;

                        while (pos < text.Length && char.IsDigit(text[pos]))
                        {
                            lexeme += text[pos];
                            pos++;
                            col++;
                        }
                    }

                    tokens.Add(new Token
                    {
                        Type = isFloat ? TokenType.FloatLiteral : TokenType.IntLiteral,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });

                    continue;
                }

               
                if (c == '"')
                {
                    string lexeme = "";
                    pos++; col++;

                    while (pos < text.Length && text[pos] != '"')
                    {
                        lexeme += text[pos];
                        pos++;
                        col++;
                    }

                    pos++; col++;

                    tokens.Add(new Token
                    {
                        Type = TokenType.StringLiteral,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });

                    continue;
                }

                
                if (c == '\'')
                {
                    string lexeme = "";
                    pos++; col++;

                    if (pos < text.Length)
                    {
                        lexeme += text[pos];
                        pos++; col++;
                    }

                    if (pos < text.Length && text[pos] == '\'')
                    {
                        pos++; col++;

                        tokens.Add(new Token
                        {
                            Type = TokenType.CharLiteral,
                            Value = lexeme,
                            Line = line,
                            StartPos = startCol,
                            EndPos = col - 1,
                            AbsoluteIndex = startPos
                        });
                        continue;
                    }
                }

                if (c == '/' && pos + 1 < text.Length && text[pos + 1] == '/')
                {
                    while (pos < text.Length && text[pos] != '\n')
                    {
                        pos++;
                        col++;
                    }
                    continue;
                }

               
                if (c == '/' && pos + 1 < text.Length && text[pos + 1] == '*')
                {
                    pos += 2;
                    col += 2;

                    while (pos + 1 < text.Length &&
                           !(text[pos] == '*' && text[pos + 1] == '/'))
                    {
                        if (text[pos] == '\n')
                        {
                            line++;
                            col = 1;
                        }
                        else col++;

                        pos++;
                    }

                    pos += 2;
                    col += 2;
                    continue;
                }

                
                string operators = "+-*/=<>!&|.,:;(){}[]";

                if (operators.Contains(c) && !operators.Contains(text[pos+1]))
                {
                    
                    tokens.Add(new Token
                    {
                        Type = TokenType.Operator,
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

                if (operators.Contains(c) && text[pos + 1] == '=')
                {
                    string lexeme = c.ToString() + text[pos + 1];
                    col ++;
                    tokens.Add(new Token
                    {
                        Type = TokenType.Operator,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });

                    pos += 2;
                    
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
