using System.Net;
using System.Net.Mail;
namespace CloudComputingProject
{
    public class emailsender
    {
        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername = "rotitovy@gmail.com";
        private readonly string smtpPassword = "obur ltdx dmib qetw\r\n";

        public void SendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(smtpUsername);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = smtpPort;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = smtpHost;
            smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

            smtp.Send(mail);
        }
    }
}