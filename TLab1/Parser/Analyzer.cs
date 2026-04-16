using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public class Analyzer
    {
        Result _result;
        public Analyzer Parse(List<Token> tokens)
        {
            _result = new Result();
            if(tokens == null || tokens.Count == 0)
            {
               
            }
            // Реализуйте синтаксический анализ здесь
            return this;
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
