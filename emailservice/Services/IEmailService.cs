using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using emailservice.Models;

namespace emailservice.Services
{
    public interface IEmailService
    {
        Task SendEmail(Order order);
        string SetEmailBody(Order order);
    }
}
