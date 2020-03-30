using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using emailservice.Models;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Web.Http;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Security.AccessControl;

namespace emailservice.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _iHostingEnvironment;

        public EmailService(IConfiguration configuration, IHostingEnvironment iHostingEnvironment)
        {
            _configuration = configuration;
            _iHostingEnvironment = iHostingEnvironment;
        }                              

        public async Task SendEmail(Order order)
        {
            using (var client = new SmtpClient())
            {

                client.UseDefaultCredentials = false;
                var credential = new NetworkCredential()
                {
                    UserName = _configuration["Email:Email"],
                    Password = _configuration["Email:Password"]
                };
                client.Credentials = credential;
                client.Host = _configuration["Email:Host"];
                client.Port = int.Parse(_configuration["Email:Port"]);

                //Enable encryption-based Internet security protocol
                client.EnableSsl = true;


                //Email message used to send email
                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(order.Customer.Email));
                    emailMessage.From = new MailAddress(_configuration["Email:Email"]);
                    emailMessage.Subject = "Shopping order confirmation";
                    emailMessage.Body = SetEmailBody(order);
                    emailMessage.IsBodyHtml = true;
                    client.Send(emailMessage);
                }
            }

            await Task.CompletedTask;
        }

        public string SetEmailBody(Order order)
        {
            string body = string.Empty;
            string halfbody = string.Empty;
            using (StreamReader reader = new StreamReader(_iHostingEnvironment.WebRootPath + "/Assets/template.html"))
            {
                body = reader.ReadToEnd();
            }
            
            using (FileStream fs = new FileStream(_iHostingEnvironment.WebRootPath + "/Assets/template2.html", FileMode.Create))
            {                
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    //Agregar accesos al archivo (modificar)
                    w.WriteLine("<table celpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                    w.WriteLine("<tr>");
                    w.WriteLine("<th></th>");
                    w.WriteLine("<th>Id</th>");
                    w.WriteLine("<th>Name</th>");
                    w.WriteLine("<th>Description</th>");
                    w.WriteLine("</tr>");
                    foreach (var item in order.Items)
                    {
                        w.WriteLine(
                            "<tr>" +
                            "<td width=\"100\" align=\"center\"> <img width=\"100\" src=\"" + item.Image +"\">"+ "</td>" +
                            "<td width=\"100\" align=\"center\">" + item.IdProduct + "</td>" +
                            "<td width=\"200\" align=\"center\">" + item.Name + "</td>" +
                            "<td align=\"center\">" + item.Description + "</td>" + "</tr>");
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</body>");
                    w.WriteLine("</html>");
                    w.Close();

                }
            }

            using (StreamReader reader = new StreamReader(_iHostingEnvironment.WebRootPath + "/Assets/template2.html"))
            {
                halfbody = reader.ReadToEnd();
            }
            body = body.Replace("{order.customer.name}", order.Customer.Name);
            body = body.Replace("{order.id}", order.Id);
            body = body.Replace("{order.shipping_tracking_id}", order.ShippingTrackingId);
            body = body.Replace("{order.shipping_address.street_address_1}", order.ShippingAddress.StreetAddress1);
            body = body.Replace("{order.shipping_address.street_address_2}", order.ShippingAddress.StreetAddress2);
            body = body.Replace("{order.shipping_tracking_id}", order.ShippingTrackingId);
            body = body.Replace("{order.shipping_address.country}", order.ShippingAddress.Country);
            body = body.Replace("{order.shipping_address.city}", order.ShippingAddress.City);
            body = body.Replace("{order.shipping_address.zip_code}", order.ShippingAddress.ZipCode);

            return body + halfbody;
        }
    }
}
