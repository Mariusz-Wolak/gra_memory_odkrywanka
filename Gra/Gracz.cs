using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gra
{
    class Gracz
    {
        string nick;
        string haslo;
        int punkty;
        int gry;
        int wygrane;
        int tury;

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

        public string Haslo
        {
            get { return haslo; }
            set { haslo = value; }
        }

        public int Punkty
        {
            get { return punkty; }
            set { punkty = value; }
        }

        public int Gry
        {
            get { return gry; }
            set { gry = value; }
        }

        public int Wygrane
        {
            get { return wygrane; }
            set { wygrane = value; }
        }

        public int Tury
        {
            get { return tury; }
            set { tury = value; }
        }

        public Gracz()
        {
            this.nick = "";
            this.haslo = "";
            this.punkty = 0;
            this.gry = 0;
            this.wygrane = 0;
            this.tury = 0;
        }
    }
}
