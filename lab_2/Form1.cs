using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string originalAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private string getNewAlphabet(int letterIndex)
        {
            string newAlphabet = "";
            //Заполняем после индекса и до конца алфавита
            for (int i = 0; letterIndex + i < originalAlphabet.Length; i++)
                newAlphabet += originalAlphabet[letterIndex + i];

            //Заполняем с начала до индекса
            for (int i = 0; i < letterIndex; i++)
                newAlphabet += originalAlphabet[i];
            return newAlphabet;
        }

        // Шифрование
        private string VizhinerSquareEncrypt(string text, string key)
        {
            string secretMessage = "";
            int keyIndex = 0;
            for (int i = 0; i<text.Length; i++)
            {
                if (originalAlphabet.Contains(char.ToLower(text[i])))
                {
                    // Позиция в оригинальном русском алфавите
                    int charIndexInOriginalAlphabet = originalAlphabet.IndexOf(char.ToLower(text[i]));
                    // Строим новый алфавит со сдвигом на эту позицию
                    string newAlphabet = getNewAlphabet(originalAlphabet.IndexOf(key[keyIndex]));
                    // Возвращаем исходный регистр
                    if (char.IsUpper(text[i]))
                        secretMessage += char.ToUpper(newAlphabet[charIndexInOriginalAlphabet]);
                    else
                        secretMessage += newAlphabet[charIndexInOriginalAlphabet];
                    keyIndex++;
                    // Если вышли за пределы ключа, то возвращаемся в начало
                    if (keyIndex >= key.Length)
                        keyIndex = 0;
                } else
                {
                    secretMessage += text[i];
                }
            }
            return secretMessage;
        }

        // Расшифровка
        private string VizhinerSquareDecrypt(string text, string key)
        {
            string originalMessage = "";
            int keyIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (originalAlphabet.Contains(char.ToLower(text[i])))
                {
                    // Строим новый алфавит по букве ключа
                    string newAlphabet = getNewAlphabet(originalAlphabet.IndexOf(key[keyIndex]));
                    // Ищем позицию буквы в новом алфавите
                    int charIndexInNewAlphabet = newAlphabet.IndexOf(char.ToLower(text[i]));
                    // Возвращаем регистр и записываем букву по позиции, но в оригинальном алфавите
                    if (char.IsUpper(text[i]))
                        originalMessage += char.ToUpper(originalAlphabet[charIndexInNewAlphabet]);
                    else
                        originalMessage += originalAlphabet[charIndexInNewAlphabet];
                    keyIndex++;
                    if (keyIndex >= key.Length)
                        keyIndex = 0;
                } else
                {
                    originalMessage += text[i];
                }
            }
            return originalMessage;
        }

        // Шифрование текста
        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string key = textBox4.Text;
            textBox8.Text = VizhinerSquareEncrypt(text, key);
        }

        // Расшифровка текста
        private void button2_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            string key = textBox3.Text;
            textBox9.Text = VizhinerSquareDecrypt(text, key);
        }
    }
}
