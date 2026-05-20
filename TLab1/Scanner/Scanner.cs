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


                if (c == '.')
                {
                    int dotStart = pos;
                    int dotStartCol = col;

                    while (pos < text.Length && text[pos] == '.')
                    {
                        pos++;
                        col++;
                    }

                    string dots = text.Substring(dotStart, pos - dotStart);

                    if (dots == "..")
                    {
                        tokens.Add(CreateToken(
                            TokenType.Range,
                            dots,
                            line,
                            dotStartCol,
                            col - 1,
                            dotStart
                        ));
                    }
                    else
                    {
                        tokens.Add(CreateToken(
                            TokenType.Error,
                            dots,
                            line,
                            dotStartCol,
                            col - 1,
                            dotStart
                        ));
                    }

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
                    bool startsWithDigit = char.IsDigit(c);

                    while (pos < text.Length &&
                           !char.IsWhiteSpace(text[pos]) &&
                           !IsSingleSeparator(text[pos]))
                    {
                        char current = text[pos];

                        if (current == '.')
                            break;

                        lexeme += current;
                        pos++;
                        col++;
                    }

                    TokenType type;

                    if (startsWithDigit)
                    {
                        type = IsOnlyDigits(lexeme) ? TokenType.IntLiteral : TokenType.Error;
                    }
                    else if (!IsOnlyLettersOrDigits(lexeme))
                    {
                        type = TokenType.Error;
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
        private bool IsOnlyLettersOrDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
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