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
    public partial class Meniu : Form
    {
        int max;
        internal static int idc;
        internal static int aux = 0;
        public Meniu()
        {
            InitializeComponent();
        }

        private void Meniu_Load(object sender, EventArgs e)
        {
            idc = 0;
            label2.Text = Navigare.user;
            #region fillDB
            /*
            string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connstring);
            SQLiteCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = "INSERT INTO Test (IDC,Tip,Enunt,Raspuns1,Raspuns2,Raspuns3,Raspuns4,RaspunsC) VALUES (@IDC,@tip,@enunt,@r1,@r2,@r3,@r4,@rc)";
            cmd1.Parameters.AddWithValue("@IDC", 0);
            cmd1.Parameters.AddWithValue("@tip", 0);
            cmd1.Parameters.AddWithValue("@enunt", "");
            cmd1.Parameters.AddWithValue("@r1", "");
            cmd1.Parameters.AddWithValue("@r2", "");
            cmd1.Parameters.AddWithValue("@r3", "");
            cmd1.Parameters.AddWithValue("@r4", "");
            cmd1.Parameters.AddWithValue("@rc", "");
            try
            {
                conn.Open();
                string[] allLines = File.ReadAllLines(Directory.GetCurrentDirectory().Trim() + "\\fillDB.txt");
                for (int i = 0; i < allLines.Length; i++)
                {
                    string[] items = allLines[i].Split(';');
                    cmd1.Parameters["@IDC"].Value = Convert.ToInt32(items[0]);
                    cmd1.Parameters["@tip"].Value = Convert.ToInt32(items[1]);
                    cmd1.Parameters["@enunt"].Value = items[2];
                    cmd1.Parameters["@r1"].Value = items[3];
                    cmd1.Parameters["@r2"].Value = items[4];
                    cmd1.Parameters["@r3"].Value = items[5];
                    cmd1.Parameters["@r4"].Value = items[6];
                    cmd1.Parameters["@rc"].Value = items[7];
                    cmd1.ExecuteNonQuery();
                }
                //MessageBox.Show("Am reusit sefule!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            */
            #endregion
        }

        private void btnStatistici_Click(object sender, EventArgs e)
        {
            Form frm = new Statistici();
            frm.Show();
        }

        private void btnChestionar_Click(object sender, EventArgs e)
        {
            var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            var conn = new SQLiteConnection(connstring);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(IDC) FROM Test";
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    max = Convert.ToInt32(rdr["MAX(IDC)"].ToString());
                }
            }
            finally
            {
                conn.Close();
            }
            if(idc<max)
            {
                idc++;
            }
            Form frm = new Test_Admitere();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }   
        private void Meniu_MouseEnter(object sender, EventArgs e)
        {
            var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            var conn = new SQLiteConnection(connstring);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(Punctaj) FROM Punctaje WHERE User_Email=@email";
            cmd.Parameters.AddWithValue("@email", Navigare.user);
            try
            {
                conn.Open();
                var rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    if(Convert.ToInt32(rdr["COUNT(Punctaj)"].ToString())>=1)
                    {
                        btnStatistici.Visible = true;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
