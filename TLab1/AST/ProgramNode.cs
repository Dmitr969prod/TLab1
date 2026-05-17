using System.Collections.Generic;

namespace TLab1.AST
{
    public class ProgramNode : AstNode
    {
        public List<AstNode> Statements { get; set; } = new List<AstNode>();

        public override string NodeName => "ProgramNode";

        public override List<AstNode> GetChildren()
        {
            return Statements;
        }
    }
}