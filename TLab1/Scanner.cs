using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1
{
    internal class Scanner
    {
        public List<string> Analyze(string Text)
        {
            List<string> result = new List<string>();
            

            List<string> numbers = FindNumbers(Text);

            result.Add("14|ключевое слово|int|Строка1, 1-3");
            result.Add("17|идентификатор|i|Строка1, 5-5");
            return result;

/*            Console.WriteLine($"Анализируемый текст: {Text}");
            Console.WriteLine("Найденные числа:");
            foreach (string number in numbers)
            {
                Console.WriteLine(number);
            }*/
            

        }

        public List<string> FindNumbers(string Text)
        {
            List<string> numbers = new List<string>();
            string currentNumber = "";
            
            foreach (char c in Text)
            {
                if (char.IsDigit(c))
                {
                    currentNumber += c;
                }
                else
                {
                    if (currentNumber != "")
                    {
                        numbers.Add(currentNumber);
                        currentNumber = "";
                    }
                }
            }
            return numbers;
        }
    }
}
