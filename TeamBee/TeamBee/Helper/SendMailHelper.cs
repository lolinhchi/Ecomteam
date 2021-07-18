using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TeamBee.Helper
{
    public class SendMailHelper
    {
        public static bool Send(string toaddress, string subject, string emailbody)
        {
            try
            {
                SendEmail(toaddress, subject, emailbody);
                return true;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return false;
            }
        }
        private static void SendEmail(string toaddress, string subject, string emailbody)
        {
            var configuration = ConfigurationBuilderExtensions.Configuration;
            var username = configuration["Gmail:Username"];
            var password = configuration["Gmail:Password"];
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(username);
                mail.To.Add(toaddress);
                mail.Subject = subject;
                mail.Body = emailbody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
