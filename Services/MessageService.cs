using LiveQ.Api.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Services
{
    public class MessageService : IMessageService
    {
        async Task IMessageService.Send(string email, string subject, string message)
        {
            try
            {
                //From Address  
                string FromAddress = "ramazan.333a@gmail.com";
                string FromAdressTitle = "Ramazan";
                //To Address  
                string ToAddress = email;
                string Subject = subject;
                string BodyContent = message;

                //Smtp Server  
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number  
                int SmtpPortNumber = 465;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAddress));
                mimeMessage.Subject = Subject; //Subject
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent
                };
                
                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, SecureSocketOptions.SslOnConnect);
                        client.Authenticate(
                            "ramazan.333a@gmail.com",
                            "Password123!@#"
                            );
                        await client.SendAsync(mimeMessage);
                        Console.WriteLine("The mail has been sent successfully !!");
                        await client.DisconnectAsync(true);
                    }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
