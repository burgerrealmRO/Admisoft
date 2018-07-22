using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
namespace Admisoft
{
    public partial class Contact : Form
    {
        public Contact()
        {
            InitializeComponent();
        }

        private async void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(textBox2.Text.Trim(),textBox4.Text.Trim());
                mail.To.Add(textBox1.Text.Trim());
                mail.From = new MailAddress(textBox2.Text.Trim());
                mail.Subject = textBox3.Text;
                mail.Body = richTextBox1.Text;
                client.Send(mail);
                MessageBox.Show("Email-ul a fost trimis cu succes!");
                await Task.Delay(2000);
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Contact_Load(object sender, EventArgs e)
        {
           pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory().Trim() + "\\gmailg.png");
        }
    }
}
