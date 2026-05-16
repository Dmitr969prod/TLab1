using System.ComponentModel;

public enum TokenType
{
    // ИДЕНТИФИКАТОРЫ
    Identifier = 10,

    // КЛЮЧЕВЫЕ СЛОВА
    Keyword = 11,
    For = 12,
    In = 13,
    Println = 14,

    // КОНСТАНТЫ
    IntLiteral = 20,
    FloatLiteral = 21,
    StringLiteral = 22,
    CharLiteral = 23,

    // ОПЕРАТОРЫ
    Operator = 40,
    Assign = 41, // =
    Range = 42,  // ..

    // РАЗДЕЛИТЕЛИ
    Separator = 50,
    LeftParen = 51,   // (
    RightParen = 52,  // )
    LeftBrace = 53,   // {
    RightBrace = 54,  // }
    Semicolon = 55,    // ;

    // ОШИБКИ
    Error = 99
}

public class Token
{
    public int Code => (int)Type;

    public string Value { get; set; }

    [Browsable(false)]
    public int Line { get; set; }

    [Browsable(false)]
    public int StartPos { get; set; }

    [Browsable(false)]
    public int EndPos { get; set; }

    [Browsable(false)]
    public int AbsoluteIndex { get; set; }

    [Browsable(false)]
    public TokenType Type { get; set; }

    public string TypeName
    {
        get
        {
            switch (Type)
            {
                case TokenType.Identifier:
                    return "Идентификатор";
                case TokenType.Keyword:
                    return "Ключевое слово";
                case TokenType.IntLiteral:
                    return "Целочисленная константа";
                case TokenType.FloatLiteral:
                    return "Вещественная константа";
                case TokenType.StringLiteral:
                    return "Строковая константа";
                case TokenType.CharLiteral:
                    return "Символьная константа";
                case TokenType.Operator:
                    return "Оператор";
                case TokenType.Separator:
                    return "Разделитель";
                case TokenType.Error:
                    return "Лексическая ошибка";
                default:
                    return "Неизвестная лексема";
            }
        }
    }

    public string Location => $"Стр: {Line}, Поз: {StartPos}-{EndPos}";
}