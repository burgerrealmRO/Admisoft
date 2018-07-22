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
    public partial class Statistici : Form
    {
        List<string> emailuri;
        string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
        SQLiteConnection conn;
        SQLiteCommand cmd,cmd2,cmd3,cmd4;
        public Statistici()
        {
            InitializeComponent();
            emailuri = new List<string>();
        }

        private void Statistici_Load(object sender, EventArgs e)
        { 
            grafic.ChartAreas["ChartArea1"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
            grafic.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 9.40F, FontStyle.Bold);
            grafic.ChartAreas["ChartArea1"].AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
            grafic.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 9.40F, FontStyle.Bold);
            grafic.Series["Grafic"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            grafic.Series["Grafic"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            label2.Text = Navigare.user;
            conn = new SQLiteConnection(connstring);
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(Punctaj) FROM Punctaje WHERE User_Email=@email";
            cmd.Parameters.AddWithValue("@email", Navigare.user);
            try { conn.Open(); SQLiteDataReader rdr = cmd.ExecuteReader(); while (rdr.Read()) { label4.Text = rdr["COUNT(Punctaj)"].ToString(); } rdr.Close(); } catch(Exception ex) { throw ex; } finally { conn.Close(); }
            cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT SUM(Punctaj)/COUNT(Punctaj) FROM Punctaje WHERE User_Email=@email";
            cmd2.Parameters.AddWithValue("@email", Navigare.user);
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd2.ExecuteReader();
                while (rdr.Read())
                {
                    label7.Text = rdr["SUM(Punctaj)/COUNT(Punctaj)"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            cmd3 = conn.CreateCommand();
            cmd3.CommandText = "SELECT User_Email FROM Punctaje GROUP BY User_Email";
            cmd4 = conn.CreateCommand();
            cmd4.CommandText = "SELECT SUM(Punctaj)/COUNT(Punctaj) FROM Punctaje WHERE User_Email=@email";
            cmd4.Parameters.AddWithValue("@email", "");
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd3.ExecuteReader();
                while(rdr.Read())
                {
                    emailuri.Add(rdr["User_Email"].ToString());
                }
                rdr.Close();
                /*
                foreach(string email in emailuri)
                {
                    MessageBox.Show(email);
                }
                */
                foreach(string email in emailuri)
                {
                    cmd4.Parameters["@email"].Value=email;
                    SQLiteDataReader rdr2 = cmd4.ExecuteReader();    
                    while(rdr2.Read())
                    {
                        grafic.Series["Grafic"].Points.AddXY(email, Convert.ToDouble(rdr2["SUM(Punctaj)/COUNT(Punctaj)"].ToString()));
                    }
                    rdr2.Close();
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
    }
}
