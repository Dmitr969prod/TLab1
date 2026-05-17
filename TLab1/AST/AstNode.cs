using System.Collections.Generic;

namespace TLab1.AST
{
    public abstract class AstNode
    {
        public abstract string NodeName { get; }

        public virtual List<AstNode> GetChildren()
        {
            return new List<AstNode>();
        }
    }
}