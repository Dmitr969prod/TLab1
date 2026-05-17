using System.Collections.Generic;

namespace TLab1.AST
{
    public class BlockNode : AstNode
    {
        public List<AstNode> Statements { get; set; } = new List<AstNode>();

        public override string NodeName => "BlockNode";

        public override List<AstNode> GetChildren()
        {
            return Statements;
        }
    }
}