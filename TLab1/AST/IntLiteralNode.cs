namespace TLab1.AST
{
    public class IntLiteralNode : AstNode
    {
        public int Value { get; set; }

        public override string NodeName => $"IntLiteralNode: {Value}";
    }
}