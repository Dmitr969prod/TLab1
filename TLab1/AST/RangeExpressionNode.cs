using System.Collections.Generic;

namespace TLab1.AST
{
    public class RangeExpressionNode : AstNode
    {
        public AstNode Start { get; set; }
        public AstNode End { get; set; }

        public override string NodeName => "RangeExpressionNode";

        public override List<AstNode> GetChildren()
        {
            return new List<AstNode>
            {
                Start,
                End
            };
        }
    }
}