using System.Collections.Generic;

namespace TLab1.AST
{
    public class ForLoopNode : AstNode
    {
        public IdentifierNode Variable { get; set; }

        public RangeExpressionNode Range { get; set; }

        public BlockNode Body { get; set; }

        public override string NodeName => "ForLoopNode";

        public override List<AstNode> GetChildren()
        {
            return new List<AstNode>
            {
                Variable,
                Range,
                Body
            };
        }
    }
}