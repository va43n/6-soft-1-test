using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6_soft_1_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("123");
            Console.ReadLine();
        }
    }
    public static class Converter_p1_10
    {
        public static double ConvertValue(string number, int p1)
        {
            int weightPower;
            string numberWithoutDelimeter = number;
            int delimeterPosition = number.IndexOf('.');

            if (delimeterPosition != -1)
            {
                weightPower = delimeterPosition - 1;
                numberWithoutDelimeter = numberWithoutDelimeter.Remove(delimeterPosition, 1);
            }
            else
                weightPower = number.Length - 1;

            weightPower = Convert.ToInt32(Math.Pow(p1, weightPower));

            return P1ToTen(numberWithoutDelimeter, p1, weightPower);
        }

        private static double P1ToTen(string number, int p1, int weight)
        {
            double result = 0;

            for (int i = 0; i < number.Length; i++)
                result += weight * Editor.alphabet.IndexOf(number[i]) / Math.Pow(p1, i);

            return result;
        }
    }

    public static class Converter_10_p2
    {
        public static string ConvertValue(double number, int p2, int c)
        {
            if (number == Math.Floor(number))
                return TenToInt(Convert.ToInt32(number), p2);
            else
                return TenToInt(Convert.ToInt32(Math.Floor(number)), p2) + "." + TenToDouble(number - Math.Floor(number), p2, c);
        }

        private static string TenToInt(int number, int p2)
        {
            string s = "", temp;

            if (number == 0)
                return "0";

            while (number >= 1)
            {
                temp = Editor.alphabet[number % p2].ToString();

                s = s.Insert(0, temp);
                number /= p2;
            }

            return s;
        }

        private static string TenToDouble(double number, int p2, int c)
        {
            string s = "";

            for (int i = 0; i < c && number != 0.0; i++)
            {
                number *= p2;
                s += Editor.alphabet[Convert.ToInt32(Math.Floor(number))];
                number -= Math.Floor(number);
            }

            return s;
        }
    }

    public class Editor
    {
        public static string alphabet = "0123456789ABCDEF";
        public static int maxLength = 36;

        public string firstNumber = "0";
        private bool hasDelimeter = false;

        public string EditSomething(int tag)
        {
            if (tag < 16)
                firstNumber = firstNumber == "0" ? alphabet[tag].ToString() : firstNumber + alphabet[tag];

            else if (tag == 16 && !hasDelimeter)
            {
                hasDelimeter = true;
                firstNumber += ".";
            }
            else if (tag == 17)
            {
                if (firstNumber.Length == 1)
                {
                    firstNumber = "0";
                    return firstNumber;
                }
                else if (firstNumber[firstNumber.Length - 1] == '.')
                    hasDelimeter = false;
                firstNumber = firstNumber.Remove(firstNumber.Length - 1, 1);
            }
            else if (tag == 18)
            {
                hasDelimeter = false;
                firstNumber = "0";
                return "0";
            }

            return firstNumber;
        }
        public string DeleteUnnecessarySymbols()
        {
            for (int i = firstNumber.Length - 1; i >= 0; i--)
            {
                if (firstNumber[i] != '0' && firstNumber[i] != '.' || !hasDelimeter)
                    break;
                if (firstNumber[i] == '.')
                    hasDelimeter = false;
                firstNumber = firstNumber.Remove(i, 1);
            }

            return firstNumber;
        }

        public void CheckDelimeter()
        {
            if (firstNumber.IndexOf(".") == -1)
                hasDelimeter = false;
            else hasDelimeter = true;
        }

        public int GetAccuracy()
        {
            if (!hasDelimeter)
                return 0;

            return firstNumber.Length - firstNumber.IndexOf(".") - 1;
        }
    }

    public struct Record
    {
        public int p1, p2;
        public string firstNumber, secondNumber;

        public Record(int p01, int p02, string num1, string num2)
        {
            p1 = p01;
            p2 = p02;
            firstNumber = num1;
            secondNumber = num2;
        }
        public override string ToString()
        {
            string newFirst = firstNumber, newSecond = secondNumber;
            if (newFirst.Length > 15)
                newFirst = firstNumber.Substring(0, 12) + "...";
            if (newSecond.Length > 15)
                newSecond = secondNumber.Substring(0, 12) + "...";

            return newFirst + " (" + p1.ToString() + ") = " + newSecond + " (" + p2.ToString() + ")";
        }
    }

    public class History
    {
        public List<Record> records = new List<Record>();

        public void AddRecord(int p01, int p02, string num1, string num2)
        {
            Record record = new Record(p01, p02, num1, num2);
            records.Add(record);
        }

        public void Clear()
        {
            records.Clear();
        }
    }

    public class Controller
    {
        public enum State { Editing, Converted };
        public State St { get; set; }
        public int p1 { get; set; }
        public int p2 { get; set; }

        public Editor editor = new Editor();

        public History history = new History();

        public Controller(int p01, int p02)
        {
            p1 = p01;
            p2 = p02;
        }

        public string ButtonClicked(int tag)
        {
            if (tag == 19)
            {
                double number_10;
                string number_p2;

                int delimeterPosition = editor.firstNumber.IndexOf('.');
                double weightPower;

                if (delimeterPosition != -1)
                    weightPower = delimeterPosition - 1;
                else
                    weightPower = editor.firstNumber.Length - 1;
                if (Math.Pow(p1, weightPower) * Editor.alphabet.IndexOf(editor.firstNumber[0]) > Math.Pow(2, 31))
                    throw new Exception("Too big number");

                number_10 = Converter_p1_10.ConvertValue(editor.firstNumber, p1);
                number_p2 = Converter_10_p2.ConvertValue(number_10, p2, CalculateAccuracy());

                if (editor.firstNumber.Length > Editor.maxLength || number_p2.Length > Editor.maxLength)
                {
                    throw new Exception("Too big number");
                }

                St = State.Converted;
                history.AddRecord(p1, p2, editor.firstNumber, number_p2);

                return number_p2;
            }
            else
            {
                St = State.Editing;
                if (tag < 16 && editor.firstNumber.Length > Editor.maxLength)
                {
                    throw new Exception("Too big number");
                }
                return editor.EditSomething(tag);
            }
        }

        private int CalculateAccuracy()
        {
            return (int)Math.Round(editor.GetAccuracy() * Math.Log(p1) / Math.Log(p2) + 0.5);
        }
    }
}
