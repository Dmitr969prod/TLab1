using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public class Analyzer
    {
        public Result _result;
        private TokenStream _stream;
        public Result Parse(List<Token> tokens)
        {
            _stream = new TokenStream(tokens);
            _result = new Result();

            if(tokens == null || tokens.Count == 0)
            {
               AddErrorsFromEmptyInput("Входная последовательность токенов пуста.");
                return _result;
            }

            while (!_stream.IsAtEnd)
            {
                int startPosition = _stream.Position;

                ParseDeclaration();

                

                if(!_stream.IsAtEnd &&  _stream.Position == startPosition)
                {
                    _stream.Advance();
                }
            }
            return _result;
            
            
        }

        private void ParseDeclaration() 
        {
            if (!Expect(TokenType.For, "Ожидалось ключевое слово for"))
                SkipTo(TokenType.LeftParen, TokenType.Identifier, TokenType.In, TokenType.For);
            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
                SkipTo(TokenType.Identifier, TokenType.In);
            if(!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
                SkipTo(TokenType.In, TokenType.RightParen);
            if (!Expect(TokenType.In, "Ожидалось ключевое слово in"))
                SkipTo(TokenType.RightParen);

            ParseRangeExpression();

            if (!Expect(TokenType.RightParen, "Ожидалась правая закрывающая скобка"))
                    SkipTo(TokenType.LeftBrace);

            ParseBlock();

            if(!Expect(TokenType.Semicolon, "Ожидался символ ; в конце объявления"))
            {
                SkipTo(TokenType.For, TokenType.LeftParen, TokenType.Identifier, TokenType.In, TokenType.RightParen, TokenType.LeftBrace, TokenType.Semicolon);
                _stream.Match(TokenType.Semicolon);
            }
        }
        
        private void ParseRangeExpression()
        {
            if (!Expect(TokenType.IntLiteral, "Ожидалось число"))
                SkipTo(TokenType.Range, TokenType.RightParen);
            if (!Expect(TokenType.Range, "Ожидался оператор .."))
                SkipTo(TokenType.IntLiteral);
            if (!Expect(TokenType.IntLiteral, "Ожидалось число после .."))
                SkipTo(TokenType.LeftBrace, TokenType.Println);
        }
        private void ParseBlock()
        {
            if (!Expect(TokenType.LeftBrace, "Ожидался символ {"))
                SkipTo(TokenType.RightBrace, TokenType.Semicolon);
            while (!_stream.IsAtEnd && !_stream.Check(TokenType.RightBrace))
            {
                ParsePrintln();
                
            }
            if (!Expect(TokenType.RightBrace, "Ожидался символ }"))
                SkipTo(TokenType.Semicolon);
        }
        private void ParsePrintln()
        {
            _stream.Advance();
            /*if(!Expect(TokenType.Println, "Ожидалось ключевое слово println"))
                SkipTo(TokenType.StringLiteral, TokenType.Semicolon);*/
        }
        private bool Expect(TokenType code, string message)
        {
            if (_stream.Match(code))
                return true;

            AddError(message);
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

        private void AddErrorFromEmptyInput(string message)
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

        private void SkipTo(params TokenType[] syncTokens)
        {
            if (_stream.IsAtEnd)
                return;

            var syncSet = new HashSet<TokenType>(syncTokens);

            while (!_stream.IsAtEnd)
            {
                // Если текущий токен — один из синхронизирующих, остановиться
                if (syncSet.Contains(_stream.Current.Type))
                    return;

                _stream.Advance();
            }
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
