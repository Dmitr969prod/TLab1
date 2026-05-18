using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TLab1
{
    public class RegexSearchClass
    {
        private RichTextBox editorTextBox;
        private DataGridView resultsGridView;
        private ComboBox searchTypeComboBox;
        private Label countLabel;
        private Label regexLabel;

        private List<SearchResult> currentResults = new List<SearchResult>();

        private class SearchResult
        {
            public string MatchText { get; set; }
            public int StartIndex { get; set; }
            public int Length { get; set; }
            public int LineNumber { get; set; }
            public int ColumnNumber { get; set; }
        }

        public enum SearchType
        {
            Quotes,
            Identifiers,
            CarNumbers
        }

        private readonly Dictionary<SearchType, string> searchDescriptions =
            new Dictionary<SearchType, string>
            {
                { SearchType.Quotes, "Цитаты в одинарных кавычках" },
                { SearchType.Identifiers, "Идентификаторы" },
                { SearchType.CarNumbers, "Российские автомобильные номера" }
            };

        private readonly Dictionary<SearchType, string> searchPatterns =
            new Dictionary<SearchType, string>
            {
                { SearchType.Quotes, @"'[^']*'" },

                
                { SearchType.Identifiers, @"\b[a-zA-Z$_][a-zA-Z0-9]*\b" },

                { SearchType.CarNumbers, @"[АВЕКМНОРСТУХ]\d{3}[АВЕКМНОРСТУХ]{2}\d{2,3}" }
            };

        public RegexSearchClass(
            RichTextBox editor,
            DataGridView resultsGrid,
            ComboBox searchCombo,
            Label countLabel,
            Label regexLabel)
        {
            this.editorTextBox = editor;
            this.resultsGridView = resultsGrid;
            this.searchTypeComboBox = searchCombo;
            this.countLabel = countLabel;
            this.regexLabel = regexLabel;

            InitializeResultsGridView();
            InitializeSearchComboBox();
            SubscribeEvents();
            UpdateRegexLabel();
            UpdateCountDisplay(0);
        }

        private void InitializeResultsGridView()
        {
            resultsGridView.Columns.Clear();

            resultsGridView.Columns.Add("MatchText", "Найденная подстрока");
            resultsGridView.Columns.Add("Position", "Начальная позиция");
            resultsGridView.Columns.Add("Length", "Длина");

            resultsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            resultsGridView.AllowUserToAddRows = false;
            resultsGridView.ReadOnly = true;
            resultsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitializeSearchComboBox()
        {
            searchTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            searchTypeComboBox.Items.Clear();

            foreach (var item in searchDescriptions)
            {
                searchTypeComboBox.Items.Add(item.Value);
            }

            searchTypeComboBox.SelectedIndex = 0;
            searchTypeComboBox.SelectedIndexChanged += SearchTypeComboBox_SelectedIndexChanged;
        }

        private void SubscribeEvents()
        {
            resultsGridView.SelectionChanged += OnResultSelected;
        }

        private void SearchTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRegexLabel();
        }

        private void UpdateRegexLabel()
        {
            SearchType selectedType = GetSelectedSearchType();

            switch (selectedType)
            {
                case SearchType.Quotes:
                    regexLabel.Text = "Регулярное выражение: /'[^']*'/";
                    break;

                case SearchType.Identifiers:
                    regexLabel.Text = @"Регулярное выражение: /\b[a-zA-Z$_][a-zA-Z0-9]*\b/";
                    break;

                case SearchType.CarNumbers:
                    regexLabel.Text = @"Регулярное выражение: /[АВЕКМНОРСТУХ]\d{3}[АВЕКМНОРСТУХ]{2}\d{2,3}/";
                    break;
            }
        }

        public void PerformRegexSearch()
        {
            ClearResults();

            if (string.IsNullOrWhiteSpace(editorTextBox.Text))
            {
                MessageBox.Show(
                    "Введите текст для поиска.",
                    "Информация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            SearchType selectedType = GetSelectedSearchType();
            string pattern = searchPatterns[selectedType];

            currentResults = FindMatchesByRegex(pattern, editorTextBox.Text);

            DisplayResults(currentResults);
            UpdateCountDisplay(currentResults.Count);
        }

        public void PerformAutomatonSearch()
        {
            ClearResults();

            if (string.IsNullOrWhiteSpace(editorTextBox.Text))
            {
                MessageBox.Show(
                    "Введите текст для поиска.",
                    "Информация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            currentResults = FindCarNumbersByAutomaton(editorTextBox.Text);

            DisplayResults(currentResults);
            UpdateCountDisplay(currentResults.Count);

            regexLabel.Text = "Доп. задание: поиск автомобильных номеров через граф автомата";
        }

        private List<SearchResult> FindMatchesByRegex(string pattern, string text)
        {
            List<SearchResult> results = new List<SearchResult>();

            Regex regex = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var position = GetLineAndColumn(text, match.Index);

                    results.Add(new SearchResult
                    {
                        MatchText = match.Value,
                        StartIndex = match.Index,
                        Length = match.Length,
                        LineNumber = position.LineNumber,
                        ColumnNumber = position.ColumnNumber
                    });
                }
            }

            return results;
        }

        private List<SearchResult> FindCarNumbersByAutomaton(string text)
        {
            List<SearchResult> results = new List<SearchResult>();

            for (int i = 0; i < text.Length; i++)
            {
                CheckCarCandidate(text, i, 8, results);
                CheckCarCandidate(text, i, 9, results);
            }

            return results;
        }

        private void CheckCarCandidate(
    string text,
    int startIndex,
    int length,
    List<SearchResult> results)
        {
            if (startIndex + length > text.Length)
                return;

            if (!HasCorrectBorders(text, startIndex, length))
                return;

            string candidate = text.Substring(startIndex, length);

            if (IsCarNumberByAutomaton(candidate))
            {
                var position = GetLineAndColumn(text, startIndex);

                results.Add(new SearchResult
                {
                    MatchText = candidate,
                    StartIndex = startIndex,
                    Length = length,
                    LineNumber = position.LineNumber,
                    ColumnNumber = position.ColumnNumber
                });
            }
        }
        private bool HasCorrectBorders(string text, int startIndex, int length)
        {
            int endIndex = startIndex + length;

            if (startIndex > 0 && char.IsLetterOrDigit(text[startIndex - 1]))
                return false;

            if (endIndex < text.Length && char.IsLetterOrDigit(text[endIndex]))
                return false;

            return true;
        }

        private bool IsCarNumberByAutomaton(string value)
        {
            int state = 0;

            foreach (char c in value)
            {
                switch (state)
                {
                    case 0:
                        if (IsCarLetter(c)) state = 1;
                        else return false;
                        break;

                    case 1:
                    case 2:
                    case 3:
                        if (IsDigit(c)) state++;
                        else return false;
                        break;

                    case 4:
                    case 5:
                        if (IsCarLetter(c)) state++;
                        else return false;
                        break;

                    case 6:
                    case 7:
                        if (IsDigit(c)) state++;
                        else return false;
                        break;

                    case 8:
                        if (IsDigit(c)) state = 9;
                        else return false;
                        break;

                    default:
                        return false;
                }
            }

            return state == 8 || state == 9;
        }

        private bool IsCarLetter(char c)
        {
            return "АВЕКМНОРСТУХ".Contains(c);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private (int LineNumber, int ColumnNumber) GetLineAndColumn(string text, int index)
        {
            int lineNumber = 1;
            int columnNumber = 1;

            for (int i = 0; i < index; i++)
            {
                if (text[i] == '\n')
                {
                    lineNumber++;
                    columnNumber = 1;
                }
                else if (text[i] != '\r')
                {
                    columnNumber++;
                }
            }

            return (lineNumber, columnNumber);
        }

        private void DisplayResults(List<SearchResult> results)
        {
            resultsGridView.Rows.Clear();

            foreach (SearchResult result in results)
            {
                resultsGridView.Rows.Add(
                    result.MatchText,
                    $"строка {result.LineNumber}, символ {result.ColumnNumber}",
                    result.Length
                );
            }
        }

        private void OnResultSelected(object sender, EventArgs e)
        {
            if (resultsGridView.SelectedRows.Count == 0)
                return;

            int selectedIndex = resultsGridView.SelectedRows[0].Index;

            if (selectedIndex < 0 || selectedIndex >= currentResults.Count)
                return;

            SearchResult selectedResult = currentResults[selectedIndex];

            HighlightTextInEditor(
                selectedResult.StartIndex,
                selectedResult.Length);
        }

        private void HighlightTextInEditor(int startIndex, int length)
        {
            editorTextBox.SelectAll();
            editorTextBox.SelectionBackColor = editorTextBox.BackColor;

            editorTextBox.Select(startIndex, length);
            editorTextBox.SelectionBackColor = Color.Yellow;
            editorTextBox.ScrollToCaret();
            editorTextBox.Focus();
        }

        public void ClearResults()
        {
            resultsGridView.Rows.Clear();
            currentResults.Clear();

            editorTextBox.SelectAll();
            editorTextBox.SelectionBackColor = editorTextBox.BackColor;
            editorTextBox.Select(0, 0);

            UpdateCountDisplay(0);
        }

        private void UpdateCountDisplay(int count)
        {
            countLabel.Text = $"Найдено совпадений: {count}";
            countLabel.ForeColor = count > 0 ? Color.Green : Color.Red;
        }

        private SearchType GetSelectedSearchType()
        {
            string selectedDescription = searchTypeComboBox.SelectedItem.ToString();

            return searchDescriptions
                .First(x => x.Value == selectedDescription)
                .Key;
        }
    }
}