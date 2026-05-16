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
        private bool IsUnexpectedEnd()
        {
            return _stream.IsAtEnd;
        }
        private void ParseDeclaration()
        {
            _suppressErrors = false;

            int beforeFor = _stream.Position;
            bool hasFor = Expect(TokenType.For, "Ожидалось ключевое слово for");

            if (!hasFor)
            {
                // Если Expect сам не съел ошибочный токен, съедаем его вручную
                if (_stream.Position == beforeFor && !_stream.IsAtEnd)
                    _stream.Advance();

                _suppressErrors = false;
            }

            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
            {
                // Не делаем SkipTo(LeftParen), иначе можно перескочить до println(
                _suppressErrors = false;
            }

            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
            {
                SkipTo(TokenType.In, TokenType.IntLiteral, TokenType.Range, TokenType.RightParen);
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

            _suppressErrors = false;

            if (!Expect(TokenType.Semicolon, "Ожидался символ ; в конце объявления"))
            {
                SkipTo(TokenType.For);
                _stream.Match(TokenType.Semicolon);
            }
        }

        private void ParseRangeExpression()
        {
            
            if (_stream.Check(TokenType.Range))
            {
                Token token = _stream.Current;

                _result.Errors.Add(new ParseError
                {
                    InvalidFragment = string.Empty,
                    Line = token.Line,
                    StartColumn = Math.Max(1, token.StartPos - 1),
                    EndColumn = Math.Max(1, token.StartPos - 1),
                    AbsoluteIndex = Math.Max(0, token.AbsoluteIndex - 1),
                    Message = "Ожидалось число"
                });
            }
            else if (_stream.Current.Type == TokenType.Error)
            {
                AddError("Ожидалось число (обнаружен недопустимый символ)");
                _stream.Advance();
            }
            else
            {
                Expect(TokenType.IntLiteral, "Ожидалось число");
            }

            
            Expect(TokenType.Range, "Ожидался оператор ..");

           
            if (_stream.Current.Type == TokenType.Error)
            {
                AddError("Ожидалось число после .. (обнаружен недопустимый символ)");
                _stream.Advance();
            }
            else
            {
                Expect(TokenType.IntLiteral, "Ожидалось число после ..");
            }
        }
        private void ParseBlock()
        {
            if (!Expect(TokenType.LeftBrace, "Ожидался символ {"))
            {
                _suppressErrors = false;

                
                if (_stream.Check(TokenType.LeftBrace))
                {
                    _stream.Advance();
                }
                else
                {
                    if (_stream.Check(TokenType.Semicolon) || _stream.Check(TokenType.RightBrace))
                        return;
                }
            }

            while (!_stream.IsAtEnd && !_stream.Check(TokenType.RightBrace))
            {
                int start = _stream.Position;

                ParsePrintln();

                if (_stream.Position == start)
                    _stream.Advance();
            }

            while (!_stream.IsAtEnd &&
       !_stream.Check(TokenType.RightBrace) &&
       !_stream.Check(TokenType.Semicolon))
            {
                int start = _stream.Position;

                ParsePrintln();

                if (_stream.Position == start)
                    _stream.Advance();
            }

            if (_stream.Check(TokenType.Semicolon))
            {
                _suppressErrors = false;
                return;
            }

            if (!Expect(TokenType.RightBrace, "Ожидался символ }"))
            {
                SkipTo(TokenType.Semicolon);
                _suppressErrors = false;
            }
        }
        private void ParsePrintln()
        {
            if (!_stream.Check(TokenType.Println))
            {
                AddError("Ожидалось ключевое слово println");

               
                if (_stream.Check(TokenType.Identifier) && _stream.CheckNext(TokenType.LeftParen))
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
            else
            {
                _stream.Advance();
                _suppressErrors = false;
            }

            if (!Expect(TokenType.LeftParen, "Ожидалась '('"))
            {
                SkipTo(TokenType.RightParen, TokenType.Semicolon, TokenType.RightBrace);

                if (_stream.Check(TokenType.RightParen))
                    _stream.Advance();

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




