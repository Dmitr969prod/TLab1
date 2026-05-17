using System.Collections.Generic;
using TLab1.AST;

namespace TLab1.Parser
{
    public sealed class Result
    {
        public Result()
        {
            Errors = new List<ParseError>();
        }

        public List<ParseError> Errors { get; private set; }

        public AstNode AstRoot { get; set; }

        public int ErrorCount
        {
            get { return Errors.Count; }
        }
    }
}