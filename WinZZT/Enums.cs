/*

Enums.cs

Gathers all significant enumerations in one place.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinZZT
{
    public enum EDirection
    {
        North,
        East,
        South,
        West
    }

    public enum ESliderType
    {
        Horizontal,
        Vertical
    }

    public enum EConveyorType
    {
        Clockwise,
        Counterclockwise
    }

    public enum EScriptState
    {
        Idle,
        Processing,
        Finished
    }

    public enum EGamemode
    {
        Game,
        MapEditor
    }

}
