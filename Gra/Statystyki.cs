using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gra
{
    public partial class Statystyki : Form
    {
        public Statystyki()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 11; i < 18; i++)
            {
                Control label = this.Controls["Label" + i];
                if (label != null)
                    label.Visible = false;
            }

            if ((string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text.Length > 0) || textBox1.Text == "")
            {
                MessageBox.Show("Podaj nick!");
                textBox1.Text = "";
            }
            else
                try
                {
                    var sr = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + textBox1.Text + "\\gracz.ID");
                    sr.Close();

                    try
                    {
                        var sr2 = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + textBox1.Text + "\\solo.ID");

                        int gier, tur;
                        double srednio_tur, celnosc;

                        gier = Convert.ToInt16(sr2.ReadLine());
                        tur = Convert.ToInt16(sr2.ReadLine());
                        sr2.Close();

                        srednio_tur = Convert.ToDouble(tur) / Convert.ToDouble(gier);
                        celnosc = 100 * 12 / srednio_tur;

                        label11.Text = Convert.ToString(gier);
                        label11.Visible = true;

                        label12.Text = Convert.ToString(Math.Round(srednio_tur, 2, MidpointRounding.AwayFromZero));
                        label12.Visible = true;

                        label13.Text = Convert.ToString(Math.Round(celnosc, 2, MidpointRounding.AwayFromZero)) + "%";
                        label13.Visible = true;
                    }
                    catch (System.IO.FileNotFoundException)
                    {

                    }

                    try
                    {
                        var sr3 = new System.IO.StreamReader("C:\\Odkrywanka-dane\\" + textBox1.Text + "\\duo.ID");

                        int gier, wygranych;
                        double powodzenie;

                        gier = Convert.ToInt16(sr3.ReadLine());
                        wygranych = Convert.ToInt16(sr3.ReadLine());
                        sr3.Close();

                        try
                        {
                            powodzenie = 100 * wygranych / gier;
                        }
                        catch (System.DivideByZeroException)
                        {
                            powodzenie = 0;
                        }

                        label14.Text = Convert.ToString(gier);
                        label14.Visible = true;

                        label15.Text = Convert.ToString(wygranych);
                        label15.Visible = true;

                        label16.Text = Convert.ToString(Math.Round(powodzenie, 1, MidpointRounding.AwayFromZero)) + "%";
                        label16.Visible = true;
                    }
                    catch (System.IO.FileNotFoundException)
                    {

                    }

                    if (label11.Visible || label14.Visible)
                    {
                        label17.Text = textBox1.Text;
                        label17.Visible = true;
                    }
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    MessageBox.Show("Konto nie istnieje!");
                    textBox1.Text = "";
                }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back &&
            !char.IsSeparator(e.KeyChar) && 
            !char.IsLetter(e.KeyChar) && 
            !char.IsDigit(e.KeyChar);
        }
    }
}
