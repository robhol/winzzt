/*

CElement.cs

Base class for all elements. Contains common methods and members.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CElement
    {

        public Point Location;
        public int Char;                        //Char: same IDs as in ZZT.
        public Color ForeColor;                 //Main color of the element
        public Color BackColor;
        public bool Block;                      //If true, try() returns false
        public bool BlockBullets = true;        //If false, bullets can pass regardless of Block value.
        public int Ordering;                    //Should be unique.
        public bool CanBeSteppedOn = false;     
        public bool Pushable = false;           //Boulders, sliders, etc

        public string Type = "undefined";       //Element type (water, wall, lion, etc.)

        /// <summary>Puts the element on the grid and registers it with the EM.</summary>
        public void Initialize(int x, int y)
        {
            //Add to grid
            Location = new Point(x, y);
            CTile t = CGrid.Get(Location);
            t.Contents.Add(this);

            //Register in Element Manager
            CElementManager.Register(this);

            //Update type string. Remove WinZZT.C (redundant.) Also lowercase.
            Type = this.GetType().ToString().Replace("WinZZT.C","").ToLower();

        }

        /// <summary>Convenience function to init. all properties. Calls Initialize().</summary>
        public void InitProps(int x, int y, int c, Color foreground, Color background, bool block, int ordering)
        {
            Initialize(x, y);
            Char = c;
            ForeColor = foreground;
            BackColor = background;
            Block = block;
            Ordering = ordering;
        }

        /// <summary>Removes the element from the grid.</summary>
        public virtual void Die()
        {
            CTile t = CGrid.Get(Location);

            if (t.Contents.Contains(this))
                t.Contents.Remove(this);          
        }

        /// <summary>
        /// Moves from A to B, no questions asked.
        /// If invalid, will spawn exceptions.
        /// </summary>
        public void Move(Point from, Point to)
        {
            //Get old tile.
            CTile f = CGrid.Get(from);
            
            //Remove from old tile
            if (f.Contents.Contains(this))    
                f.Contents.Remove(this);
            
            //Get new tile
            CTile t = CGrid.Get(to);
            
            //Add to tile and update Location
            t.Contents.Add(this);
            this.Location = to;
        }

        /// <summary>
        /// Tries to move in a direction.
        /// </summary>
        /// <param name="d">Direction in which to move.</param>
        /// <param name="move">If false, it's a "theoretical move". It will return true or false, but not actually move.</param>
        /// <param name="push">Whether or not to push pushable elements while moving.</param>
        /// <returns></returns>
        public bool Try(EDirection d, bool move, bool push)
        {

            //Get destination coords
            Point p = CGrid.GetInDirection(Location, d);

            if (!CGrid.IsValid(p))
                return false;
            
            CTile t = CGrid.Get(p);

            if (!t.IsBlocked())
            {
                CElement c = t.GetTopmost();

                /*if (c != null && c.CanBeSteppedOn) //Trigger SteppedOn for topmost object
                    c.SteppedOn(this);*/

                if (move)
                    Move(Location, p); //Do the actual move unless "hypothetical"

                return true;
            }

            else if (t.IsBlocked() && t.GetTopmost() != null)
            {   //Blocked, check for pushable or "steppable" and act accordingly..
                CElement e = t.GetTopmost();

                if (e.Pushable && push)
                {   //Push. If successful, move after.
                    if (e.PushTowards(d))
                        Move(Location, p);
                }

                if (e.CanBeSteppedOn)
                {   //Let whatever we're stepping on know, and move if OK.
                    if (e.SteppedOn(this))
                        Move(Location, p);
                }

            }

            else
            {   //No go, completely blocked. Call Touch() for element, if any.
                CElement c = t.GetTopmost();
                
                if (c != null)
                    c.Touch(this);

            }

            return false;

        }

        /// <summary>
        /// Tries going in given direction, pushing objects in front.
        /// </summary>
        /// <param name="d">Direction in which to move.</param>
        /// <returns>Whether move was successful.</returns>
        public virtual bool PushTowards(EDirection d)
        {
            //Get coords
            Point p = CGrid.GetInDirection(Location, d);

            //Cancel if invalid and return false
            if (!CGrid.IsValid(p))
                return false;

            CTile t = CGrid.Get(p);

            if (!t.IsBlocked())
            {
                Move(Location, p);
                return true;
            }
            else if (t.IsBlocked() && t.GetTopmost() != null && t.GetTopmost().Pushable)
            {   //Blocked, but pushable. Try to push the other object.
                if (t.GetTopmost().PushTowards(d))
                {   //If it went OK, we can move after.
                    Move(Location, p);
                    return true;
                }
            }

            //No openings.
            return false;

        }

        /// <summary>
        /// Moves towards a certain point.
        /// </summary>
        /// <param name="p">Point to move towards</param>
        public bool Seek(Point p)
        {

            EDirection x = CGrid.GetDirectionToPoint(Location, p, false);
            if (CGrid.Get(CGrid.GetInDirection(Location,x)).IsBlocked())
                x = CGrid.GetDirectionToPoint(Location, p, true);

            return Try(x,true,true);

        }

        /// <summary>
        /// (Overload) Moves towards a certain element.
        /// </summary>
        /// <param name="e">Element to move towards</param>
        public bool Seek(CElement e)
        {
            return Seek(e.Location);
        }

        /// <summary>
        /// Whether the element is touching/adjacent to a given element.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool IsTouching(CElement e)
        {
            for (int dir = 0; dir < 4; dir++)
                if (CGrid.Get(CGrid.GetInDirection(Location, (EDirection)dir)).Contents.Contains(e))
                    return true;

            return false;
        }


        /// <summary>
        /// Tries to shoot in a given direction.
        /// </summary>
        /// <param name="d">Direction in which to shoot</param>
        /// <returns></returns>
        public bool Shoot(EDirection d)
        {

            Point p = CGrid.GetInDirection(Location, d);

            if (CGrid.IsValid(p) && !CGrid.Get(p).IsBlocked() || (CGrid.Get(p).GetTopmost() != null && !CGrid.Get(p).GetTopmost().BlockBullets))
            {
                new CBullet(p.X, p.Y, 100, d, this);
                return true;
            }
            else
                return false;

        }

        /// <summary>
        /// Checks if the element is adjacent to an element with the given type, in the given direction.
        /// </summary>
        /// <param name="t">Type to look for</param>
        /// <param name="d">Direction in which to look</param>
        /// <returns></returns>
        public bool HasElementInDirection(string t, EDirection d)
        {
            Point p = CGrid.GetInDirection(Location, d);

            if (CGrid.IsValid(p))
                return CGrid.Get(p).ContainsType(t);

            return false;

        }

        /// <summary>
        /// Function triggered when an element is shot.
        /// </summary>
        /// <param name="responsible">Source of the bullet</param>
        /// <param name="bullet">The bullet object</param>
        public virtual void Shot(CElement responsible, CBullet bullet)
        {

        }

        /// <summary>
        /// Triggered when an element is touching this one.
        /// </summary>
        /// <param name="responsible">The "toucher"</param>
        public virtual void Touch(CElement responsible)
        {

        }

        /// <summary>
        /// Function triggered when an element is stepped on. 
        /// In order to block, return false in overriding function.
        /// </summary>
        /// <param name="responsible">The element that stepped on this one</param>
        public virtual bool SteppedOn(CElement responsible)
        {
            return true;
        }

    }



}
