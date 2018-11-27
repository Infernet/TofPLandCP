using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TofPLandCP
{
    class LexemeAnalyzer
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Рабочая папка
        /// </summary>
        public string WorkForlder { get; private set; }


        /// <summary>
        /// Исходный код
        /// </summary>
        private List<string> Code = new List<string>();
        /// <summary>
        /// Результат работы, выходной файл
        /// </summary>
        private List<string> ResultWork = new List<string>();

        //Известные данные до запуска
        /// <summary>
        /// Таблица переходов
        /// </summary>
        private List<List<Cell>> AutomateTable = new List<List<Cell>>();
        /// <summary>
        /// Список операций
        /// </summary>
        private List<string> Operation = new List<string>();
        /// <summary>
        /// Список разделителей
        /// </summary>
        private List<string> Delimiter = new List<string>();
        /// <summary>
        /// Список служебных слов
        /// </summary>
        private List<string> Word = new List<string>();

        //Данные, заполняемые в процессе работы
        /// <summary>
        /// Список числовых констант
        /// </summary>
        private List<string> Number = new List<string>();
        /// <summary>
        /// Список символьных констант
        /// </summary>
        private List<string> Character = new List<string>();
        /// <summary>
        /// Список идентификаторов
        /// </summary>
        private List<string> Identifier = new List<string>();
        /// <summary>
        /// Список меток
        /// </summary>
        private List<string> Mark = new List<string>();

        //Буфер и текущее состояние автомата
        /// <summary>
        /// Буфер
        /// </summary>
        private string LexemeBuffer = "";
        /// <summary>
        /// Текущий индекс в строке кода
        /// </summary>
        private int CurrentCodeIndex = 0;
        /// <summary>
        /// Текущая строка кода
        /// </summary>
        private int CurrentLine = 0;
        /// <summary>
        /// Текущее состояние автомата
        /// </summary>
        private int CurrentState = 2;
        /// <summary>
        /// Текущая строка записи результата
        /// </summary>
        private int CurrentLResult = 0;
        /// <summary>
        /// Проверяет соблюдение GO TO (mark), true когда GO TO был обнаружен
        /// </summary>
        private bool IsGOTO = false;


        public LexemeAnalyzer(string name, string directory,ref List<string> code)
        {
            Name = name;
            WorkForlder = directory;
            Code = code;
            //создает рабочую директорию для удобной работы
            if (!Directory.Exists(WorkForlder + Name))
                Directory.CreateDirectory(WorkForlder + Name);
            LoadTable();
            LoadData();
        }



        public List<string> Start()
        {
            //добавление первой страницы результата
            ResultWork.Add("");
            while (CurrentLine <= Code.Count)
            {

                if (CurrentCodeIndex == 0)
                {
                    //процедура поиска комментария или метки
                    procedure_0_definition_mark_or_comment();
                    CurrentState = 2;
                    //запуск анализа следующего состояния
                    Next_State();
                    continue;
                }
                //загрузка символа в буффер
                LexemeBuffer_push();
                //запуск анализа следующиего состояния
                Next_State();

                if ((CurrentCodeIndex >= Code[CurrentLine].Length) && !(CurrentLine == Code.Count - 1))
                {
                    CurrentLResult++;
                    CurrentLine++;
                    CurrentCodeIndex = 0;
                    ResultWork.Add("");
                }
                if (CurrentState == -1)
                {
                    //проверка на конец
                    //сохранение результатов и выход
                    SaveResult();
                    return ResultWork;
                }
            }
            SaveResult();
            return ResultWork;
        }


        /// <summary>
        /// функция захвата символа в буфер, с проверкой конца блока кода
        /// </summary>
        private void LexemeBuffer_push()
        {
            if (CurrentCodeIndex != Code[CurrentLine].Length)
            {
                LexemeBuffer += Code[CurrentLine][CurrentCodeIndex];
                CurrentCodeIndex++;
            }
            else
            {
                if (LexemeBuffer == "" && CurrentLine != Code.Count)
                {
                    //перевод указателя и анализируемой строки кода
                    CurrentLine++;
                    CurrentCodeIndex = 0;
                    LexemeBuffer += Code[CurrentLine][CurrentCodeIndex];
                    //для результата переход на след. строку
                    ResultWork.Add("");
                    CurrentLResult++;
                }
                else
                    CurrentState = -1;
            }
        }


        /// <summary>
        /// Анализ перехода и выполнение процедур
        /// </summary>
        private void Next_State()
        {
            int procedure = 0;
            int new_state = 0;
            int resultAnalyzeChar = CharAnalyze()-1;
            //конец строки
            if (resultAnalyzeChar == -2)
            {
                CurrentState = -1;
                return;
            }
            procedure = AutomateTable[CurrentState][resultAnalyzeChar].Procedure;
            new_state = AutomateTable[CurrentState][resultAnalyzeChar].State;
            if (new_state == 0)
            {
                Console.WriteLine("Ошибка перехода после анализа");
                Environment.Exit(2);
            }
            CurrentState = new_state;
            if (procedure != 0)
                switch (procedure)
                {
                    case 3:
                        procedure_3_identifier();
                        break;
                    case 4:
                        procedure_4_word();
                        break;
                    case 5:
                        procedure_5_numeric_const();
                        break;
                    case 6:
                        procedure_6_numeric_and_dot_operation();
                        break;
                    case 7:
                        procedure_7_operation();
                        break;
                    case 8:
                        procedure_8_character_const();
                        break;
                    case 9:
                        procedure_9_delimiter();
                        break;

                    default:
                        Console.WriteLine("Ошибка выбора процедуры: " + procedure);
                        Environment.Exit(3);
                        break;
                }
        }
        /// <summary>
        /// Анализ следующего символа
        /// </summary>
        /// <returns>Код типа символа</returns>
        private int CharAnalyze()
        {
            if (CurrentCodeIndex >= Code[CurrentLine].Length)
                if (CurrentLine == Code.Count - 1)
                    return -1;
                else
                {
                    return 5;
                }
            //проверка на комментарий в 0 позиции
            char charAnalyze = Code[CurrentLine][CurrentCodeIndex];
            //символ С
            if (charAnalyze == 'C')
                return 1;
            //символ E
            if (charAnalyze == 'E')
                return 2;
            //буква
            if ('A' <= charAnalyze && charAnalyze <= 'Z')
                return 3;
            //число
            if ('0' <= charAnalyze && charAnalyze <= '9')
                return 4;
            //разделитель
            foreach (string str in Delimiter)
                if (str == charAnalyze.ToString())
                    return 5;
            //точка
            if (charAnalyze == '.')
                return 6;
            //знак
            if (charAnalyze == '+' || charAnalyze == '-')
                return 7;
            //звезда
            if (charAnalyze == '*')
                return 8;
            //операция
            foreach (string str in Operation)
                if (str == charAnalyze.ToString())
                    return 9;
            //двойные ковычки
            if (charAnalyze == '\"')
                return 10;
            if (charAnalyze == '\'')
                return 11;

            Console.WriteLine("Символ {0} не найден, завершение работы.", charAnalyze);
            Console.ReadKey();
            Environment.Exit(1);
            return -2;
        }
        /// <summary>
        /// Сохранение результатов работы
        /// </summary>
        private void SaveResult()
        {
            FileLab.SaveFile(WorkForlder + Name + @"\Identifier.txt",ref Identifier);
            FileLab.SaveFile(WorkForlder + Name + @"\Number.txt", ref Number);
            FileLab.SaveFile(WorkForlder + Name + @"\Character.txt", ref Character);
            FileLab.SaveFile(WorkForlder + Name + @"\Mark.txt", ref Mark);
            FileLab.SaveFile(WorkForlder + Name + @"\Result.txt", ref ResultWork);
            FileLab.SaveFile(WorkForlder + Name + @"\Fortran.txt", ref Code);
        }
        /// <summary>
        /// Загрузка таблицы из Excel файла
        /// </summary>
        private void LoadTable()
        {
            ExcelManipul excelManipul = new ExcelManipul();
            excelManipul.Open(Name + "_Table", WorkForlder + Name + @"\", false);
            //получение ячейки начала
            int startCell = Int32.Parse(excelManipul.GetCell(1, 1));
            //получение крайней правой ячейки
            int rightEndCell = Int32.Parse(excelManipul.GetCell(2, 1));
            //получение нижней границы
            int downEndCell = Int32.Parse(excelManipul.GetCell(1, 2));

            for (int row = startCell; row < (startCell + downEndCell); row++)
            {
                AutomateTable.Add(new List<Cell>());
                for (int column = startCell; column < (startCell + rightEndCell); column += 2)
                {
                    //заполнение таблицы
                    AutomateTable[row - startCell].Add(new Cell() { State = Int32.Parse(excelManipul.GetCell(column, row)), Procedure = Int32.Parse(excelManipul.GetCell(column + 1, row)) });
                }
            }
            excelManipul.Close();
        }
        /// <summary>
        /// Заполнение списков данными о языке Fortran
        /// </summary>
        private void LoadData()
        {
            //Служебные слова
            if (!FileLab.OpenFile(WorkForlder + Name + @"\Word.txt", ref Word))
            {
                Console.WriteLine("Ошибка открытия файла Word.txt, завершение работы.");
                Environment.Exit(9);
            }
            //Операции
            if(!FileLab.OpenFile(WorkForlder + Name + @"\Operation.txt", ref Operation))
            {
                Console.WriteLine("Ошибка открытия файла Operation.txt, завершение работы.");
                Environment.Exit(9);
            }
            //Разделители
            if(!FileLab.OpenFile(WorkForlder + Name + @"\Delimiter.txt", ref Delimiter))
            {
                Console.WriteLine("Ошибка открытия файла Delimiter.txt, завершение работы.");
                Environment.Exit(9);
            }

        }

        /// <summary>
        /// процедура определения с 0 по 5 позицию строки комментария или метки
        /// </summary>
        private void procedure_0_definition_mark_or_comment()
        {
                //Проверка на 'C', т.е. комментарий
                if (CharAnalyze() == 1)
                {
                    procedure_1_comment();
                    return;
                }
                //Проверка позиции с 1 по 5 на наличие метки
                while (CurrentCodeIndex != 5 && CharAnalyze() != 4)
                {
                    LexemeBuffer_push();
                }
                if (CharAnalyze() == 4 && CurrentCodeIndex != 5)
                {
                    procedure_2_mark();
                    return;
                }
                LexemeBuffer_push();

                //смещение вывода исходного файла для отделения программы от метки
                //!!!быть внимательней здесь!!!
                ResultWork[CurrentLResult] += LexemeBuffer;
                for (int i = 0; i < (6 - CurrentCodeIndex); i++)
                {
                    ResultWork[CurrentLResult] += " ";
                }
                LexemeBuffer = "";
        }
        /// <summary>
        /// процедура обработки комментария
        /// </summary>
        private void procedure_1_comment()
        {
            //игнорирование строки, т.е. считывание всей строки, очистка буфера, обнуление счетчика позиции
            CurrentLine++;
            LexemeBuffer = "";
            CurrentCodeIndex = 0;
        }
        /// <summary>
        /// процедура обработки метки
        /// </summary>
        private void procedure_2_mark()
        {
            //очистка буфера от пробелов
            LexemeBuffer="";

            //Сбор в буфер метки до окончания притока цифр, либо выход за диапозон области расположения метки в строке
            while (CharAnalyze() == 4 && CurrentCodeIndex != 5)
            {
                LexemeBuffer_push();
            }
            //занесение в таблицу меток (если такого числа нет), запись токена, очистка буфера
            //дабы выделить позицию метки, используется измененная версия процедуры идентификатора

            //поиск среди записанных
            //токен
            string token="M";
            foreach (string mark in Mark)
            {
                if (mark == LexemeBuffer)
                {
                    token += Mark.IndexOf(mark);
                    ResultWork[CurrentLResult] += token + " ";
                    CurrentCodeIndex = (6 - CurrentCodeIndex);
                    LexemeBuffer = "";
                    return;
                }
            }
            //запись нового в таблицу
            Mark.Add(LexemeBuffer);
            token += Mark.IndexOf(LexemeBuffer);
            ResultWork[CurrentLResult] += token + " ";
            //очистка буфера
            LexemeBuffer = "";
            //смещение указателя строки кода для считывания области программы
            CurrentCodeIndex+= (6 - CurrentCodeIndex);
        }
        /// <summary>
        /// процедура поиска присутствия уже записанного идентификатора, либо создание новой записи
        /// </summary>
        private void procedure_3_identifier()
        {
            //поиск среди записанных
            string token="I";
            foreach (string identifier in Identifier)
                if (identifier == LexemeBuffer)
                {
                    token += Identifier.IndexOf(identifier);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }
            //запись нового идентификатора
            token += Identifier.Count;
            Identifier.Add(LexemeBuffer);
            LexemeBuffer = "";
            ResultWork[CurrentLResult] += token + " ";
        }
        /// <summary>
        /// процедура обработки служебного слова, поиск по таблице, если не обнаружено, вызов процедуры обработки идентификатора
        /// </summary>
        private void procedure_4_word()
        {
            //Поиск среди имеющихся в файле служебных слов
            string token="W";
            foreach (string word in Word)
            {
                if(word=="GO TO" && LexemeBuffer == "GO")
                {
                    int goToCount = 0;
                    while(LexemeBuffer!="GO TO")
                    {
                        LexemeBuffer_push();
                        goToCount++;
                        if (goToCount > 3)
                        {
                            Console.WriteLine("Ошибка при выявлении служебного слова GO TO, подозрение использования GO, как идентификатора");
                            Environment.Exit(4);
                        }
                    }
                    token += Word.IndexOf(word);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    IsGOTO = true;
                    return;
                }
                if (word == LexemeBuffer)
                {
                    token += Word.IndexOf(word);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }
            }
            //Если не обнаружено, запуск процедуры идентификатора
            procedure_3_identifier();
            return;
        }
        /// <summary>
        /// процедура обработки числовых констант
        /// </summary>
        private void procedure_5_numeric_const()
        {
            //проверка на GOTO
            string token = "";
            if (IsGOTO)
            {
                foreach (string mark in Mark)
                    if (mark == LexemeBuffer)
                    {
                        token += "M" + Mark.IndexOf(mark);
                        ResultWork[CurrentLResult] += token + " ";
                        IsGOTO = false;
                        LexemeBuffer = "";
                        return;
                    }

                token += "M" + Mark.Count;
                Mark.Add(LexemeBuffer);
                ResultWork[CurrentLResult] += token + " ";
                IsGOTO = false;
                LexemeBuffer = "";
                return;

            }
            //Поиск среди имеющихся в файле числовых констант
            token += "N";
            foreach (string number in Number)
                if (number == LexemeBuffer)
                {
                    token += Number.IndexOf(number);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }
            //Запись нового
            token += Number.Count;
            Number.Add(LexemeBuffer);
            ResultWork[CurrentLResult] += token + " ";
            LexemeBuffer = "";
        }
        /// <summary>
        /// частный случай, когда после цифры идет точка, а за ней буква, отсечение из буфера точки, вызов процедуры обработки числовых констант, возврат точки в буфер
        /// </summary>
        private void procedure_6_numeric_and_dot_operation()
        {
            //отделение точки от буфера
            string tmp_for_dot = LexemeBuffer.Substring(LexemeBuffer.Length - 1, 1);
            LexemeBuffer = LexemeBuffer.Substring(0, LexemeBuffer.Length - 1);
            //вызов численной константы
            procedure_5_numeric_const();
            //возврат точки в буффер
            LexemeBuffer = tmp_for_dot;
        }
        /// <summary>
        /// процедура обработки операций
        /// </summary>
        private void procedure_7_operation()
        {
            //Поиск кода операции
            string token = "O";
            foreach (string operation in Operation)
                if (LexemeBuffer == operation)
                {
                    token += Operation.IndexOf(operation);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }
            Console.WriteLine("Ошибка на процедуре 7 (операции), неизвестная операция");
            Environment.Exit(7);
        }
        /// <summary>
        /// процедура обработки символьных констант
        /// </summary>
        private void procedure_8_character_const()
        {
            //Поиск среди имеющихся в файле числовых констант
            string token = "S";
            foreach (string charConst in Character)
                if (charConst == LexemeBuffer)
                {
                    token += Character.IndexOf(charConst);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }
            //Запись нового
            token += Character.Count;
            ResultWork[CurrentLResult] += token + " ";
            Character.Add(LexemeBuffer);
            LexemeBuffer = "";
        }
        /// <summary>
        /// процедура обработки разделителей, если обнаружен конец строки, сброс счетчика текущей позиции символа в файле
        /// </summary>
        private void procedure_9_delimiter()
        {
            //Поиск кода операции
            string token = "D";
            foreach (string delimiter in Delimiter)
                if (delimiter == LexemeBuffer)
                {
                    token += Delimiter.IndexOf(delimiter);
                    ResultWork[CurrentLResult] += token + " ";
                    LexemeBuffer = "";
                    return;
                }

            Console.WriteLine("Ошибка на процедуре 9 (разделители), неизвестный разделитель");
            Environment.Exit(6);
        }


    }
}
