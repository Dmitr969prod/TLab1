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
        private bool IsUnexpectedEnd()
        {
            return _stream.IsAtEnd;
        }
        private void ParseDeclaration() 
        {
            _suppressErrors = false;
            if (!Expect(TokenType.For, "Ожидалось ключевое слово for"))
            {
                SkipTo(TokenType.For, TokenType.Semicolon);
                _stream.Match(TokenType.Semicolon);
                return;
            }
                
            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
                SkipTo(TokenType.Identifier, TokenType.In, TokenType.LeftParen);
            if(!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
                SkipTo(TokenType.In, TokenType.RightParen);
            if (!Expect(TokenType.In, "Ожидалось ключевое слово in"))
                SkipTo(TokenType.RightParen, TokenType.IntLiteral, TokenType.Range);

            ParseRangeExpression();

            if (!Expect(TokenType.RightParen, "Ожидалась правая закрывающая скобка"))
                    SkipTo(TokenType.LeftBrace);

            ParseBlock();

            if(!Expect(TokenType.Semicolon, "Ожидался символ ; в конце объявления"))
            {
                SkipTo(TokenType.For);
                _stream.Match(TokenType.Semicolon);
            }
        }
        
        private void ParseRangeExpression()
        {
            if (!Expect(TokenType.IntLiteral, "Ожидалось число"))
                SkipTo(TokenType.Range, TokenType.RightParen, TokenType.IntLiteral);
            if (!Expect(TokenType.Range, "Ожидался оператор .."))
                SkipTo(TokenType.IntLiteral, TokenType.RightParen);
            if (!Expect(TokenType.IntLiteral, "Ожидалось число после .."))
                SkipTo(TokenType.LeftBrace, TokenType.Println, TokenType.Semicolon, TokenType.RightParen);
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
            if (!Expect(TokenType.Println, "Ожидалось ключевое слово println"))
                SkipTo(TokenType.StringLiteral, TokenType.Semicolon);
            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
                SkipTo(TokenType.Identifier, TokenType.RightBrace, TokenType.Semicolon, TokenType.RightParen);
            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
                SkipTo(TokenType.Semicolon, TokenType.RightBrace, TokenType.RightParen);
            if (!Expect(TokenType.RightParen, "Ожидалась правая закрывающая скобка"))
                SkipTo(TokenType.Semicolon, TokenType.RightBrace, TokenType.RightParen);

        }
        private bool Expect(TokenType code, string message)
        {
            if (_stream.Match(code))
                return true;

            if (!_suppressErrors)
            {
                AddError(message);
                if(IsUnexpectedEnd())
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
            if (_stream.IsAtEnd)
                return;

            var syncSet = new HashSet<TokenType>(syncTokens);

            while (!_stream.IsAtEnd)
            {
                
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
