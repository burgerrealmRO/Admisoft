using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Configuration;
using System.IO;
namespace Admisoft
{
    public partial class Inregistrare : Form
    {
        string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
        SQLiteConnection conn;
        public Inregistrare()
        {
            InitializeComponent();
        }

        private async void btnRegisterOUT_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!="")
            {
                if (isValidEmail(textBox1.Text))
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        conn = new SQLiteConnection(connstring);
                        SQLiteCommand cmdaux = conn.CreateCommand();
                        SQLiteCommand cmdfill = conn.CreateCommand();
                        cmdaux.CommandText = "SELECT EXISTS(SELECT Email FROM Useri WHERE Email=@email)";
                        cmdfill.CommandText = "INSERT INTO Useri (Email,Parola,Drepturi) VALUES (@email,@parola,2)";
                        cmdaux.Parameters.AddWithValue("@email", textBox1.Text);
                        cmdfill.Parameters.AddWithValue("@email", textBox1.Text);
                        cmdfill.Parameters.AddWithValue("@parola", textBox2.Text);
                        try
                        {
                            conn.Open();
                            int status1 = Convert.ToInt32(cmdaux.ExecuteScalar());
                            if (status1 == 0)
                            {
                                cmdfill.ExecuteNonQuery();
                                MessageBox.Show("Am reusit!"); // break;
                                pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory().Trim() + "\\succes.jpg");
                                await Task.Delay(2000);
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Exista deja un utilizator cu acest email!"); textBox1.Clear();
                                pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory().Trim() + "\\denied.jpg");
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Parolele nu se potrivesc"); textBox2.Clear(); textBox3.Clear();
                        pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory().Trim() + "\\denied.jpg");
                    }
                }
                else
                {
                    MessageBox.Show("Email-ul nu este valid!"); textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Toate spatiile trebuie completate!");
                pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory().Trim() + "\\denied.jpg");
            }
        }
        private bool isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
