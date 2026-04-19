using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public sealed class Result
    {
        public Result() 
        {
            Errors = new List<ParseError>();
        }
        public List<ParseError> Errors { get; private set; }
        public int ErrorCount
        {
            get { return Errors.Count; }
        }
    }
}
