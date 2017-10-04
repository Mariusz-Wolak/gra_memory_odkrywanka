using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using System.IO;
using System.Reflection;

namespace Gra
{
    public partial class Form1 : Form
    {
        private SoundPlayer card;
        private SoundPlayer win;
        private SoundPlayer rozdanie;
        private SoundPlayer ping;

        Random numer = new Random(); 
        bool losowanie_ok; 
        int wylosowana_liczba; 
        int powtorka = 0; // wykorzystywana przy losowaniu 
        int ktory_zaczyna; 
        bool gramy = false; 
        bool gramy2; 

        int liczba_pol_latwy = 12;
        int liczba_pol_trudny = 24;
        int laczna_liczba_pol;
        int aktualna_liczba_pol; // juz konkretnego poziomu, przypisywana przy losowaniu

        int liczba_obrazow;
        Pole[] pole = new Pole[36]; 
        Gracz gracz1 = new Gracz(); 
        Gracz gracz2 = new Gracz(); 
        int[] numer_pola = new int[36];

        bool jeden_gracz = false;
        char kto;

        int klik = 0;
        bool[] aktywowano = new bool[36];
        int aktywne_pole;

        int tura = 1;

        bool tryb_latwy = false;


        void ukryj_pozostaly()
        {
            int param1, param2;

            if (tryb_latwy)
            {
                param1 = 0;
                param2 = liczba_pol_latwy;
            }
            else
            {
                param1 = liczba_pol_latwy;
                param2 = laczna_liczba_pol;
            }

            for (int i = param1; i < param2; i++)
            {
                if(aktywne_pole == i)
                {
                    int nr_obrazka = aktywne_pole + 1;
                    PictureBox mojPicBox = (PictureBox)this.Controls["pictureBox" + nr_obrazka];

                    if (mojPicBox != null)
                    {
                        mojPicBox.Image = Properties.Resources.pytajnik;
                        mojPicBox.Enabled = true;
                    }    
                }
            }
        }

        void czy_koniec()
        {
            int pozostalych_obrazkow = 0;

            for (int i = 1; i <= 36; i++)
            {
                PictureBox mojPicBox = (PictureBox)this.Controls["pictureBox" + i];

                if (mojPicBox != null && mojPicBox.Enabled == true)
                {
                    pozostalych_obrazkow++;
                    break;
                }
            }

            if (pozostalych_obrazkow == 0)
            {
                win.Play();

                if (jeden_gracz) MessageBox.Show(">" + gracz1.Nick + "<! Wygrywasz w " + tura + " turze!", "Wygrana!");
                else
                {
                    if (gracz1.Punkty > gracz2.Punkty) MessageBox.Show(">" + gracz1.Nick + "<! Wygrywasz w " + tura + " turze, z ilością punktów: " + gracz1.Punkty, "Wygrana!");
                    else if (gracz1.Punkty < gracz2.Punkty) MessageBox.Show(">" + gracz2.Nick + "<! Wygrywasz w " + tura + " turze, z ilością punktów: " + gracz2.Punkty, "Wygrana!");
                    else if (gracz1.Punkty == gracz2.Punkty) MessageBox.Show("Remis po " + tura + " turach!", "Remis");
                }

                if (tryb_latwy == false) zapisz_statystyki();
                od_nowa();
            }
        }



        void dezaktywacja()
        {
            for (int i = 0; i < 36; i++)
            {
                aktywowano[i] = false;
            }
        }


        void zapisz_statystyki()
        {
                if (jeden_gracz)
                {
                    try
                    {
                        var sr = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\solo.ID");
                        gracz1.Gry = Convert.ToInt16(sr.ReadLine());
                        gracz1.Tury = Convert.ToInt16(sr.ReadLine());
                        sr.Close();
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        var sw = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\solo.ID");
                        gracz1.Gry = 0;
                        gracz1.Tury = 0;
                        sw.Close();
                    }
                    finally
                    {
                        var sw = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\solo.ID");
                        sw.Write(Convert.ToString(gracz1.Gry + 1) + "\n" + Convert.ToString(gracz1.Tury + tura));
                        sw.Close();
                    }
                }
                else
                {
                    try
                    {
                        var sr = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\duo.ID");
                        gracz1.Gry = Convert.ToInt16(sr.ReadLine());
                        gracz1.Wygrane = Convert.ToInt16(sr.ReadLine());
                        sr.Close();
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        var sw = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\duo.ID");
                        gracz1.Gry = 0;
                        gracz1.Wygrane = 0;
                        sw.Close();
                    }
                    finally
                    {
                        var sw = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz1.Nick + "\\duo.ID");
                        if (gracz1.Punkty > gracz2.Punkty)
                        {
                            sw.Write(Convert.ToString(gracz1.Gry + 1) + "\n" + Convert.ToString(gracz1.Wygrane + 1));
                            sw.Close();
                        }
                        else if (gracz1.Punkty < gracz2.Punkty)
                        {
                            sw.Write(Convert.ToString(gracz1.Gry + 1));
                            sw.Close();
                        }
                    }

                    try
                    {
                        var sr2 = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + gracz2.Nick + "\\duo.ID");
                        gracz2.Gry = Convert.ToInt16(sr2.ReadLine());
                        gracz2.Wygrane = Convert.ToInt16(sr2.ReadLine());
                        sr2.Close();
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        var sw2 = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz2.Nick + "\\duo.ID");
                        gracz2.Gry = 0;
                        gracz2.Wygrane = 0;
                        sw2.Close();
                    }
                    finally
                    {
                        var sw2 = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + gracz2.Nick + "\\duo.ID");
                        if (gracz2.Punkty > gracz1.Punkty)
                        {
                            sw2.Write(Convert.ToString(gracz2.Gry + 1) + "\n" + Convert.ToString(gracz2.Wygrane + 1));
                            sw2.Close();
                        }
                        else if (gracz2.Punkty < gracz1.Punkty)
                        {
                            sw2.Write(Convert.ToString(gracz2.Gry + 1));
                            sw2.Close();
                        }
                    }
                }
        }

        void od_nowa()
        {
            wyswietlanyNick.Visible = false;       
            punkty.Visible = false;
            wyswietlanyNick2.Visible = false;
            punkty2.Visible = false;
            tury.Visible = false;
            zrodlo.Visible = false;

            wyswietlanyNick.Enabled = false;
            punkty.Enabled = false;
            wyswietlanyNick2.Enabled = false;
            punkty2.Enabled = false;

            
            for (int i = 1; i <= 36; i++)
            {
                PictureBox mojPicBox = (PictureBox)this.Controls["pictureBox" + i];
                      
                if (mojPicBox != null)
                {
                    mojPicBox.Visible = false;
                    mojPicBox.Image = Properties.Resources.pytajnik;
                    mojPicBox.Enabled = false;
                }
            }

            jeden_gracz = false;
            klik = 0;
            dezaktywacja();
            tura = 0; // bo sie zwieksza o 1 kilka linijek poniżej wywołania tej funkcji, zawsze
            tryb_latwy = false;

            nickTextBox.Text = gracz1.Nick = "";
            nick2TextBox.Text = gracz2.Nick = "";
            punkty.Text = "PUNKTY: " + Convert.ToString(gracz1.Punkty = 0);
            punkty2.Text = "PUNKTY: " + Convert.ToString(gracz2.Punkty = 0);
            hasloTextBox.Text = gracz1.Haslo = "";
            haslo2TextBox.Text = gracz2.Haslo = "";
            gracz1.Gry = 0;
            gracz2.Gry = 0;
            gracz1.Wygrane = 0;
            gracz2.Wygrane = 0;
            gracz1.Tury = 0;
            gracz2.Tury = 0;

            tytul.Visible = true;
            menu.Visible = true;
            opcja1.Text = "GRAJ";
            opcja2.Text = "ZASADY";
            opcja1.Visible = true;
            opcja2.Visible = true;
            wstecz.Text = "WYJŚCIE";
            wstecz.Visible = true;
            gramy = false;
            gramy2 = false;
            staty.Visible = true;    
        }

        void losowanie()
        {
            int param1, param2;

            if (tryb_latwy)
            {
                param1 = 0;
                param2 = liczba_pol_latwy;
                aktualna_liczba_pol = liczba_pol_latwy;
            }
            else
            {
                param1 = liczba_pol_latwy;
                param2 = laczna_liczba_pol;
                aktualna_liczba_pol = liczba_pol_trudny;
            }
            
            liczba_obrazow = aktualna_liczba_pol / 2;

            for (int i = param1; i < param2; i++)
            {
                do
                {
                    wylosowana_liczba = numer.Next(1, liczba_obrazow + 1);
                    losowanie_ok = true;

                    for (int j = param1; j < i; j++)
                    {
                        if (wylosowana_liczba == pole[j].Nr) powtorka++;
                        if (powtorka == 2) losowanie_ok = false;
                    }

                    if (losowanie_ok == true)
                    {
                        pole[i] = new Pole();
                        pole[i].Nr = wylosowana_liczba;
                        numer_pola[i] = wylosowana_liczba;
                    }

                    powtorka = 0;

                } while (losowanie_ok != true);
            }
        }

        void pokazObrazek(object sender)
        {
            PictureBox mojPicBox = (PictureBox)sender;
            string nazwaPicBoxa = mojPicBox.Name.ToString();
            int nrPicBoxa = Convert.ToInt32(nazwaPicBoxa.Substring(10));

            card.Play();
            klik++;
            aktywowano[nrPicBoxa - 1] = true;
            mojPicBox.Enabled = false;

            pole[nrPicBoxa - 1] = new Pole();
            pole[nrPicBoxa - 1].Nr = numer_pola[nrPicBoxa - 1];
            
            for (int i=1; i<=liczba_obrazow; i++)
            {
                if(pole[nrPicBoxa - 1].Nr == i)
                {
                    object obrazek = Properties.Resources.ResourceManager.GetObject("obrazek" + i);
                    mojPicBox.Image = (Image)obrazek;
                    break;
                }
            }

            if (klik == 2)
            {
                for (int i = 0; i < laczna_liczba_pol; i++)
                {
                    if (aktywowano[i] && i != (nrPicBoxa-1))
                    {
                        aktywne_pole = i;
                        break;
                    }
                }

                if (pole[nrPicBoxa - 1].Nr != pole[aktywne_pole].Nr) //jesli nie dopasowano pary
                {
                    if (jeden_gracz == false)
                    {
                        if (kto == '1')
                        {
                            MessageBox.Show("Nie do pary!\nTeraz odkrywa >" + gracz2.Nick + "<", "A to pech...");
                            kto = '2';
                            wyswietlanyNick.Enabled = false;
                            punkty.Enabled = false;
                            wyswietlanyNick2.Enabled = true;
                            punkty2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Nie do pary!\nTeraz odkrywa >" + gracz1.Nick + "<", "A to pech...");
                            kto = '1';
                            wyswietlanyNick.Enabled = true;
                            punkty.Enabled = true;
                            wyswietlanyNick2.Enabled = false;
                            punkty2.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nie do pary!", "A to pech...");
                    }

                    mojPicBox.Image = Properties.Resources.pytajnik;
                    mojPicBox.Enabled = true;

                    ukryj_pozostaly();
                }
                else //jesli dopasowano pare
                {
                    if (jeden_gracz == false)
                    {
                        if (kto == '1')
                        {
                            gracz1.Punkty++;
                            punkty.Text = "PUNKTY: " + Convert.ToString(gracz1.Punkty);
                            MessageBox.Show("Dobrze dobrana para!", "Gratulacje!");
                        }
                        else
                        {
                            gracz2.Punkty++;
                            punkty2.Text = "PUNKTY: " + Convert.ToString(gracz2.Punkty);
                            MessageBox.Show("Dobrze dobrana para!", "Gratulacje!");
                        }
                    }
                    else
                    {
                        gracz1.Punkty++;
                        punkty.Text = "PUNKTY: " + Convert.ToString(gracz1.Punkty);
                        MessageBox.Show("Dobrze dobrana para!", "Gratulacje!");
                        
                    }

                    czy_koniec();
                }

                tura++;
                tury.Text = "TURA " + tura + ".";
                klik = 0;
                dezaktywacja();
            }
        }

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        public Form1()
        {
            InitializeComponent();

            GoFullscreen(true);

            card = new SoundPlayer("card-flip.wav");
            win = new SoundPlayer("victory.wav");
            rozdanie = new SoundPlayer("dealing_card.wav");
            ping = new SoundPlayer("metal-ping-1.wav");

            laczna_liczba_pol = liczba_pol_latwy + liczba_pol_trudny;

            #region transparent

            tytul.Parent = tlo;
            tytul.BackColor = Color.Transparent;

            menu.Parent = tlo;
            menu.BackColor = Color.Transparent;

            opcja1.Parent = tlo;
            opcja1.BackColor = Color.Transparent;

            opcja2.Parent = tlo;
            opcja2.BackColor = Color.Transparent;

            wstecz.Parent = tlo;
            wstecz.BackColor = Color.Transparent;

            nickLabel.Parent = tlo;
            nickLabel.BackColor = Color.Transparent;

            nick2Label.Parent = tlo;
            nick2Label.BackColor = Color.Transparent;

            tury.Parent = tlo;
            tury.BackColor = Color.Transparent;

            zrodlo.Parent = tlo;
            zrodlo.BackColor = Color.Transparent;

            wyswietlanyNick.Parent = tlo;
            wyswietlanyNick.BackColor = Color.Transparent;

            punkty.Parent = tlo;
            punkty.BackColor = Color.Transparent;

            wyswietlanyNick2.Parent = tlo;
            wyswietlanyNick2.BackColor = Color.Transparent;

            punkty2.Parent = tlo;
            punkty2.BackColor = Color.Transparent;

            infoLabel.Parent = tlo;
            infoLabel.BackColor = Color.Transparent;

            start.Parent = tlo;
            start.BackColor = Color.Transparent;

            rejestracja.Parent = tlo;
            rejestracja.BackColor = Color.Transparent;

            hasloLabel.Parent = tlo;
            hasloLabel.BackColor = Color.Transparent;

            haslo2Label.Parent = tlo;
            haslo2Label.BackColor = Color.Transparent;

            staty.Parent = tlo;
            staty.BackColor = Color.Transparent;

            #endregion
        }

        private void opcja1_Click(object sender, EventArgs e)
        {
            ping.Play();
            if (opcja1.Text == "GRAJ")
            {
                wstecz.Text = "WSTECZ";

                opcja1.Text = "JEDEN GRACZ";
                opcja2.Text = "DWÓCH GRACZY";

                staty.Visible = false;
            }
            else if (opcja1.Text == "JEDEN GRACZ")
            {
                wstecz.Visible = true;

                opcja1.Text = "ŁATWY";
                opcja2.Text = "TRUDNY";

                jeden_gracz = true;
            }
            else
            {
                tryb_latwy = true;

                tytul.Visible = false;
                menu.Visible = false;
                opcja1.Visible = false;
                opcja2.Visible = false;

                nickLabel.Visible = true;
                nickTextBox.Visible = true;
                start.Visible = true;

                if (jeden_gracz == false)
                {
                    nick2Label.Visible = true;
                    nick2TextBox.Visible = true;
                }
            }
        }

        private void opcja2_Click(object sender, EventArgs e)
        {
            ping.Play();
            if (opcja2.Text == "ZASADY")
            {
                tytul.Visible = false;
                menu.Visible = false;
                opcja1.Visible = false;
                opcja2.Visible = false;
                staty.Visible = false;

                infoLabel.Visible = true;
                wstecz.Text = "WSTECZ";
            }
            else if (opcja2.Text == "DWÓCH GRACZY")
            {
                wstecz.Visible = true;

                opcja1.Text = "ŁATWY";
                opcja2.Text = "TRUDNY";
            }
            else
            {
                tytul.Visible = false;
                menu.Visible = false;
                opcja1.Visible = false;
                opcja2.Visible = false;

                nickLabel.Visible = true;
                nickTextBox.Visible = true;
                start.Visible = true;
                rejestracja.Visible = true;

                if (jeden_gracz == false)
                {
                    nick2Label.Visible = true;
                    nick2TextBox.Visible = true;

                    if (tryb_latwy == false)
                    {
                        hasloLabel.Visible = true;
                        hasloTextBox.Visible = true;

                        haslo2Label.Visible = true;
                        haslo2TextBox.Visible = true;
                    }
                }
                else
                {
                    if (tryb_latwy == false)
                    {
                        hasloLabel.Visible = true;
                        hasloTextBox.Visible = true;
                    }
                }
            }
        }

        private void wstecz_Click(object sender, EventArgs e)
        {
            ping.Play();

            if (wstecz.Text == "WYJŚCIE")
            {
                Application.Exit();
            }
            else if (infoLabel.Visible == true) // jesli jestesmy w "zasadach"
            {
                tytul.Visible = true;
                menu.Visible = true;
                opcja1.Visible = true;
                opcja2.Visible = true;
                staty.Visible = true;

                infoLabel.Visible = false;
                wstecz.Text = "WYJŚCIE";
            }
            else if (opcja1.Text == "JEDEN GRACZ") // kiedy wcisniemy "graj" i wybieramy tryb single lub multi
            {
                opcja1.Text = "GRAJ";
                opcja2.Text = "ZASADY";

                wstecz.Text = "WYJŚCIE";

                staty.Visible = true;
            }
            else if (opcja1.Visible == true) // kiedy wybieramy poziom trudności
            {
                opcja1.Text = "JEDEN GRACZ";
                opcja2.Text = "DWÓCH GRACZY";

                jeden_gracz = false;
                tryb_latwy = false;
            }
            else // kiedy wybralismy juz tryby i wpisujemy nick;
            {
                tryb_latwy = false;

                tytul.Visible = true;
                menu.Visible = true;
                opcja1.Visible = true;
                opcja2.Visible = true;

                nickLabel.Visible = false;
                nickTextBox.Visible = false;
                start.Visible = false;
                nick2Label.Visible = false;
                nick2TextBox.Visible = false;
                rejestracja.Visible = false;

                nickTextBox.Text = "";
                nick2TextBox.Text = "";
                hasloTextBox.Text = "";
                haslo2TextBox.Text = "";

                hasloLabel.Visible = false;
                hasloTextBox.Visible = false;
                haslo2Label.Visible = false;
                haslo2TextBox.Visible = false;
            }
        }

        void walidacja(KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && 
            !char.IsSeparator(e.KeyChar) && 
            !char.IsLetter(e.KeyChar) && 
            !char.IsDigit(e.KeyChar);
        }

        

        private void start_Click(object sender, EventArgs e)
        {
            bool zly_nick = false;
            gramy2 = true;

            if ( //jesli puste nicki
                ((string.IsNullOrWhiteSpace(nickTextBox.Text) && nickTextBox.Text.Length > 0) ||  nickTextBox.Text == "")
                 || (jeden_gracz == false && ((string.IsNullOrWhiteSpace(nickTextBox.Text) && nickTextBox.Text.Length > 0) || nick2TextBox.Text == ""))
                )
            {
                MessageBox.Show("Proszę podać nick!");
                zly_nick = true;
            }

            else if ((nickTextBox.Text == nick2TextBox.Text) && jeden_gracz == false)
            {
                MessageBox.Show("Proszę podać różne nicki!");
                zly_nick = true;
            }
            else if (tryb_latwy)
            {
                gracz1.Nick = wyswietlanyNick.Text = nickTextBox.Text;
                gracz2.Nick = wyswietlanyNick2.Text = nick2TextBox.Text;

                for (int i=1; i<=liczba_pol_latwy; i++)
                {
                    PictureBox mojPicBox = (PictureBox)this.Controls["pictureBox" + i];
                    if (mojPicBox != null)
                    {
                        mojPicBox.Visible = true;
                        mojPicBox.Enabled = true;
                    }
                } 
            }
            else // tryb trudny
            {
                try
                {
                    var sr = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + nickTextBox.Text + "\\gracz.ID");
                    gracz1.Nick = sr.ReadLine();
                    gracz1.Haslo = sr.ReadLine();
                    sr.Close();

                    if (gracz1.Nick == nickTextBox.Text && gracz1.Haslo == hasloTextBox.Text)
                    {
                        gramy = true;
                    }
                    else
                    {
                        gramy = false;
                        MessageBox.Show("Złe dane");
                    }
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    gramy = false;
                    MessageBox.Show("Złe dane!");
                }

                if (jeden_gracz == false)
                {
                    try
                    {
                        var sr = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + nick2TextBox.Text + "\\gracz.ID");
                        gracz2.Nick = sr.ReadLine();
                        gracz2.Haslo = sr.ReadLine();
                        sr.Close();

                        if (gracz2.Nick != nick2TextBox.Text || gracz2.Haslo != haslo2TextBox.Text)
                        {                        
                            gramy2 = false;
                            MessageBox.Show("Złe dane drugiego gracza!");
                        }
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        gramy2 = false;
                        MessageBox.Show("Złe dane drugiego gracza!");
                    }
                }

                if (gramy && gramy2)
                {
                    gracz1.Nick = wyswietlanyNick.Text = nickTextBox.Text;
                    gracz2.Nick = wyswietlanyNick2.Text = nick2TextBox.Text;

                    for (int i = liczba_pol_latwy + 1; i <= laczna_liczba_pol; i++)
                    {
                        PictureBox mojPicBox = (PictureBox)this.Controls["pictureBox" + i];
                        if (mojPicBox != null)
                        {
                            mojPicBox.Visible = true;
                            mojPicBox.Enabled = true;
                        }
                    }
                }
            }

            losowanie();

            if ((tryb_latwy || (gramy && gramy2)) && zly_nick == false)
            {
                rozdanie.Play();

                nickLabel.Visible = false;
                nickTextBox.Visible = false;
                hasloLabel.Visible = false;
                hasloTextBox.Visible = false;
                nick2Label.Visible = false;
                nick2TextBox.Visible = false;
                haslo2Label.Visible = false;
                haslo2TextBox.Visible = false;
                start.Visible = false;
                tytul.Visible = false;
                wstecz.Visible = false;
                rejestracja.Visible = false;

                tury.Visible = true;
                zrodlo.Visible = true;

                wyswietlanyNick.Visible = true;

                if (jeden_gracz == false)
                {
                    punkty.Visible = true;

                    wyswietlanyNick2.Visible = true;
                    punkty2.Visible = true;

                    ktory_zaczyna = numer.Next(1, 3);

                    if (ktory_zaczyna == 1)
                    {
                        kto = '1';
                        MessageBox.Show("Zaczyna " + gracz1.Nick + "!", "Zaczynamy!");

                        wyswietlanyNick.Enabled = true;
                        punkty.Enabled = true;
                        wyswietlanyNick2.Enabled = false;
                        punkty2.Enabled = false;
                    }
                    else
                    {
                        kto = '2';
                        MessageBox.Show("Zaczyna " + gracz2.Nick + "!", "Zaczynamy!");

                        wyswietlanyNick.Enabled = false;
                        punkty.Enabled = false;
                        wyswietlanyNick2.Enabled = true;
                        punkty2.Enabled = true;
                    }
                }
                else
                {
                    wyswietlanyNick.Enabled = true;
                    punkty.Enabled = true;
                }
            }
        }

        private void rejestracja_Click(object sender, EventArgs e)
        {
            Rejestracja reg = new Rejestracja();
            reg.Show();
        }

        private void staty_Click(object sender, EventArgs e)
        {
            Statystyki st = new Statystyki();
            st.Show();
        }

        private void pole_Click(object sender, EventArgs e)
        {
            pokazObrazek(sender);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            walidacja(e);
        }
    }
}
