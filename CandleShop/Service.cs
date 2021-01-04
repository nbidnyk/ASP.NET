using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mail;
//using Microsoft.Extensions.Logging;

namespace CandleShop
{
    public class Service
    {
        //private readonly ILogger<Service>;
        public void SendEmailDefault()
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress("admin@mycompany.com", "CandleShop");
            message.To.Add("natasha.bidnyk@gmail.com");
            message.Subject = "Сообщение от System.Net.Mail";
            message.Body = "<div> Сообщение от System.Net.Mail</div>";

            using (SmtpClient client = new SmtpClient("smpt.gmail.com"))
            {
                client.Credentials = new NetworkCredential("photo.storage.066@gmail.com", "photo666"); //логин-пароль от аккаунта
                client.Port = 587; //порт 587 либо 465
                client.EnableSsl = true; //SSL обязательно

                client.Send(message);
            }
        }
        public void SendEmailCustom()
        {

        }
    }
}