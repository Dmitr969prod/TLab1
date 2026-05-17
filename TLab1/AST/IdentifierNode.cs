namespace TLab1.AST
{
    public class IdentifierNode : AstNode
    {
        public string Name { get; set; }

        public override string NodeName => $"IdentifierNode: {Name}";
    }
}