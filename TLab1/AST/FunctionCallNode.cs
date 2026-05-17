using System.Collections.Generic;

namespace TLab1.AST
{
    public class FunctionCallNode : AstNode
    {
        public string FunctionName { get; set; }

        public List<AstNode> Arguments { get; set; } = new List<AstNode>();

        public override string NodeName => $"FunctionCallNode: {FunctionName}";

        public override List<AstNode> GetChildren()
        {
            return Arguments;
        }
    }
}