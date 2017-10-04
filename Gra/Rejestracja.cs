using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; //Directory.Exists

namespace Gra
{
    public partial class Rejestracja : Form
    {
        public string nick;

        public Rejestracja()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Żadne pole nie może pozostać puste!", "Wypełnij pola!");
            }
            else
            {
                if (Directory.Exists("C:\\Odkrywanka-dane\\" + textBox1.Text))
                {
                    MessageBox.Show("Konto z podanym nickiem już istnieje!", "Nick zajęty!");
                }          
                else if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("Błędnie powtórzone hasło!", "Błąd");
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                {
                    System.IO.Directory.CreateDirectory("C:\\Odkrywanka-dane\\" + textBox1.Text);
                    var sw = new System.IO.StreamWriter("C:\\Odkrywanka-dane\\" + textBox1.Text + "\\gracz.ID");
                    sw.Write(textBox1.Text + "\n" + textBox2.Text);
                    sw.Close();

                    MessageBox.Show("Rejestracja zakończona sukcesem!\nMożesz się teraz zalogować za pomocą utworzonego właśnie konta.");
                    this.Hide();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
            }  
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back &&
            !char.IsSeparator(e.KeyChar) &&
            !char.IsLetter(e.KeyChar) &&
            !char.IsDigit(e.KeyChar);
        }
    }
}
