using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TofPLandCP
{
    /// <summary>
    /// Состояние в стеке по таблице 0
    /// </summary>
    enum State_0
    {
        EMPTY=0,
        Скобка,
        iA,
        IF,
        TiIF,
        iProc,
        iDef,
        iTYPE,
        RETURN,
        STOP,
        Равно,
        GOTO,
        CALL,
        ОП3,
        ОП4,
        ОП5,
        ОП6,
    }
    /// <summary>
    /// Состояние в стеке по таблице 1
    /// </summary>
    enum State_1
    {
        А1=0,
        OTHER
    }
    /// <summary>
    /// Состояние в стеке по таблице 2
    /// </summary>
    enum State_2
    {
        IF=0,
        OTHER
    }
}
