using BHub.Infra.Environments;
using BHub.Infra.Extension.Interfaces;
using System.Net;
using System.Net.Mail;

namespace BHub.Infra.Extension
{
    public class MailExtension : IMailExtension
    {
        public void SendMail(string cliente)
        {
            var user = Constants.SMTP_USERNAME;
            var pass = Constants.SMTP_USER_PASSWORD;
            var email = Constants.SMTP_SERVER_ADDRESS;

            var mail = new MailMessage
            {
                From = new MailAddress(user, "Bruno Vieira", System.Text.Encoding.UTF8)
            };
            
            var resultado = string.Join(",", cliente);
            
            mail.To.Add("brunojpv@outlook.com");
            mail.Subject = "Clientes Inseridos com Sucesso!";
            mail.IsBodyHtml = true;
            mail.Body = $"<h3>Olá,</h3>  <p>Foram inseridos os clientes:  <b>{resultado}</b>. ";

            SmtpClient client = new(email)
            {
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass)
            };

            client.Send(mail);
        }
    }
}
