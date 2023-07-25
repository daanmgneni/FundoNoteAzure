using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundoFunctions
{
    public class SendEmailOnReceive
    {
        private readonly IConfiguration _config;
        private readonly string _senderEmail;
        private readonly string _senderAppPassword;

        private const int _port = 587;
        private const string _host = "smtp.gmail.com";

        // Constructor
        public SendEmailOnReceive(IConfiguration config)
        {
            _config = config;

            // Read the sender email and app password from configuration.
            _senderEmail = _config["SenderEmail"];
            _senderAppPassword = _config["SenderAppPassword"];
        }

        // The Azure Function that will be triggered when a message is received from the "forget-password" Service Bus topic.
        [FunctionName("SendEmailOnReceive")]
        public void Run([ServiceBusTrigger("forget-password", Connection = "ServiceBusConnection")] string messageBody, string To)
        {
            // Extract the reset token from the message body received from the Service Bus.
            string resetToken = messageBody;

            // Email subject and body content.
            string subject = "Fundoo Notes Reset Token";
            string body = $"\nTo reset your fundoo-notes password, copy and paste the following code : \n\n\n\t {resetToken}";

            // Use Smtp to the email.
            using (SmtpClient smtpClient = new SmtpClient(_host))
            {
                // Set SMTP properties.
                smtpClient.Port = _port;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderAppPassword);

                // Send the email to the specified recipient (To address).
                smtpClient.Send(_senderEmail, To, subject, body);
            }
        }
    }
}
