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
namespace Admisoft
{
    public partial class Test_Admitere : Form
    {
        int q = 1;
        string ceas = "60:00";
        int minute = 60;
        int secunde = 0;
        bool gata = false; bool abandon;
        internal static int score = 10;
        int myidc;
        public Test_Admitere()
        {
            InitializeComponent();
        }
        private void btnNextQ_Click(object sender, EventArgs e)
        {
#region ScoreManagement
            if (groupBox1.Visible == true)
            {
                if (radioButton1.Checked && radioButton1.Text.Trim() == RaspunsC.Text.Trim())
                {
                    score += 3;
                }
                else if (radioButton2.Checked && radioButton2.Text.Trim() == RaspunsC.Text.Trim())
                {
                    score += 3;
                }
                else if (radioButton3.Checked && radioButton3.Text.Trim() == RaspunsC.Text.Trim())
                {
                    score += 3;
                }
                else if (radioButton4.Checked && radioButton4.Text.Trim() == RaspunsC.Text.Trim())
                {
                    score += 3;
                }
                else
                {
                    var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
                    var conn = new SQLiteConnection(connstring);
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Stats (UserEmail,IDC,NrIntrb,Enunt,RC) VALUES (@email,@idc,@nrintrb,@enunt,@rc)";
                    cmd.Parameters.AddWithValue("@email", Navigare.user);
                    cmd.Parameters.AddWithValue("@idc", Meniu.idc);
                    cmd.Parameters.AddWithValue("@nrintrb", q);
                    cmd.Parameters.AddWithValue("@enunt", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@rc", RaspunsC.Text);
                    try { conn.Open(); cmd.ExecuteNonQuery(); }
                    catch(Exception ex) { throw ex; }
                    finally { conn.Close(); }
                }
            }
            if (textBox2.Visible == true)
            {
                if(textBox2.Text.Trim() == RaspunsC.Text.Trim())
                {
                    score += 3;
                }
                else
                {
                    var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
                    var conn = new SQLiteConnection(connstring);
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Stats (UserEmail,IDC,NrIntrb,Enunt,RC) VALUES (@email,@idc,@nrintrb,@enunt,@rc)";
                    cmd.Parameters.AddWithValue("@email", Navigare.user);
                    cmd.Parameters.AddWithValue("@idc", Meniu.idc);
                    cmd.Parameters.AddWithValue("@nrintrb", q);
                    cmd.Parameters.AddWithValue("@enunt", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@rc", RaspunsC.Text);
                    try { conn.Open(); cmd.ExecuteNonQuery(); }
                    catch (Exception ex) { throw ex; }
                    finally { conn.Close(); }
                }
            }
#endregion
            if (q <= 29)
            {      
                q++; label2.Text = q.ToString() + "."; if(q==30) { btnNextQ.Text = "Finish"; }
                if (q >= 11 && q <= 20)
                {
                    richTextBox2.Text = "II. ANALIZATI CELE DOUA ENUNTURI SI MARCATI CU:\n\ta) daca ambele enunturi sunt adevarate si exista legatura cauzala intre ele;\n\tb) daca ambele enunturi sunt adevarate, dar nu exista legatura cauzala intre ele;\n\tc) daca primul enunt este adevarat, iar al doilea este fals;\n\td) daca primul enunt este fals, iar al doilea este adevarat.";
                    groupBox1.Visible = false;
                    textBox2.Visible = true;
                }
                if (q >= 21 && q <= 30)
                {
                    richTextBox2.Text = "III. MARCATI LITERA CORESPUNZATOARE ORDINII CRONOLOGICE  PE CARE O  CONSIDERATI CORECTA:";
                    groupBox1.Visible = true;
                    textBox2.Visible = false;
                }
                string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
                SQLiteConnection conn = new SQLiteConnection(connstring);
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM TEST WHERE IDC=@IDC LIMIT 1 OFFSET @offset-1";
                cmd.Parameters.AddWithValue("@IDC", myidc);
                cmd.Parameters.AddWithValue("@offset", q);
                try
                {
                    conn.Open();
                    SQLiteDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        richTextBox1.Text = rdr["Enunt"].ToString();
                        radioButton1.Text = rdr["Raspuns1"].ToString();
                        radioButton2.Text = rdr["Raspuns2"].ToString();
                        radioButton3.Text = rdr["Raspuns3"].ToString();
                        radioButton4.Text = rdr["Raspuns4"].ToString();
                        RaspunsC.Text = rdr["RaspunsC"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
                textBox2.Clear();
            }
            else
            {
                var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
                var conn = new SQLiteConnection(connstring);
                var cmd = conn.CreateCommand(); cmd.CommandText = "INSERT INTO Punctaje (User_Email,Punctaj,Data_Efectuarii) VALUES (@email,@punctaj,@dataef)";
                cmd.Parameters.AddWithValue("@email", Navigare.user);
                cmd.Parameters.AddWithValue("@punctaj", score);
                cmd.Parameters.AddWithValue("@dataef", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                try { conn.Open(); cmd.ExecuteNonQuery(); } catch(Exception ex) { throw ex; } finally { conn.Close(); }
                abandon = false;
                this.Close();
                Form frm = new Rezultat();
                frm.Show();
            }                                                          
        }

        private void Test_Admitere_Load(object sender, EventArgs e)
        {
            abandon = true;
            richTextBox2.Text = "I. MARCATI LITERA CORESPUNZATOARE RASPUNSULUI PE CARE IL CONSIDERATI CORECT:";
            myidc = Meniu.idc;
            textBox2.Visible = false;
            string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connstring);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM TEST WHERE IDC=@IDC LIMIT 1 OFFSET @offset-1";
            cmd.Parameters.AddWithValue("@IDC", myidc);
            cmd.Parameters.AddWithValue("@offset", q);
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    richTextBox1.Text = rdr["Enunt"].ToString();
                    radioButton1.Text = rdr["Raspuns1"].ToString();
                    radioButton2.Text = rdr["Raspuns2"].ToString();
                    radioButton3.Text = rdr["Raspuns3"].ToString();
                    radioButton4.Text = rdr["Raspuns4"].ToString();
                    RaspunsC.Text = rdr["RaspunsC"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        // timer1 event
        private void timer1_Tick(object sender, EventArgs e)
        {
            --secunde;
            if(secunde<0)
            {
                --minute;
                secunde = 59;
            }
            if(minute<0)
            {
                gata = true;
            }
            else
            {
                ceas = minute.ToString().PadLeft(2, '0') + ":" + secunde.ToString().PadLeft(2, '0');
                textBox1.Text = ceas;
            }
            if(gata==true)
            {
                timer1.Stop();
                MessageBox.Show("Timpul s-a terminat! Ati obtinut "+score.ToString()+" puncte!");
            }
        }

        // timer trigger - btn start
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false; button2.Visible = false;
            timer1.Interval = 1000;
            timer1.Start();
            btnNextQ.Enabled = true; button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myidc = DateTime.Now.Second%7+1;
            textBox2.Visible = false;
            string connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connstring);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM TEST WHERE IDC=@IDC LIMIT 1 OFFSET @offset-1";
            cmd.Parameters.AddWithValue("@IDC", myidc);
            cmd.Parameters.AddWithValue("@offset", q);
            try
            {
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    richTextBox1.Text = rdr["Enunt"].ToString();
                    radioButton1.Text = rdr["Raspuns1"].ToString();
                    radioButton2.Text = rdr["Raspuns2"].ToString();
                    radioButton3.Text = rdr["Raspuns3"].ToString();
                    radioButton4.Text = rdr["Raspuns4"].ToString();
                    RaspunsC.Text = rdr["RaspunsC"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        private void Test_Admitere_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(abandon==true)
            {
                var connstring = ConfigurationManager.ConnectionStrings["AdmisoftConnectionString"].ConnectionString;
                var conn = new SQLiteConnection(connstring);
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Stats";
                try { conn.Open(); cmd.ExecuteNonQuery(); } catch(Exception ex) { throw ex; } finally { conn.Close(); }
            }
        }
    }
}
