using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TofPLandCP
{
    class ReversePolishNotation
    {
        //поля для работы
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Рабочая папка
        /// </summary>
        public string WorkForlder { get; private set; }

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
        /// Текущая строка записи результата
        /// </summary>
        private int CurrentLResult = 0;
        /// <summary>
        /// Текущая таблица
        /// </summary>
        private int CurrentTable = 0;
        /// <summary>
        /// Код в виде токенов
        /// </summary>
        private List<List<string>> LexemeCode = new List<List<string>>();
        /// <summary>
        /// Результат работы, выходной файл
        /// </summary>
        private List<string> ResultWork = new List<string>();

        /// <summary>
        /// Таблица 0
        /// </summary>
        private List<List<string[]>> Table_0 = new List<List<string[]>>();
        /// <summary>
        /// Таблица 1
        /// </summary>
        private List<List<string[]>> Table_1 = new List<List<string[]>>();
        /// <summary>
        /// Таблица 2
        /// </summary>
        private List<List<string[]>> Table_2 = new List<List<string[]>>();
        /// <summary>
        /// Таблица 3
        /// </summary>
        private List<List<string[]>> Table_3 = new List<List<string[]>>();
        /// <summary>
        /// Стек для автомата
        /// </summary>
        private Stack<Stack_Cell> StackMP = new Stack<Stack_Cell>();

        //Служебные счетчики

        /// <summary>
        /// Счетчик служебной метки IF
        /// </summary>
        private int TmarkCount = 1;
        /// <summary>
        /// Счетчик служебный номера процедуры
        /// </summary>
        private int IprocCount = 1;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя работы</param>
        /// <param name="directory">Рабочая папка</param>
        /// <param name="code">Ссылка на код программы</param>
        public ReversePolishNotation(string name, string directory, ref List<string> code)
        {
            Name = name;
            WorkForlder = directory;
            //создает рабочую директорию для удобной работы
            if (!Directory.Exists(WorkForlder + Name))
                Directory.CreateDirectory(WorkForlder + Name);
            LoadTable();

            //Заполнение списка лексем из первой лабороторной
            int count = 0;
            foreach (string lexemes in code)
            {
                string[] tmp = lexemes.Split(new String[] { "D0", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length != 0)
                    LexemeCode.Add(new List<string>());
                for (int i = 0; i < tmp.Length; i++)
                    LexemeCode[count].Add(tmp[i]);
                LexemeCode[count].Add("ENDL");
                count++;
            }


        }


        /// <summary>
        /// Запуск автомата
        /// </summary>
        /// <returns></returns>
        public List<string> Start()
        {
            //добавление первой страницы результата
            ResultWork.Add("");
            while (CurrentLine < LexemeCode.Count)
            {
                //заталкивание
                LexemeBuffer = LexemeCode[CurrentLine][CurrentCodeIndex];
                //оценка операции
                NextState();
                //выполнение процедур
                //инкремент проверяемого символа
                CurrentCodeIndex++;
                LexemeBuffer = "";
            }
            //сохранение результатов
            FileLab.SaveFile(WorkForlder + Name + @"\OPZ.txt", ref ResultWork);
            return ResultWork;
        }

        //методы для работы

        /// <summary>
        /// Определяет список процедур которые необходимо выполнить
        /// </summary>
        private void NextState()
        {
            string[] procedure;
            switch (CurrentTable)
            {
                //0 состояние
                case 0:
                    //получение списка процедур по таблице 0
                    procedure = Table_0[(int)StackCodeTable0()][(int)LexemeEntranceTable0()];
                    //Выполнение полученных инструкций
                    CarryOutProcedure(procedure);
                    break;
                //1 состояние
                case 1:
                    //получение списка процедур по таблице 1
                    procedure = Table_1[(int)StackCodeTable1()][(int)LexemeEntranceTable1()];
                    //Выполнение полученных инструкций
                    CarryOutProcedure(procedure);
                    break;
                //2 состояние
                case 2:
                    //получение списка процедур по таблице 2
                    procedure = Table_2[(int)StackCodeTable2()][(int)LexemeEntranceTable2()];
                    //Выполнение полученных инструкций
                    CarryOutProcedure(procedure);
                    break;
                //3 состояние
                case 3:
                    //получение списка процедур по таблице 2
                    procedure = Table_3[0][(int)LexemeEntranceTable3()];
                    //Выполнение полученных инструкций
                    CarryOutProcedure(procedure);
                    break;
                default:
                    Environment.Exit(6);
                    break;
            }
        }
        /// <summary>
        /// Выполнение полученных инструкций
        /// </summary>
        /// <param name="procedure"></param>
        private void CarryOutProcedure(string[] procedure)
        {
            for (int i = 0; i < procedure.Length; i++)
                RunProcedure(procedure[i]);
        }

        /// <summary>
        /// Анализ и выполнение процедур
        /// </summary>
        /// <param name="command"></param>
        private void RunProcedure(string command)
        {
            //Определение названия процедуры
            string commandName = command.Substring(0, 3);
            //Сложная операция или простая?
            bool OperationType = false;
            if (command.Length > 3)
                OperationType = true;
            //Разделение по сложности операции
            //Сложные
            if (OperationType)
            {
                //Уточнение выполняемой операции
                //Вырезание самой пояснения т.к. формат сложной БББ(пояснение)
                string OperationMode = command.Substring(4,command.Length-5);
                RunHardProcedure(commandName, OperationMode);
            }
            //Простые
            else
                //Исполение простой операции
                switch (commandName)
                {
                    //Переход к новой строке
                    case "Not":
                        //сброс указателя лексемы в начало строки
                        CurrentCodeIndex = -1;
                        //переход к чтению следующей строки
                        CurrentLine++;
                        if(CurrentLine<LexemeCode.Count)
                            ResultWork.Add("");
                        //переход записи результата на новую строку
                        CurrentLResult++;
                        LexemeBuffer = "";
                        break;
                    case "Выт":
                        //Выталкивание из вершины стека содержимого в выходную строку
                        if (StackMP.Peek().Count > 0)
                            ResultWork[CurrentLResult] += StackMP.Peek().Count + " ";
                        ResultWork[CurrentLResult] += StackMP.Peek().Lexeme + " ";
                        break;
                    case "Пуш":
                        //запись в верхину стека неидексируемой лексемы
                        StackMP.Push(new Stack_Cell { Count = 0, Lexeme = LexemeBuffer });
                        break;
                    case "Pop":
                        //выталкивание из стека вершины
                        StackMP.Pop();
                        break;
                    case "Држ":
                        //удерживание текущего анализируемого символа
                        CurrentCodeIndex--;
                        LexemeBuffer = "";
                        break;

                    default:
                        Environment.Exit(70);
                        break;
                }
        }
        /// <summary>
        /// Выполение сложной операции
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="operationMode"></param>
        private void RunHardProcedure(string commandName, string operationMode)
        {
            //Определение типа сложной операции
            switch (commandName)
            {
                //Смена рабочей таблицы состояний
                case "Tbl":
                    if (operationMode.Length == 1)
                        CurrentTable = Int32.Parse(operationMode);
                    else
                        Environment.Exit(70);
                    break;
                //Push в стек iProc iDef iType
                case "Пуш":
                    switch (operationMode)
                    {
                        //iType
                        case "iTYPE":
                            StackMP.Push(new Stack_Cell() { Count = 1, Lexeme = LexemeBuffer });
                            break;
                        //iProc
                        case "iProc":
                            StackMP.Push(new Stack_Cell() { Count = (IprocCount++), Lexeme = "Proc" });
                            break;
                        //iDef
                        case "iDef":
                            StackMP.Push(new Stack_Cell() { Count = 1, Lexeme = "Def" });
                            break;
                        //1A
                        case "1A":
                            StackMP.Push(new Stack_Cell() { Count = 1, Lexeme = "A" });
                            break;
                        default:
                            break;
                    }
                    break;
                //Вывод в выходную строку
                case "Вых":
                    switch (operationMode)
                    {
                        //Протолкнуть буфер в результирующую строку
                        case "X":
                            ResultWork[CurrentLResult] += LexemeBuffer + " ";
                            break;
                        //Начало процедуры
                        case "iНП":
                            ResultWork[CurrentLResult] += StackMP.Peek().Count + " НП ";
                            break;
                        //Конец описания
                        case "iКО":
                            ResultWork[CurrentLResult] += StackMP.Peek().Count + " DF ";
                            ResultWork[CurrentLResult] += IprocCount-1 + " КО ";
                            break;
                        //Конец процедуры
                        case "КП":
                            ResultWork[CurrentLResult] += "КП ";
                            break;
                        //Безусловный переход
                        case "БП":
                            ResultWork[CurrentLResult] += "БП ";
                            break;
                        //Метка Ti IF
                        case "Ti:":
                            ResultWork[CurrentLResult] += "T" + StackMP.Peek().Count + ": ";
                            break;
                        //Ti УПЛ
                        case "iУПЛ":
                            ResultWork[CurrentLResult] += "T" + TmarkCount + " УПЛ ";
                            break;
                    }
                    break;
                //Замена в вершине стека
                case "Зам":
                    //Буферная ячейка для замены в вершине стека
                    Stack_Cell newCell = StackMP.Peek();
                    switch (operationMode)
                    {
                        //Инкрементация счетчика в вершине
                        case "i++":
                            newCell.Count++;
                            StackMP.Pop();
                            StackMP.Push(newCell);
                            break;
                        //Ti IF
                        case "iIF":
                            //замена вершины стека с ( IF) на (i IF)
                            newCell.Count = TmarkCount;
                            TmarkCount++;
                            StackMP.Pop();
                            StackMP.Push(newCell);
                            break;
                    }
                    break;
                
            }
        }

        /// <summary>
        /// Загрузка таблицы из Excel файла
        /// </summary>
        private void LoadTable()
        {
            ExcelManipul excelManipul = new ExcelManipul();
            excelManipul.Open(Name + "_Table", WorkForlder + Name + @"\", false);

            //Таблица 0

            //получение ячейки начала
            int startCell = Int32.Parse(excelManipul.GetCell(2, 3));
            //получение правой границы
            int rightCell = Int32.Parse(excelManipul.GetCell(2, 4));
            //получение нижней границы
            int downCell = Int32.Parse(excelManipul.GetCell(2, 5));

            //Int32.Parse(excelManipul.GetCell(column, row))

            for (int row = startCell; row <= downCell; row++)
            {
                Table_0.Add(new List<string[]>());
                for (int column = startCell; column <= rightCell; column++)
                {
                    //string[] textBoxLines = textResult.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    //заполнение таблицы
                    Table_0[row - startCell].Add(excelManipul.GetCell(column, row).Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            //Таблица 1

            //ряд стартовой ячейки
            int startRow = Int32.Parse(excelManipul.GetCell(2, 23));
            //правая граница
            rightCell = Int32.Parse(excelManipul.GetCell(2, 24));
            //нижняя граница
            downCell = Int32.Parse(excelManipul.GetCell(2, 25));

            for (int row = startRow; row <= downCell; row++)
            {
                Table_1.Add(new List<string[]>());
                for (int column = startCell; column <= rightCell; column++)
                {
                    //заполнение таблицы
                    Table_1[row - startRow].Add(excelManipul.GetCell(column, row).Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }


            //Таблица 2

            //ряд стартовой ячейки
            startRow = Int32.Parse(excelManipul.GetCell(2, 28));
            //правая граница
            rightCell = Int32.Parse(excelManipul.GetCell(2, 29));
            //нижняя граница
            downCell = Int32.Parse(excelManipul.GetCell(2, 30));

            for (int row = startRow; row <= downCell; row++)
            {
                Table_2.Add(new List<string[]>());
                for (int column = startCell; column <= rightCell; column++)
                {
                    //заполнение таблицы
                    Table_2[row - startRow].Add(excelManipul.GetCell(column, row).Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }



            //Таблица 3

            //ряд стартовой ячейки
            startRow = Int32.Parse(excelManipul.GetCell(2, 33));
            //правая граница
            rightCell = Int32.Parse(excelManipul.GetCell(2, 24));
            //нижняя граница
            downCell = Int32.Parse(excelManipul.GetCell(2, 35));

            for (int row = startRow; row <= downCell; row++)
            {
                Table_3.Add(new List<string[]>());
                for (int column = startCell; column <= rightCell; column++)
                {
                    //заполнение таблицы
                    Table_3[row - startRow].Add(excelManipul.GetCell(column, row).Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }


            excelManipul.Close();
        }

        /// <summary>
        /// Определение что лежит на вершине стека по таблице 0
        /// </summary>
        /// <returns></returns>
        private State_0 StackCodeTable0()
        {
            //пустой ли стек?
            if (StackMP.Count == 0)
                return State_0.EMPTY;
            //индексируемый ли элемент в вершине?
            if (StackMP.Peek().Count == 0)
                switch (StackMP.Peek().Lexeme)
                {
                    //Скобка
                    case "D2":
                        return State_0.Скобка;
                    //IF
                    case "W13":
                        return State_0.IF;
                    //RETURN
                    case "W10":
                        return State_0.RETURN;
                    //STOP
                    case "W1":
                        return State_0.STOP;
                    //Равно
                    case "O5":
                        return State_0.Равно;
                    //GO TO
                    case "W12":
                        return State_0.GOTO;
                    //CALL
                    case "W3":
                        return State_0.CALL;
                    //Операции по приоритету
                    default:
                        switch (OperationPriority(StackMP.Peek().Lexeme))
                        {
                            case 3:
                                return State_0.ОП3;
                            case 4:
                                return State_0.ОП4;
                            case 5:
                                return State_0.ОП5;
                            case 6:
                                return State_0.ОП6;
                            default:
                                //ошибка
                                Environment.Exit(6);
                                return State_0.EMPTY;
                        }
                }
            //Индексируемые элементы в вершине
            else
                switch (StackMP.Peek().Lexeme)
                {
                    //i A
                    case "A":
                        return State_0.iA;
                    //Ti IF
                    case "W13":
                        return State_0.TiIF;
                    //i Proc
                    case "Proc":
                        return State_0.iProc;
                    //i Def
                    case "Def":
                        return State_0.iDef;
                    //iTYPE
                    case "W4":
                    case "W5":
                    case "W6":
                    case "W7":
                        return State_0.iTYPE;

                    default:
                        //место под ошибку
                        return State_0.EMPTY;
                }
        }
        /// <summary>
        /// Определение что лежит на вершине стека по таблице 1
        /// </summary>
        /// <returns></returns>
        private State_1 StackCodeTable1()
        {
            //A1
            if (StackMP.Count != 0 && (StackMP.Peek().Count == 1 && StackMP.Peek().Lexeme == "A") )
                return State_1.А1;
            //Другое
            return State_1.OTHER;
        }
        /// <summary>
        /// Определение что лежит на вершине стека по таблице 2
        /// </summary>
        /// <returns></returns>
        private State_2 StackCodeTable2()
        {
            if (StackMP.Count != 0 && (StackMP.Peek().Count == 0 && StackMP.Peek().Lexeme == "W13"))
                return State_2.IF;
            return State_2.OTHER;
        }

        /// <summary>
        /// Определение что пришло на вход по таблице 0
        /// </summary>
        /// <returns></returns>
        private Entrance_0 LexemeEntranceTable0()
        {
            //проверка на конец строки
            if (LexemeBuffer== "ENDL")
                return Entrance_0.ENDL;
            //пуш в буфер

            //проверка класса лексемы
            switch (LexemeBuffer[0])
            {
                //Численные и символьные константы
                case 'N':
                case 'S':
                    return Entrance_0.Константа;
                //Идентификатор
                case 'I':
                    return Entrance_0.Идентификатор;
                //Метка
                case 'M':
                    return Entrance_0.Метка;
                //Операции
                case 'O':
                    {
                        //Операция равно
                        if (LexemeBuffer == "O5")
                            return Entrance_0.Равно;
                        //Операции по приоритету
                        switch (OperationPriority(LexemeBuffer))
                        {
                            case 3:
                                return Entrance_0.ОП3;
                            case 4:
                                return Entrance_0.ОП4;
                            case 5:
                                return Entrance_0.ОП5;
                            case 6:
                                return Entrance_0.ОП6;
                            default:
                                Environment.Exit(7);
                                return Entrance_0.ENDL;
                        }
                    }
                //Служебные слова
                case 'W':
                    switch (LexemeBuffer)
                    {
                        //IF
                        case "W13":
                            return Entrance_0.IF;
                        //GO TO
                        case "W12":
                            return Entrance_0.GOTO;
                        //CALL
                        case "W3":
                            return Entrance_0.CALL;
                        //RETURN
                        case "W10":
                            return Entrance_0.RETURN;
                        //END
                        case "W2":
                            return Entrance_0.END;
                        //TYPE
                        case "W4":
                        case "W5":
                        case "W6":
                        case "W7":
                            return Entrance_0.TYPE;
                        //STOP
                        case "W1":
                            return Entrance_0.STOP;
                        //PROGRAM
                        case "W0":
                            return Entrance_0.PROGRAM;
                        //SUBROUTINE
                        case "W11":
                            return Entrance_0.SUMROUTINE;
                        //FUNCTION
                        case "W8":
                            return Entrance_0.FUNCTION;
                        //PARAMETER
                        case "W9":
                            return Entrance_0.PARAMETER;
                        default:
                            Environment.Exit(8);
                            return Entrance_0.ENDL;
                    }
                //Разделители
                case 'D':
                    switch (LexemeBuffer)
                    {
                        // (
                        case "D2":
                            return Entrance_0.СкобкаО;
                        // )
                        case "D3":
                            return Entrance_0.СкобкаЗ;
                        // ,
                        case "D1":
                            return Entrance_0.Запятая;
                        default:
                            Environment.Exit(9);
                            return Entrance_0.ENDL;
                    }
                default:
                    Environment.Exit(10);
                    return Entrance_0.ENDL;
            }
        }
        /// <summary>
        /// Определение что пришло на вход по таблице 1
        /// </summary>
        /// <returns></returns>
        private Entrance_1 LexemeEntranceTable1()
        {
            //костыль на конец строки
            if (LexemeBuffer == "ENDL")
                return Entrance_1.OTHER;
            switch (LexemeBuffer[0])
            {
                //Численные и символьные константы
                case 'N':
                case 'S':
                    return Entrance_1.Константа;
                //Идентификатор
                case 'I':
                    return Entrance_1.Идентификатор;
                default:
                    switch (LexemeBuffer)
                    {
                        case "D3":
                            return Entrance_1.СкобкаЗ;
                        case "D2":
                            return Entrance_1.СкобкаО;
                        default:
                            return Entrance_1.OTHER;
                    }
            }
        }
        /// <summary>
        /// Определение что пришло на вход по таблице 2
        /// </summary>
        /// <returns></returns>
        private Entrance_2 LexemeEntranceTable2()
        {
            if (LexemeBuffer == "D3")
                return Entrance_2.СкобкаЗ;
            return Entrance_2.OTHER;
        }
        /// <summary>
        /// Определение что пришло на вход по таблице 3
        /// </summary>
        /// <returns></returns>
        private Entrance_3 LexemeEntranceTable3()
        {
            if (LexemeBuffer == "D2")
                return Entrance_3.СкобкаО;
            return Entrance_3.OTHER;
        }


        /// <summary>
        /// Возвращает 3-6 при успешном определении категории операций, 0 при ошибке
        /// </summary>
        /// <returns></returns>
        private int OperationPriority(string lexeme)
        {
            switch (lexeme)
            {
                //Операции 3 приоритета
                case "O6":
                case "O7":
                case "O8":
                case "O9":
                case "O10":
                case "O11":
                    return 3;
                //Операции 4 приоритета
                case "O0":
                case "O1":
                    return 4;
                //Операции 5 приоритета
                case "O2":
                case "O3":
                    return 5;
                //Операции 6 приоритета
                case "O4":
                    return 6;
                default:
                    return 0;
            }
        }
        
    }
}
