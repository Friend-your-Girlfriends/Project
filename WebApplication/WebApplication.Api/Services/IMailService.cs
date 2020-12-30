using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
    public class SendGridMailService // : IMailService
    {
        //public Task SendEmailAsync(string toEmail, string subject, string content)
        //{
        //}
    }
}
