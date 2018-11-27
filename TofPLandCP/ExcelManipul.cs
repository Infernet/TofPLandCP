using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
namespace TofPLandCP
{
    class ExcelManipul
    {
        /// <summary>
        /// экземпляр приложения
        /// </summary>
        private Excel.Application ExcelApp;
        /// <summary>
        /// экземпляр рабочей книги
        /// </summary>
        private Excel.Workbook ExcelBook;
        /// <summary>
        /// экземпляр листа Excel
        /// </summary>
        private Excel.Worksheet ExcelSheet;
        /// <summary>
        /// Имя текущего файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Текущая директория 
        /// </summary>
        public string UsingFolder { get; set; }
        /// <summary>
        /// True, если приложение запущено
        /// </summary>
        public bool IsOpen { get;private set; }



        public ExcelManipul()
        {
            //FileName = name;
            //UsingFolder = folder;
            //IsOpen = false;
            ////открытие экземпляра Excel
            //ExcelApp = new Excel.Application();
            ////откритие книги
            ////ExcelBook = ExcelApp.Workbooks.Open(UsingFolder + FileName+".xlsx", 0, writeMode, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            //ExcelBook = ExcelApp.Workbooks.Open(UsingFolder + FileName + ".xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            ////выбор таблицы
            //ExcelSheet = (Excel.Worksheet)ExcelBook.Sheets[1];

            /*
            string str = File.ReadAllText(@"E:\General\YandexDisk\Programming\C#\project\University\ТЯПиВП\TofPLandCP\Source\Automate_table_file.txt");
            string[] table = str.Split(new string[] { " ", "\n\r", "\r\n", "\t", "_" }, StringSplitOptions.RemoveEmptyEntries);

            int k = 0;
            for (int i = 3; i < 27; i++)
                for (int j = 3; j < 25; j++)
                {
                    ExcelSheet.Cells[i, j] = table[k].ToString();
                    k++;
                }

            Close();
            */
        }

        /// <summary>
        /// Возвращает значение ячейки
        /// </summary>
        /// <param name="column">Столбец</param>
        /// <param name="row">Ряд</param>
        /// <returns></returns>
        public string GetCell(int column, int row)
        {
            if (column > 0 && row > 0)
                return ExcelSheet.Cells[row, column].Text;
            else
            {
                Environment.Exit(1);
                return "";
            }
        }
        /// <summary>
        /// Метод закрывающий файл и приложение
        /// </summary>
        public void Close()
        {
            ExcelBook.Close(true, Type.Missing, Type.Missing);
            ExcelApp.Quit();
            ExcelApp = null;
            ExcelBook = null;
            ExcelSheet = null;
            GC.Collect();
            
        }
        /// <summary>
        /// Открывает приложение и соответствующие элементы для работы
        /// </summary>
        public void Open(string fileName,string Folder,bool writeMode,int selectedSheets=1)
        {
            FileName = fileName;
            UsingFolder = Folder;
            IsOpen = true;
            //открытие экземпляра Excel
            ExcelApp = new Excel.Application();
            //откритие книги
            ExcelBook = ExcelApp.Workbooks.Open(UsingFolder + FileName + ".xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //выбор таблицы
            ExcelSheet = (Excel.Worksheet)ExcelBook.Sheets[selectedSheets];
        }
    }
}
