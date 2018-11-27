using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TofPLandCP
{
    static class FileLab
    {
        //Сохранение файла
        public static bool SaveFile(string FilePath,ref List<string> list)
        {
            //создание нового файла
            if (!File.Exists(FilePath))
                File.Create(FilePath).Close();
            using (StreamWriter output = new StreamWriter(FilePath, false))
            {
                foreach (string listLine in list)
                    output.WriteLine(listLine);
            }
            if (File.Exists(FilePath))
                return true;
            else
                return false;
        }
        //Открытие и считывание информации из файла
        public static bool OpenFile(string FilePath,ref List<string> codeList)
        {
            List<string> newList = new List<string>();
            using (StreamReader input = new StreamReader(FilePath))
            {
                while (!input.EndOfStream)
                    newList.Add(input.ReadLine());      
            }
            if (newList.Count > 0)
            {
                codeList = newList;
                return true;
            }
            else
                return false;
        }
    }
}
