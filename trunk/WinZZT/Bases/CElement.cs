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
        public int Char;
        public Color ForeColor;
        public Color BackColor;
        public bool Block;
        public bool BlockBullets = true;
        public int Ordering;
        public bool CanBeSteppedOn = false;
        public bool Pushable = false;

        public bool IsPlayer = false;

        public void Initialize(int x, int y)
        {
            //Add to grid
            Location = new Point(x, y);
            CTile t = CGrid.Get(Location);
            t.Contents.Add(this);

            //Register in Element Manager
            CElementManager.Register(this);

        }

        public void InitProps(int x, int y, int c, Color foreground, Color background, bool block, int ordering)
        {
            Initialize(x, y);
            Char = c;
            ForeColor = foreground;
            BackColor = background;
            Block = block;
            Ordering = ordering;
        }

        public virtual void Die()
        {
            CTile t = CGrid.Get(Location);

            if (t.Contents.Contains(this))
                t.Contents.Remove(this);          

        }

        public void Move(Point from, Point to)
        {
            CTile f = CGrid.Get(from);
            
            if (f.Contents.Contains(this))    
                f.Contents.Remove(this);
            
            CTile t = CGrid.Get(to);
            
            t.Contents.Add(this);
            
            this.Location = to;
        }

        public bool Try(EDirection d, bool move, bool push)
        {

            Point p = CGrid.GetInDirection(Location, d);

            if (!CGrid.IsValid(p))
                return false;
            
            CTile t = CGrid.Get(p);

            if (!t.IsBlocked())
            {
                CElement c = t.GetTopmost();

                if (c != null && c.CanBeSteppedOn)
                    c.SteppedOn(this);

                if (move)
                    Move(Location, p);

                return true;
            }
            else if (t.IsBlocked() && push && t.GetTopmost() != null && t.GetTopmost().Pushable)
            {
                if (t.GetTopmost().PushTowards(d))
                    Move(Location,p);
            }
            else
            {
                CElement c = t.GetTopmost();
                
                if (c != null)
                    c.Touch();

            }

            return false;


        }

        public virtual bool PushTowards(EDirection d)
        {

            Point p = CGrid.GetInDirection(Location, d);

            if (!CGrid.IsValid(p))
                return false;

            CTile t = CGrid.Get(p);

            if (!t.IsBlocked())
            {
                Move(Location, p);
                return true;
            }
            else if (t.IsBlocked() && t.GetTopmost() != null && t.GetTopmost().Pushable)
            {
                if (t.GetTopmost().PushTowards(d))
                {
                    Move(Location, p);
                    return true;
                }
            }
            

            return false;

        }

        public void Seek(Point p)
        {

            EDirection x = CGrid.GetDirectionToPoint(Location, p, false);
            if (CGrid.Get(CGrid.GetInDirection(Location,x)).IsBlocked())
                x = CGrid.GetDirectionToPoint(Location, p, true);

            Try(x,true,true);

        }

        public void Seek(CElement e)
        {
            Seek(e.Location);
        }

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

        public virtual void Shot(CElement responsible, CBullet bullet)
        {

        }

        public virtual void Touch()
        {

        }

        public virtual void SteppedOn(CElement responsible)
        {

        }

    }



}
