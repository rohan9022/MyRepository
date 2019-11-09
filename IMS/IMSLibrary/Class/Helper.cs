using System;
using System.IO;

namespace IMSLibrary.Class
{
    public class Helper
    {
        protected Helper()
        {
        }

        public static void MoveFile(string FolderPath, string FileName, string valType)
        {
            if (!Directory.Exists(FolderPath + "\\" + valType))
            {
                Directory.CreateDirectory(FolderPath + "\\" + valType);
            }
            File.Move(FolderPath + "\\" + FileName, FolderPath + "\\" + valType + "\\" + FileName);
        }

        public static byte[] ConvertToByte(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            byte[] photo = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return photo;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string DecimalToWords(decimal d)
        {
            //Grab a string form of your decimal value ("12.34")
            var formatted = d.ToString();

            if (formatted.Contains("."))
            {
                //If it contains a decimal point, split it into both sides of the decimal
                string[] sides = formatted.Split('.');

                //Process each side and append them with "and", "dot" or "point" etc.
                if (Int32.Parse(sides[1]) > 0) return NumberToWords(Int32.Parse(sides[0])) + " and " + NumberToWords(Int32.Parse(sides[1])) + " Paisa Only.";
                return NumberToWords(Int32.Parse(sides[0])) + " Only.";
            }
            else
            {
                //Else process as normal
                return NumberToWords(Convert.ToInt32(d));
            }
        }
    }
}