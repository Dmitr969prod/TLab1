using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public sealed class ParseError
    {
        public string InvalidFragment { get; set; } = string.Empty;
        public int Line { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int AbsoluteIndex { get; set; }
        public string Message { get; set; } = string.Empty;

        public string LocationText
        {
            get
            {
                if (StartColumn == EndColumn)
                    return string.Format("строка {0}, позиция {1}", Line, StartColumn);

                return string.Format("строка {0}, позиции {1}-{2}", Line, StartColumn, EndColumn);
            }
        }
    }
}
