using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TLab1.AST;

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

            ProgramNode program = new ProgramNode();

            if (tokens == null || tokens.Count == 0)
            {
                AddErrorsFromEmptyInput("Входная последовательность токенов пуста.");
                return _result;
            }

            while (!_stream.IsAtEnd)
            {
                int startPosition = _stream.Position;

                AstNode node = ParseDeclaration();

                if (node != null)
                    program.Statements.Add(node);

                if (!_stream.IsAtEnd && _stream.Position == startPosition)
                {
                    _stream.Advance();
                }
            }

            _result.AstRoot = program;

            return _result;
        }

        private AstNode ParseDeclaration()
        {
            _suppressErrors = false;

            if (!Expect(TokenType.For, "Ожидалось ключевое слово for"))
                return null;

            if (!Expect(TokenType.LeftParen, "Ожидалась левая открывающая скобка"))
                return null;

            Token variableToken = _stream.Current;

            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
                return null;

            if (!Expect(TokenType.In, "Ожидалось ключевое слово in"))
                return null;

            Token startToken = _stream.Current;

            if (!Expect(TokenType.IntLiteral, "Ожидалось число"))
                return null;

            if (!Expect(TokenType.Range, "Ожидался оператор .."))
                return null;

            Token endToken = _stream.Current;

            if (!Expect(TokenType.IntLiteral, "Ожидалось число после .."))
                return null;

            if (!Expect(TokenType.RightParen, "Ожидалась правая закрывающая скобка"))
                return null;

            if (!Expect(TokenType.LeftBrace, "Ожидался символ {"))
                return null;

            Token functionToken = _stream.Current;

            if (!Expect(TokenType.Println, "Ожидалось ключевое слово println"))
                return null;

            if (!Expect(TokenType.LeftParen, "Ожидалась '('"))
                return null;

            Token argumentToken = _stream.Current;

            if (!Expect(TokenType.Identifier, "Ожидалось имя переменной"))
                return null;

            if (!Expect(TokenType.RightParen, "Ожидалась ')'"))
                return null;

            if (!Expect(TokenType.RightBrace, "Ожидался символ }"))
                return null;

            if (_stream.Check(TokenType.Semicolon))
                _stream.Advance();

            IdentifierNode variable = new IdentifierNode
            {
                Name = variableToken.Value
            };

            RangeExpressionNode range = new RangeExpressionNode
            {
                Start = new IntLiteralNode
                {
                    Value = int.Parse(startToken.Value)
                },
                End = new IntLiteralNode
                {
                    Value = int.Parse(endToken.Value)
                }
            };

            FunctionCallNode printlnCall = new FunctionCallNode
            {
                FunctionName = functionToken.Value
            };

            printlnCall.Arguments.Add(new IdentifierNode
            {
                Name = argumentToken.Value
            });

            BlockNode block = new BlockNode();
            block.Statements.Add(printlnCall);

            return new ForLoopNode
            {
                Variable = variable,
                Range = range,
                Body = block
            };
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
                SkipTo(TokenType.RightParen);
                _suppressErrors = false;
            }
        }
        private void ParseBlock()
        {
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

                ParsePrintln();

                if (_stream.Position == start)
                    _stream.Advance();
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




