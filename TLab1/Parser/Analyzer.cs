using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public class Analyzer
    {
        public Result _result;
        private TokenStream _stream;
        private bool _suppressErrors;
        private bool _blockAborted;

        public Result Parse(List<Token> tokens)
        {
            _stream = new TokenStream(tokens);
            _result = new Result();



            if (tokens == null || tokens.Count == 0)
            {
                AddErrorsFromEmptyInput("Входная последовательность токенов пуста.");
                return _result;
            }

            while (!_stream.IsAtEnd)
            {
                
                int startPosition = _stream.Position;

                ParseDeclaration();



                if (!_stream.IsAtEnd && _stream.Position == startPosition)
                {
                    _stream.Advance();
                }
            }
            return _result;


        }

        private void ParseDeclaration()
        {
            _suppressErrors = false;

            if (_stream.Check(TokenType.Semicolon))
            {
                AddError("Одиночный символ ';' вне объявления");
                _stream.Advance();
                return;
            }

            bool hasFor = false;

            if ((_stream.Current.Type == TokenType.Identifier || _stream.Current.Type == TokenType.Error) &&
                _stream.Peek(1).Type == TokenType.Semicolon &&
                (_stream.Peek(2).Type == TokenType.Identifier || _stream.Peek(2).Type == TokenType.Error) &&
                _stream.Peek(3).Type == TokenType.LeftParen)
            {
                Token startToken = _stream.Current;
                Token endToken = _stream.Peek(2);

                AddErrorForRange(
                    "Ожидалось ключевое слово for",
                    startToken,
                    endToken,
                    startToken.Value + ";" + endToken.Value
                );

                _stream.Advance(); 
                _stream.Advance(); 
                _stream.Advance(); 

                hasFor = true; 
                _suppressErrors = false;
            }
            else
            {
                int beforeFor = _stream.Position;
                hasFor = Expect(TokenType.For, "Ожидалось ключевое слово for");

                if (!hasFor)
                {
                    if (_stream.Position == beforeFor && !_stream.IsAtEnd)
                        _stream.Advance();

                    _suppressErrors = false;
                }
            }

            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
            {
                SkipTo(TokenType.Identifier, TokenType.In, TokenType.IntLiteral);
                _suppressErrors = false;
            }

            

            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
            {
                if (_stream.Check(TokenType.In) && _stream.CheckNext(TokenType.In))
                {
                    _stream.Advance();
                }
                else
                {
                    SkipTo(TokenType.In, TokenType.IntLiteral, TokenType.Range, TokenType.RightParen);
                }

                _suppressErrors = false;
            }

            if (!Expect(TokenType.In, "Ожидалось ключевое слово in"))
            {
                SkipTo(TokenType.IntLiteral, TokenType.Range, TokenType.RightParen);
                _suppressErrors = false;
            }

            ParseRangeExpression();

            if (!Expect(TokenType.RightParen, "Ожидалась правая закрывающая скобка"))
                SkipTo(TokenType.LeftBrace, TokenType.Println);

            ParseBlock();
            if (_blockAborted)
                return;
            if (_stream.Check(TokenType.Semicolon))
            {
                _stream.Advance();
                return;
            }

            _suppressErrors = false;

            if (!Expect(TokenType.Semicolon, "Ожидался символ ; в конце объявления"))
            {
                SkipTo(TokenType.For);
                _stream.Match(TokenType.Semicolon);
            }
        }

        private void ParseRangeExpression()
        {
            
            if (_stream.Current.Type == TokenType.Error)
            {
                AddError("Ожидалось число");
                _stream.Advance();

                
                SkipTo(TokenType.Range, TokenType.RightParen);
                _suppressErrors = false;
            }
            else if (!Expect(TokenType.IntLiteral, "Ожидалось число"))
            {
                SkipTo(TokenType.Range, TokenType.RightParen);
                _suppressErrors = false;
            }

            
            if (!Expect(TokenType.Range, "Ожидался оператор .."))
            {
                SkipTo(TokenType.RightParen);
                _suppressErrors = false;
                return;
            }

            
            if (_stream.Current.Type == TokenType.Error)
            {
                AddError("Ожидалось число после ..");
                _stream.Advance();

                SkipTo(TokenType.RightParen);
                _suppressErrors = false;
                return;
            }

            if (!Expect(TokenType.IntLiteral, "Ожидалось число после .."))
            {
                SkipTo(
                    TokenType.RightParen,
                    TokenType.LeftBrace,
                    TokenType.Println,
                    TokenType.Identifier,
                    TokenType.Semicolon
                );

                _suppressErrors = false;
            }
        }
        private void ParseBlock()
        {
            bool hasPrintlnInBlock = false;
            bool hasAnyStatementInBlock = false;
            _blockAborted = false;

            if (!Expect(TokenType.LeftBrace, "Ожидался символ {"))
            {
                _suppressErrors = false;

                if (_stream.Check(TokenType.LeftBrace))
                {
                    _stream.Advance();
                }
                else if (_stream.Check(TokenType.Semicolon))
                {
                    _stream.Advance();
                    _suppressErrors = false;
                    _blockAborted = true;
                    return;
                }
                else if (_stream.Check(TokenType.Println))
                {
                }
                else if (_stream.Check(TokenType.Identifier) && _stream.CheckNext(TokenType.LeftParen))
                {
                }
                else
                {
                    SkipTo(TokenType.RightBrace, TokenType.Semicolon);
                    _suppressErrors = false;
                    return;
                }
            }

            

            while (!_stream.IsAtEnd &&
                   !_stream.Check(TokenType.RightBrace) &&
                   !_stream.Check(TokenType.Semicolon))
            {
                int start = _stream.Position;
                hasAnyStatementInBlock = true;

                if (_stream.Check(TokenType.Println) ||
                    ((_stream.Check(TokenType.Identifier) || _stream.Check(TokenType.Error)) &&
                     _stream.CheckNext(TokenType.LeftParen)))
                {
                    ParsePrintln();
                    hasPrintlnInBlock = true;
                }
                else if (hasPrintlnInBlock)
                {
                    AddError("Лишний символ после println(i)");
                    _stream.Advance();
                }
                else
                {
                    ParsePrintln();
                }

                if (_stream.Position == start)
                    _stream.Advance();
            }
            if (!hasPrintlnInBlock && !hasAnyStatementInBlock)
            {
                Token token = _stream.IsAtEnd ? _stream.Previous : _stream.Current;

                _result.Errors.Add(new ParseError
                {
                    InvalidFragment = string.Empty,
                    Line = token.Line,
                    StartColumn = token.StartPos,
                    EndColumn = token.EndPos,
                    AbsoluteIndex = token.AbsoluteIndex,
                    Message = "Ожидалось ключевое слово println"
                });

                _result.Errors.Add(new ParseError
                {
                    InvalidFragment = string.Empty,
                    Line = token.Line,
                    StartColumn = token.StartPos,
                    EndColumn = token.EndPos,
                    AbsoluteIndex = token.AbsoluteIndex,
                    Message = "Ожидалась '('"
                });

                _result.Errors.Add(new ParseError
                {
                    InvalidFragment = string.Empty,
                    Line = token.Line,
                    StartColumn = token.StartPos,
                    EndColumn = token.EndPos,
                    AbsoluteIndex = token.AbsoluteIndex,
                    Message = "Ожидалось имя переменной"
                });

                _result.Errors.Add(new ParseError
                {
                    InvalidFragment = string.Empty,
                    Line = token.Line,
                    StartColumn = token.StartPos,
                    EndColumn = token.EndPos,
                    AbsoluteIndex = token.AbsoluteIndex,
                    Message = "Ожидалась ')'"
                });
            }
            if (!Expect(TokenType.RightBrace, "Ожидался символ }"))
            {
                if (!_stream.Check(TokenType.Semicolon))
                    SkipTo(TokenType.Semicolon);

                _suppressErrors = false;
            }
        }
        private void ParsePrintln()
        {

            if (!_stream.Check(TokenType.Println))
            {
                if ((_stream.Current.Type == TokenType.Identifier || _stream.Current.Type == TokenType.Error) &&
                    _stream.Peek(1).Type == TokenType.Semicolon &&
                    (_stream.Peek(2).Type == TokenType.Identifier || _stream.Peek(2).Type == TokenType.Error) &&
                    _stream.Peek(3).Type == TokenType.LeftParen)
                {
                    Token startToken = _stream.Current;
                    Token endToken = _stream.Peek(2);

                    string fragment = startToken.Value + ";" + endToken.Value;

                    AddErrorForRange(
                        "Ожидалось ключевое слово println",
                        startToken,
                        endToken,
                        fragment
                    );

                    _stream.Advance();
                    _stream.Advance();
                    _stream.Advance();

                    _suppressErrors = false;
                }
                else
                {
                    AddError("Ожидалось ключевое слово println");

                    if ((_stream.Check(TokenType.Identifier) ||
                         _stream.Check(TokenType.Error)) &&
                        _stream.CheckNext(TokenType.LeftParen))
                    {
                        _stream.Advance();
                        _suppressErrors = false;
                    }
                    else
                    {
                        SkipTo(TokenType.Semicolon, TokenType.RightBrace);

                        if (_stream.Check(TokenType.Semicolon))
                            _stream.Advance();

                        return;
                    }
                }
            }
            else
            {
                _stream.Advance();
                _suppressErrors = false;
            }


            if (!Expect(TokenType.LeftParen, "Ожидалась '('"))
            {
                SkipTo(TokenType.Semicolon, TokenType.RightBrace);
                _suppressErrors = false;
                return;
            }

            
            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
            {
                SkipTo(TokenType.RightParen, TokenType.Semicolon, TokenType.RightBrace);
                _suppressErrors = false;

                if (_stream.Check(TokenType.RightParen))
                {
                    _stream.Advance();
                    return;
                }
            }

            
            if (!Expect(TokenType.RightParen, "Ожидалась ')'"))
            {
                SkipTo(TokenType.Semicolon, TokenType.RightBrace);
                _suppressErrors = false;
                return;
            }
        }

        private void AddErrorForRange(string message, Token startToken, Token endToken, string fragment)
        {
            _result.Errors.Add(new ParseError
            {
                InvalidFragment = fragment,
                Line = startToken.Line,
                StartColumn = startToken.StartPos,
                EndColumn = endToken.EndPos,
                AbsoluteIndex = startToken.AbsoluteIndex,
                Message = message
            });
        }
        private bool Expect(TokenType code, string message)
        {
            
            if (_stream.Current.Type == TokenType.Error)
            {
                AddError($"{message} (обнаружен недопустимый фрагмент: '{_stream.Current.Value}')");
                _stream.Advance();
                _suppressErrors = true;
                return false;
            }

           
            if (_stream.Match(code))
            {
                _suppressErrors = false; 
                return true;
            }

            
            if (!_suppressErrors)
            {
                AddError(message);
                _suppressErrors = true; 
            }
            return false;
        }
        private void AddError(string message)
        {
            Token token = _stream.IsAtEnd ? _stream.Previous : _stream.Current;

            _result.Errors.Add(new ParseError
            {
                InvalidFragment = token.Value ?? string.Empty,
                Line = token.Line,
                StartColumn = token.StartPos,
                EndColumn = token.EndPos,
                AbsoluteIndex = token.AbsoluteIndex,
                Message = message
            });
        }


        private void SkipTo(params TokenType[] syncTokens)
        {
            if (_stream.IsAtEnd) return;

            var syncSet = new HashSet<TokenType>(syncTokens);

            while (!_stream.IsAtEnd && !syncSet.Contains(_stream.Current.Type))
            {
                _stream.Advance();
            }

            _suppressErrors = false; 
        }

        private void AddErrorsFromEmptyInput(string message)
        {
            _result.Errors.Add(new ParseError
            {
                InvalidFragment = string.Empty,
                Line = 1,
                StartColumn = 1,
                EndColumn = 1,
                AbsoluteIndex = 0,
                Message = message
            });
        }
    }
}




