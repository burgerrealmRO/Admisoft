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
using System.Diagnostics;
namespace Admisoft
{
    public partial class Navigare : Form
    {
        internal static string user = null;
        int[] frecv = new int[1001];
        string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
        SQLiteConnection conn;
        public Navigare()
        {
            InitializeComponent();
        }

        private void Navigare_Load(object sender, EventArgs e)
        {
            #region minitestconexiune
            /*
            var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            var conn = new SQLiteConnection(connstring);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Useri";
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    MessageBox.Show(rdr["Email"].ToString());
                }
            }
            finally
            {
                conn.Close();
            }
            */
            #endregion  
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            conn = new SQLiteConnection(connstring);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT Email FROM Useri WHERE Email=@email)";
            cmd.Parameters.AddWithValue("@email", textBox1.Text);
            SQLiteCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT EXISTS (SELECT Parola FROM Useri WHERE Parola=@parola)";
            cmd2.Parameters.AddWithValue("@parola", textBox2.Text);
            try
            {
                conn.Open();
                int statusemail = Convert.ToInt32(cmd.ExecuteScalar());
                if(statusemail==1)
                {
                    int statusparola = Convert.ToInt32(cmd2.ExecuteScalar());
                    if(statusparola==1)
                    {
                        MessageBox.Show("Acces garantat!");
                        user = textBox1.Text;
                        textBox1.Clear(); textBox2.Clear();
                        Form frm = new Meniu();
                        frm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Parola invalida!");
                        textBox2.Clear();
                    }
                   
                }
                else
                {
                    MessageBox.Show("Email invalid!");
                    textBox1.Clear();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnRegisterIN_Click(object sender, EventArgs e)
        {
            Form frm = new Inregistrare();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new Contact();
            frm.Show();
        }
    }
}
