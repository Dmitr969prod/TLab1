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

                _stream.Match(TokenType.Separator);

                if(!_stream.IsAtEnd &&  _stream.Position == startPosition)
                {
                    _stream.Advance();
                }
            }
            return _result;
            
            
        }

        private void ParseDeclaration() { }
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
