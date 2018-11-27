using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TofPLandCP
{
    public partial class Form1 : Form
    {
        //директория для сохранения результатов (по defult выбрана папка с .exe файлом приложения)
        private string folderPath = Environment.CurrentDirectory;
        //коллекция список со строками исходного кода
        private List<string> codeList=new List<string>(); 
        //коллекция список с результатом работы
        private List<string> workResult= new List<string>();
      
        public Form1()
        {
            InitializeComponent();  
        }
        /// <summary>
        /// Выбор папки для сохранения результатов работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = folderPath;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = folderBrowserDialog.SelectedPath;
                saveFileCode.Enabled = true;
                openFileCode.Enabled = true;
                lab1Button.Enabled = true;
            }
        }
        /// <summary>
        /// Выбор файла с исходным кодом программы на языке Fortran
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileCode_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = folderPath;
            openFileDialog.Filter = "Fortran file (*.for)|*.for| Все файлы (*.*)|*.*";
            openFileDialog.FileName = "Код программы" + ".for";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                if (FileLab.OpenFile(openFileDialog.FileName, ref codeList))
                {
                    //отключение редактирования формы
                    MessageBox.Show("Файл успешно открыт", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateForm();
                }
                else
                    MessageBox.Show("Что-то пошло не так, файл не открылся.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Выбор места сохранения файла с исходным кодом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileCode_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textCode.Text))
            {
                //подготовка списка к сохранению в файл
                ListUpdate();
                saveFileDialog.InitialDirectory = folderPath;
                saveFileDialog.Filter = "Fortran file (*.for)|*.for| Все файлы (*.*)|*.*";
                saveFileDialog.FileName = "Code.for";
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                    if (FileLab.SaveFile(saveFileDialog.FileName, ref codeList))
                        MessageBox.Show("Файл успешно сохранен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Что-то пошло не так, файл не сохранился.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        /// <summary>
        /// Обновление формы
        /// </summary>
        private void UpdateForm()
        {
            textCode.Text = "";
            foreach (string str in codeList)
                textCode.Text += str+Environment.NewLine;
            textResult.Text = "";
            foreach (string str in workResult)
                textResult.Text += str + Environment.NewLine;
        }
        /// <summary>
        /// Заполнение списка содержимым textBox
        /// </summary>
        private void ListUpdate()
        {
            if (!String.IsNullOrEmpty(textCode.Text))
            {
                //заполнение списка содержимым textBox игнорируя пустые строки
                string[] textBoxLines = textCode.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                codeList.Clear();
                for (int i = 0; i < textBoxLines.Length; i++)
                    codeList.Add(textBoxLines[i]);
                //удаление строк состоящих из пробелов
                for (int j = 0; j < codeList.Count; j++)
                    if (codeList[j].Trim() == String.Empty)
                        codeList.Remove(codeList[j]);
            }
            if (!String.IsNullOrEmpty(textResult.Text))
            {
                //заполнение списка содержимым textBox игнорируя пустые строки
                string[] textBoxLines = textResult.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                workResult.Clear();
                for (int i = 0; i < textBoxLines.Length; i++)
                    workResult.Add(textBoxLines[i]);
            }
        }

        private void lab1Button_Click(object sender, EventArgs e)
        {
            LexemeAnalyzer lexeme = new LexemeAnalyzer("Lab_1", Environment.CurrentDirectory + @"\",ref codeList);
            workResult.Clear();
            workResult = lexeme.Start();
            UpdateForm();
            windowControl.SelectTab(1);
            MessageBox.Show("Лексемный анализ завершен. Результаты сохранены.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lab2Button.Enabled = true;
            saveResult1.Enabled = true;
        }

        private void lab2Button_Click(object sender, EventArgs e)
        {
            codeList = new List<string>( workResult);
            UpdateForm();
            ReversePolishNotation RPN = new ReversePolishNotation("Lab_2", Environment.CurrentDirectory + @"\", ref workResult);
            
            workResult.Clear();
            workResult = RPN.Start();
            UpdateForm();
            windowControl.SelectTab(1);
            MessageBox.Show("Перевод в ОПЗ завершен. Результаты сохранены.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lab3Button.Enabled = true;
        }

        private void lab3Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Будет готова, когда будет готова.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveResult1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textCode.Text))
            {
                //подготовка списка к сохранению в файл
                ListUpdate();
                saveFileDialog.InitialDirectory = folderPath;
                saveFileDialog.Filter = "Text file (*.txt)|*.txt| Все файлы (*.*)|*.*";
                saveFileDialog.FileName = "Result.txt";
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                    if (FileLab.SaveFile(saveFileDialog.FileName, ref workResult))
                        MessageBox.Show("Файл успешно сохранен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Что-то пошло не так, файл не сохранился.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}
