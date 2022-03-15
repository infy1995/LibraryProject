using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LibraryProject.Services
{
    public class MailSender
    {
        //public static void SendMail(string to, string subject, string body)
        //{
        //    var message = new System.Net.Mail.MailMessage("test.do.projektu@gmail.com", to)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true
        //    };
        //    var smtpClient = new System.Net.Mail.SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
        //        Credentials = new System.Net.NetworkCredential(
        //            "test.do.projektu@gmail.com",
        //            "TeStoWany_1234"),
        //        EnableSsl = true
        //    };
        //    smtpClient.Send(message);
        //}

        public static void SendMail(string to, string subject, string body)
        {
            var apiKey = Properties.Settings.Default.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("har95@o2.pl", "Library");
            var sendTo = new EmailAddress(to, to);

            var msg = MailHelper.CreateSingleEmail(
                from,
                sendTo,
                subject,
                body,
                body
                );

            client.SendEmailAsync(msg);
        }
    }
}