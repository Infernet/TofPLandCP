using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TofPLandCP
{
    /// <summary>
    /// Лексема на входе по таблице 0
    /// </summary>
    enum Entrance_0
    {
        Константа=0,
        Идентификатор,
        Метка,
        СкобкаО,
        СкобкаЗ,
        Запятая,
        ENDL,
        IF,
        GOTO,
        CALL,
        RETURN,
        END,
        Равно,
        TYPE,
        STOP,
        PROGRAM,
        SUMROUTINE,
        FUNCTION,
        PARAMETER,
        ОП3,
        ОП4,
        ОП5,
        ОП6
    }
    /// <summary>
    /// Лексема на входе по таблице 1
    /// </summary>
    enum Entrance_1
    {
        СкобкаЗ=0,
        СкобкаО,
        Константа,
        Идентификатор,
        OTHER
    }
    /// <summary>
    /// Лексема на входе по таблице 2
    /// </summary>
    enum Entrance_2
    {
        СкобкаЗ=0,
        OTHER
    }
    /// <summary>
    /// Лексема на входе по таблице 3
    /// </summary>
    enum Entrance_3
    {
        СкобкаО=0,
        OTHER
    }
}
