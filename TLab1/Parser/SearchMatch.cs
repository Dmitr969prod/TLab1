using System.ComponentModel;

namespace TLab1.Parser
{
    public sealed class SearchMatch
    {
        [DisplayName("Найденная подстрока")]
        public string Fragment { get; set; } = string.Empty;

        [DisplayName("Строка")]
        public int Line { get; set; }

        [DisplayName("Символ")]
        public int Column { get; set; }

        [DisplayName("Длина")]
        public int Length { get; set; }

        [Browsable(false)]
        public int AbsoluteIndex { get; set; }

        public string Location
        {
            get
            {
                return $"строка {Line}, символ {Column}";
            }
        }
    }
}