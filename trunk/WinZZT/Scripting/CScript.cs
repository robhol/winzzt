using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CScript
    {
        //Store a list of global and object/script specific variables
        static public Dictionary<string, string> Vars;

        public CObject Object;
        public EScriptState State = EScriptState.Idle;

        private int Line = 0;
        private string[] Lines;

        public static void Initialize()
        {
            //Initialize global var dictionary
            Vars = new Dictionary<string, string>();

        }

        public static void SetVariable(string name, string value)
        {

            if (!Vars.ContainsKey(name))
                Vars.Add(name, value);
            else
                Vars[name] = value;

        }

        public CScript(CObject eObject, string script)
        {

            //Add reference to object
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
                case "END": //Stops script execution
                    Line = Lines.Length;
                    return false;

                case "WALK": //Tries moving in a direction
                    if (
                        !Object.Try(CUtil.getDirectionFromString(args[1]), true, true)
                        && args.Length > 2
                    )
                        JumpToLabel(args[2]);

                    return true;

                case "CHAR": //Changes char

                    if (args.Length < 2)
                        return false;

                    int chr;
                    if (int.TryParse(args[1], out chr))
                        Object.Char = chr;

                    return false;

                case "GOTO": //Starts executing script at given label

                    if (args.Length < 2)
                        return false;
                    
                    this.JumpToLabel(args[1]);
                    return false;

                case "LOCK": //Prevents object from responding to messages
                    Object.Locked = true;
                    return false;

                case "UNLOCK": //Reverses LOCK effect
                    Object.Locked = false;
                    return false;
                    
                case "CYCLE": //Sets execution speed

                    if (args.Length < 2)
                        return false;

                    int cycle;
                    if (int.TryParse(args[1], out cycle))
                        Object.Cycle = cycle;

                    return false;

                case "DIE": //Removes object
                    Object.Die();
                    return false;

                case "PAUSE": //Does nothing, waits for next cycle/Step
                    return true;

                case "MSG": //Sends a message to an object with the given name

                    if (args.Length < 3)
                        return false;

                    switch (args[1].ToUpper())
                    {
                        case "ALL":
                            CElementManager.SendMessageToObjects(null, args[2]);
                            break;

                        case "OTHERS":
                            CElementManager.SendMessageToObjects(Object, args[2]);
                            break;

                        default:
                            CElementManager.SendMessageToObject(args[1], args[2]);
                            break;
                    }
                    
                    return false;

                case "COLOR": //Changes colors...

                    if (args.Length < 2)
                        return false;

                    Color f;

                    if (CUtil.getColorFromString(args[1], out f))
                        Object.ForeColor = f;

                    Color b;

                    if (args.Length > 2)
                    {
                        if (CUtil.getColorFromString(args[2], out b))
                            Object.BackColor = b;
                    }

                        return false;

                case "BECOME": //Changes the object into whatever

                    if (args.Length < 2)
                        return false;

                    CElementBlueprint bpBecome = new CElementBlueprint(args[1], Color.White);

                    Object.Become(bpBecome);

                    return false;

                case "PUT": //Puts an object in whatever direction

                    if (args.Length < 2)
                        return false;

                    CElementBlueprint bpPut = new CElementBlueprint(args[2], Color.White);

                    Object.Put(CUtil.getDirectionFromString(args[1]), bpPut);

                    return false;

                case "GIVE":

                    if (args.Length < 3)
                        return false;

                    int amt;
                    
                    if (!int.TryParse(args[2], out amt))
                        return false;

                    switch (args[1].ToUpper())
                    {
                        case "AMMO":
                            CGame.PlayerAmmo += amt;
                            break;

                        case "TORCHES":
                            CGame.PlayerTorches += amt;
                            break;

                        case "HEALTH":
                            CGame.PlayerHealth += amt;
                            break;

                        case "GEMS":
                            CGame.PlayerGems += amt;
                            break;

                        case "SCORE":
                            CGame.PlayerScore += amt;
                            break;
                    }

                    return false;

                case "TAKE":
                    {

                        if (args.Length < 3)
                            return false;

                        int takeamt;
                        if (!int.TryParse(args[2], out takeamt))
                            return false;

                        string escapeTo = "";

                        if (args.Length > 2)
                            escapeTo = args[3];

                        switch (args[1].ToUpper())
                        {
                            case "AMMO":

                                if (CGame.PlayerAmmo >= takeamt)
                                    CGame.PlayerAmmo -= takeamt;
                                else if (escapeTo != "")
                                    JumpToLabel(escapeTo);

                                break;

                            case "TORCHES":
                                if (CGame.PlayerTorches >= takeamt)
                                    CGame.PlayerTorches -= takeamt;
                                else if (escapeTo != "")
                                    JumpToLabel(escapeTo);

                                break;

                            case "HEALTH":
                                if (CGame.PlayerHealth >= takeamt)
                                    CGame.DamagePlayer(takeamt);
                                else if (escapeTo != "")
                                    JumpToLabel(escapeTo);

                                break;

                            case "GEMS":
                                if (CGame.PlayerGems >= takeamt)
                                    CGame.PlayerGems -= takeamt;
                                else if (escapeTo != "")
                                    JumpToLabel(escapeTo);

                                break;

                            case "SCORE":
                                if (CGame.PlayerScore >= takeamt)
                                    CGame.PlayerScore -= takeamt;
                                else if (escapeTo != "")
                                    JumpToLabel(escapeTo);

                                break;

                        }

                        return false;

                    }

                case "SHOOT": //Shoots in whatever direction

                    if (args.Length < 2)
                        return false;

                    Object.Shoot(CUtil.getDirectionFromString(args[1]));

                    return false;

                case "SET": //Sets a variable
                    {

                        if (args.Length < 3)
                            return false;

                        SetVariable(args[1], args[2]);

                        break;
                    }

                case "GOTOIF": //Basic IF statement... Validates an expression.
                    {
                        //Input: 
                        //ARG #     1       2     3      4
                        //#GOTOIF label     A  operator  B

                        if (args.Length < 5)
                            return false;

                        bool result = false;

                        //Get operator
                        switch (args[3].ToLower())
                        {
                            case "=":
                                result = args[2] == args[4]; break;

                            case "morethan":
                                result = int.Parse(args[2])  > int.Parse(args[4]); break;

                            case "lessthan":
                                result = int.Parse(args[2]) <  int.Parse(args[4]); break;

                            case "isdefined":
                                result = args[2] != "(undefined)"; break;
                        }

                        //Act accordingly
                        if (result)
                            JumpToLabel(args[1]);

                        return false;
                    }

                case "UNSET": //Unsets/deletes a variable
                    {

                        if (args.Length < 2)
                            return false;

                        if (Vars.ContainsKey(args[1]))
                            Vars.Remove(args[1]);

                        break;
                    }

                case "MATH":
                    {
                        //  arg  1 2  3  4  5
                        // #MATH x = 123 + 456

                        if (args.Length != 6 || args[2] != "=")
                            return false;

                        double ma, mb;
                        if (!double.TryParse(args[3], out ma) || !double.TryParse(args[5], out mb))
                            return false; //Uh oh, something's wrong.

                        double result = -1;
                        switch (args[4]) //Check operator
                        {
                            case "+":
                                result = ma + mb; break;
                            case "-":
                                result = ma - mb; break;
                            case "*":
                                result = ma * mb; break;
                            case "/":
                                result = ma / mb; break;
                        }

                        SetVariable(args[1], result.ToString());

                        return false;
                    }

                case "CHANGE": // Changes all elements of a given type to another.
                    {

                        if (args.Length < 3)
                            return false;

                        CElementBlueprint from = new CElementBlueprint(args[1], Color.Transparent);
                        CElementBlueprint to   = new CElementBlueprint(args[2], Object.ForeColor);

                        CElementManager.Change(from, to);

                        break;
                    }

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

                string s = Lines[Line];

                //Look for any @s (variable sigils) and skip if none were found
                if (s.IndexOf("@") != -1)
                {

                    //Replace "system variables"
                    s = s.Replace("@seek", CUtil.getDirectionString(CGrid.GetDirectionToPoint(Object.Location, CGame.Player.Location, false)));
                    s = s.Replace("@aligned", CUtil.getAligned(Object.Location, CGame.Player.Location));


                    //Replace all variables with their value
                    foreach (KeyValuePair<string, string> p in Vars)
                    {
                        s = s.Replace("@" + p.Key, p.Value);
                    }

                    //Replace any other variable names
                    s = CUtil.stripVariables(s);

                }

                switch (s.Substring(0,1))
                {
                    case "#": //Command line
                        waitAfter = Execute(s.Substring(1));
                        break;

                    case ":": //Label (for GOTO and "events")
                        break;

                    default:  //Defaulting to displaying the text
                        CDrawing.DisplayText(s, 2000);
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
