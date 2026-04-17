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
        public Result Parse(List<Token> tokens)
        {
            _result = new Result();
            if(tokens == null || tokens.Count == 0)
            {
               AddErrorsFromEmptyInput("Входная последовательность токенов пуста.");
                return _result;
            }
            return _result;
            
            
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
