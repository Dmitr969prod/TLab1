using System;
using System.Collections.Generic;

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

              
                if (c == '.' && pos + 1 < text.Length && text[pos + 1] == '.')
                {
                    tokens.Add(CreateToken(TokenType.Range, "..", line, startCol, col + 1, startPos));
                    pos += 2;
                    col += 2;
                    continue;
                }
                
                if (c == '.')
                {
                    tokens.Add(CreateToken(TokenType.Error, ".", line, startCol, col, startPos));
                    pos++;
                    col++;
                    continue;
                }

                
                if (IsSingleSeparator(c))
                {
                    tokens.Add(CreateToken(GetSeparatorType(c), c.ToString(), line, startCol, col, startPos));
                    pos++;
                    col++;
                    continue;
                }

                
                if (!char.IsWhiteSpace(c) && !IsSingleSeparator(c))
                {
                    string lexeme = "";
                    bool hasIllegalChars = false;
                    bool startsWithDigit = char.IsDigit(c);

                    while (pos < text.Length && !char.IsWhiteSpace(text[pos]) && !IsSingleSeparator(text[pos]))
                    {
                        char current = text[pos];

                        if (!char.IsLetterOrDigit(current))
                        {
                            
                            if (current == '.' && pos + 1 < text.Length && text[pos + 1] == '.')
                                break;

                     
                            if (lexeme.Length > 0)
                                break;

                            lexeme += current;
                            pos++;
                            col++;
                            hasIllegalChars = true;
                            break;
                        }

                        lexeme += current;
                        pos++;
                        col++;
                    }

                    TokenType type;

                    if (hasIllegalChars)
                    {
                        type = TokenType.Error;
                    }
                    else if (startsWithDigit)
                    {
                        type = IsOnlyDigits(lexeme) ? TokenType.IntLiteral : TokenType.Error;
                    }
                    else
                    {
                        
                        switch (lexeme)
                        {
                            case "for":
                                type = TokenType.For;
                                break;
                            case "in":
                                type = TokenType.In;
                                break;
                            case "println":
                                type = TokenType.Println;
                                break;
                            default:
                                type = TokenType.Identifier;
                                break;
                        }
                    }

                    tokens.Add(CreateToken(type, lexeme, line, startCol, col - 1, startPos));
                    continue;
                }

                
                tokens.Add(CreateToken(TokenType.Error, c.ToString(), line, startCol, col, startPos));
                pos++;
                col++;
            }

            return tokens;
        }

        private bool IsSingleSeparator(char c)
        {
            return c == '(' || c == ')' || c == '{' || c == '}' || c == ';';
        }

        private TokenType GetSeparatorType(char c)
        {
            switch (c)
            {
                case '(': return TokenType.LeftParen;
                case ')': return TokenType.RightParen;
                case '{': return TokenType.LeftBrace;
                case '}': return TokenType.RightBrace;
                case ';': return TokenType.Semicolon;
                default: return TokenType.Error;
            }
        }

        private bool IsOnlyDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }

        private Token CreateToken(TokenType type, string val, int line, int startCol, int endCol, int absIdx)
        {
            return new Token
            {
                Type = type,
                Value = val,
                Line = line,
                StartPos = startCol,
                EndPos = endCol,
                AbsoluteIndex = absIdx
            };
        }
    }
}