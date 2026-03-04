using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1
{
    internal class Scanner
    {
        public void Analyze(string Text)
        {
            

            List<string> numbers = FindNumbers(Text);

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
