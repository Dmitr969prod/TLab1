using System.Collections.Generic;
using TLab1.AST;
using TLab1.Parser;

namespace TLab1.Semantic
{
    public class SemanticAnalyzer
    {
        public List<ParseError> Analyze(AstNode root)
        {
            List<ParseError> errors = new List<ParseError>();

            if (root is ProgramNode program)
            {
                foreach (AstNode node in program.Statements)
                {
                    if (node is ForLoopNode forNode)
                        CheckForLoop(forNode, errors);
                }
            }

            return errors;
        }

        private void CheckForLoop(ForLoopNode node, List<ParseError> errors)
        {
            string loopVariable = node.Variable.Name;

            // Правило 3: допустимые значения
            if (node.Range.Start is IntLiteralNode startNode &&
                node.Range.End is IntLiteralNode endNode)
            {
                if (startNode.Value > endNode.Value)
                {
                    errors.Add(new ParseError
                    {
                        InvalidFragment = $"{startNode.Value}..{endNode.Value}",
                        Line = 1,
                        StartColumn = 1,
                        EndColumn = 1,
                        Message = "Семантическая ошибка: начало диапазона не может быть больше конца"
                    });
                }
            }

            // Правило 4: использование идентификаторов
            foreach (AstNode statement in node.Body.Statements)
            {
                if (statement is FunctionCallNode call)
                {
                    foreach (AstNode arg in call.Arguments)
                    {
                        if (arg is IdentifierNode identifier)
                        {
                            if (identifier.Name != loopVariable)
                            {
                                errors.Add(new ParseError
                                {
                                    InvalidFragment = identifier.Name,
                                    Line = 1,
                                    StartColumn = 1,
                                    EndColumn = 1,
                                    Message = $"Семантическая ошибка: идентификатор '{identifier.Name}' не был объявлен ранее"
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}