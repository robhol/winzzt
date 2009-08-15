using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinZZT
{
    class CScript
    {
        //static public Dictionary<string, object> GlobalVars;
        //public Dictionary<string, object> Vars;

        public CObject Object;
        public EScriptState State = EScriptState.Idle;

        private int Line = 0;
        private string[] Lines;

        public CScript(CObject eObject, string script)
        {

            Object = eObject;

            //Trim all lines
            string[] rawLines = script.Split("\r\n".ToCharArray());
            Lines = new string[rawLines.Length];

            for (int i = 0; i < rawLines.Length; i++)
            {
                Lines[i] = rawLines[i].Trim();
            }
               

        }

        private int FindLabel(string s)
        {
            int ret = -1;

            for (int i = 0; i < Lines.Length; i++)
            {
                if (Lines[i].Length > 1 && Lines[i].Substring(0, 1) == ":" && Lines[i].Substring(1) == s)
                    ret = i;
            }

            return ret;

        }

        public void JumpToLabel(string s)
        {
            int ln = FindLabel(s);

            if (ln != -1)
                Line = ln;
        }

        private bool Execute(string s)
        {   
            //Executes a line. 
            //NOTE: If returning true, will wait for next cycle.

            string[] args = s.Trim().Split(" ".ToCharArray());

            switch (args[0].ToUpper())
            {
                case "END":
                    Line = Lines.Length;
                    return false;

                case "WALK":
                    Object.Try(CUtil.getDirectionFromString(args[1]), true, true);
                    return true;

                case "CHAR":
                    Object.Char = int.Parse(args[1]);
                    return false;

                case "LOCK":
                    Object.Locked = true;
                    return false;

                case "UNLOCK":
                    Object.Locked = false;
                    return false;
                    
            }

            return true;

        }

        public void Step()
        {

            if (Line >= Lines.Length)
                return;

            State = EScriptState.Processing;
            bool waitAfter = false;

            if (Lines[Line] != "")
            {
                switch (Lines[Line].Substring(0, 1))
                {
                    case "#": //Command line
                        waitAfter = Execute(Lines[Line].Substring(1));
                        break;

                    case ":": //Label (for GOTO and "events")
                        break;

                    default:  //Defaulting to displaying text
                        CDrawing.DisplayText(Lines[Line], 1000);
                        break;

                }
            }

            Line++;

            if (Line >= Lines.Length)
                State = EScriptState.Finished;
            else
                State = EScriptState.Idle;

            if (!waitAfter && State == EScriptState.Idle)
                Step();

        }


    }
}
