using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace lab_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private string encrypt(string text, int vertical, int horizontal)
        {
            string[,] table = new string[vertical, horizontal];

            int index = 0;
            // Составляем таблицу (матрицу)
            for (int i = 0; i < vertical; i++)
            {
                for (int j = 0; j < horizontal; j++)
                {
                    if (index < text.Length)
                    {
                        table[i, j] = text[index++].ToString();
                    }
                    else
                    {
                        table[i, j] = " ";
                    }
                }
            }
            string secretMessage = "";
            // Получаем слово из таблицы
            for (int j = 0; j < horizontal; j++)
            {
                for (int i = 0; i < vertical; i++)
                {
                    secretMessage += table[i, j];
                }
            }
            return secretMessage;
        }

        static string decrypt(string text, int vertical, int horizontal)
        {
            int length = text.Length;
            string[,] table = new string[vertical, horizontal];
            int index = 0;
            // Составляем таблицу (матрицу)
            for (int j = 0; j < horizontal; j++)
            {
                for (int i = 0; i < vertical; i++)
                {
                    if (index < length)
                    {
                        table[i, j] = text[index++].ToString();
                    }
                    else
                    {
                        table[i, j] = "";
                    }
                }
            }
            string decrypted = "";
            // Получаем слово из таблицы
            for (int i = 0; i < vertical; i++)
            {
                for (int j = 0; j < horizontal; j++)
                {
                    decrypted += table[i, j];
                }
            }

            return decrypted;
        }

        // Функция ищет в словаре русского языка слова из decryptedText
        private List<string> findRussianWordsInDecryptedText(string decryptedText)
        {
            List<string> russianWords = new List<string>();
            string pattern = "[^а-яА-Я ]";
            // Составляем текст, состоящий только из русских букв
            decryptedText = Regex.Replace(decryptedText, pattern, " ");
            // Лишние пробелы из начала и конца удаляем
            decryptedText.Trim();
            // Используем регулярное выражение для замены двойных и более пробелов на одиночные пробелы
            pattern = @"\s+";
            decryptedText = Regex.Replace(decryptedText, pattern, " ");
            List<string> russianSymbolsText = new List<string>(decryptedText.Split(" "));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Меняем кодировку, чтобы файл читался адекватно
            var reader = new System.IO.StreamReader("russian.txt", encoding: Encoding.GetEncoding(1251));
            while (reader.Peek() >= 0)
            {
                var newLine = reader.ReadLine().Trim();
                // Ищем в переданном тексте итерируемое слово
                var foundWords = russianSymbolsText.FindAll(e => e.ToLower() == newLine.ToLower());
                // Добавляем все найденные слова 
                russianWords.AddRange(foundWords);
            }
            return russianWords;
        }

        // Функция считает количество букв в массиве русских слов
        private int calculateRussianLetters(List<string> russianWordsFromVariant)
        {
            int russianSymbolsCount = 0;
            foreach (var word in russianWordsFromVariant)
                russianSymbolsCount += word.Length;
            return russianSymbolsCount;
        }


        private string breakTheCipher(string text)
        {
            text = text.Trim();
            string original = "";
            List<string> decrypted = new List<string>();
            // Считаем, сколько русских символов в данном тексте
            string pattern = "[^а-яА-Я]";
            var russianSymbolsText = Regex.Replace(text, pattern, "");
            int russianSymbolsCount = russianSymbolsText.Length;
            // Максимальное количество букв вариантов расшифровки из слов 
            int theMostMatchedLetters = 0;
            for (int i = 0; i<text.Length; i++)
            {
                for (int j = 0; j<text.Length; j++)
                {
                    if (text.Length <= i * j)
                    {
                        string newVariant = decrypt(text, i, j).Trim();
                        // Если такого варианта еще не было
                        if (!decrypted.Contains(newVariant))
                        {
                            List<string> russianWords = findRussianWordsInDecryptedText(newVariant);
                            int russianSymbolsCountInNewVariant = calculateRussianLetters(russianWords);
                            // Если количество букв совпадает идеально с количеством букв из оригинального текста
                            if (russianSymbolsCountInNewVariant == russianSymbolsCount)
                                return newVariant;
                            if (russianSymbolsCountInNewVariant > theMostMatchedLetters)
                            {
                                original = newVariant;
                                theMostMatchedLetters = russianSymbolsCountInNewVariant;
                            }
                            decrypted.Add(newVariant);
                        }
                    }
                }
            }
            return original;
        }

        // Шифрование текста
        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int vertical = Int32.Parse(textBox4.Text);
            int horizontal = Int32.Parse(textBox6.Text);
            if (vertical * horizontal < text.Length)
                MessageBox.Show("Текст не поместится в таблицу с введенными размерами!");
            else
                textBox8.Text = encrypt(text, vertical, horizontal);
        }


        // Расшифровка текста
        private void button2_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            int vertical = Int32.Parse(textBox5.Text);
            int horizontal = Int32.Parse(textBox7.Text);
            if (vertical * horizontal < text.Length)
                MessageBox.Show("Текст не поместится в таблицу с введенными размерами!");
            else 
                textBox9.Text = decrypt(text, vertical, horizontal);
        }


        // Взлом текста
        private void button3_Click(object sender, EventArgs e)
        {
            string encrypted = textBox3.Text;
            textBox10.Text = breakTheCipher(encrypted);
        }
    }
}
