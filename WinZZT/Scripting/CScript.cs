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
        static public Dictionary<string, object> Vars;

        public CObject Object;
        public EScriptState State = EScriptState.Idle;

        private int Line = 0;
        private string[] Lines;

        public static void Initialize()
        {
            //Initialize global var dictionary
            Vars = new Dictionary<string, object>();

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
                    Object.Try(CUtil.getDirectionFromString(args[1]), true, true);
                    return true;

                case "CHAR": //Changes char
                    Object.Char = int.Parse(args[1]);
                    return false;

                case "GOTO": //Starts executing script at given label
                    this.JumpToLabel(args[1]);
                    return false;

                case "LOCK": //Prevents object from responding to messages
                    Object.Locked = true;
                    return false;

                case "UNLOCK": //Reverses LOCK effect
                    Object.Locked = false;
                    return false;
                    
                case "CYCLE": //Sets execution speed
                    Object.Cycle = int.Parse(args[1]);
                    return false;

                case "DIE": //Removes object
                    Object.Die();
                    return false;

                case "PAUSE": //Does nothing, waits for next cycle/Step
                    return true;

                case "MSG": //Sends a message to an object with the given name
                    CElementManager.SendMessageToObject(args[1], args[2]);
                    return false;

                case "COLOR": //Changes colors...
                    Object.ForeColor = CUtil.getColorFromString(args[1]);
                    if (args.Length > 2)
                        Object.BackColor = CUtil.getColorFromString(args[2]);
                    return false;

                case "BECOME": //Changes the object into whatever

                    CElementBlueprint bpBecome = new CElementBlueprint(args[1], Color.White);

                    Object.Become(bpBecome);

                    return false;

                case "PUT": //Puts an object in whatever direction

                    CElementBlueprint bpPut = new CElementBlueprint(args[2], Color.White);

                    Object.Put(CUtil.getDirectionFromString(args[1]), bpPut);

                    return false;

                case "GIVE":
                    int amt = int.Parse(args[2]);

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

                        int takeamt = int.Parse(args[2]);

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

                    Object.Shoot(CUtil.getDirectionFromString(args[1]));

                    return false;

                case "SET": //Sets a variable
                    {

                        if (!Vars.ContainsKey(args[1]))
                            Vars.Add(args[1], args[2]);
                        else
                            Vars[args[1]] = args[2];

                        break;
                    }

                case "GOTOIF": //Basic IF statement... Validates an expression.
                    {
                        //Input: 
                        //ARG #     1       2     3      4
                        //#GOTOIF label     A  operator  B

                        bool result = false;

                        //Get operator
                        switch (args[3].ToLower())
                        {
                            case "=":
                                result = args[2] == args[4]; break;

                            case ">":
                                result = int.Parse(args[2])  > int.Parse(args[4]); break;

                            case "<":
                                result = int.Parse(args[2]) <  int.Parse(args[4]); break;

                            case "isdefined":
                                result = args[2] != "(undefined)"; break;
                        }

                        //Act accordingly
                        if (result)
                            JumpToLabel(args[1]);

                        break;
                    }

                case "UNSET": //Unsets/deletes a variable
                    {

                        if (Vars.ContainsKey(args[1]))
                            Vars.Remove(args[1]);

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

                    //Replace all variables with their value
                    foreach (KeyValuePair<string, object> p in Vars)
                    {
                        s = s.Replace("@" + p.Key, p.Value.ToString());
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
