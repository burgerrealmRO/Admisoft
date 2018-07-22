using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;
namespace Admisoft
{
    public partial class Rezultat : Form
    {
        public Rezultat()
        {
            InitializeComponent();
        }
        private void Rezultat_Load(object sender, EventArgs e)
        {
            label4.Text = Test_Admitere.score.ToString();
            var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            var conn = new SQLiteConnection(connstring);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT NrIntrb AS [Item],Enunt AS [Enunt],RC AS [Raspuns corect] FROM Stats";
            var cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT COUNT(Enunt) FROM Stats";
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd2.ExecuteReader();
                while(rdr.Read())
                {
                    label6.Text = rdr["COUNT(Enunt)"].ToString();
                }
                label5.Text = (30 - Convert.ToInt32(label6.Text)).ToString();
                rdr.Close();
                SQLiteDataReader rdr2 = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr2);
                this.dataGridView1.DataSource = dt;
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

        private void Rezultat_FormClosing(object sender, FormClosingEventArgs e)
        {
            var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            var conn = new SQLiteConnection(connstring);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Stats";
            try { conn.Open(); cmd.ExecuteNonQuery(); } catch(Exception ex) { throw ex; } finally { conn.Close(); }
        }
    }
}
