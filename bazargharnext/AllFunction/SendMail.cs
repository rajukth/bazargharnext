
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class SendMail
    {

        public bool SendEmail(string reply_to, string reply_subject, string messageDetail)
        {
           
            //Fetching Settings from WEB.CONFIG file.  
            string emailSender = "khatiwadatab@gmail.com";
            string emailSenderPassword = "khatiwada123";
            string emailSenderHost = "smtp.gmail.com";
            int emailSenderPort = 587;
            Boolean emailIsSSL = true;

            string FilePath = "AllFunction/EmailTemplet.csHtml";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();


            //Repalce [newusername] = signup user name   
            MailText = MailText.Replace("[newusername]", reply_to.Trim());
            MailText = MailText.Replace("[Content]", messageDetail);
            

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            //Set From Email ID  
            mailMessage.From = new MailAddress(emailSender);

            //Set To Email ID  
            mailMessage.To.Add(reply_to);

            //Set Subject  
            mailMessage.Subject = reply_subject;

            //Set Body Text of Email   
            mailMessage.Body = MailText;

            //Now set your SMTP   
            SmtpClient _smtp = new SmtpClient();

            //Set HOST server SMTP detail  
            _smtp.Host = emailSenderHost;

            //Set PORT number of SMTP  
            _smtp.Port = emailSenderPort;

            //Set SSL --> True / False  
            _smtp.EnableSsl = emailIsSSL;

            //Set Sender UserEmailID, Password  
            NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
            _smtp.Credentials = _network;

            //Send Method will send your MailMessage create above.  
            _smtp.Send(mailMessage);


            return true;
        }
    }
}
