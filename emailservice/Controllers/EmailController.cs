using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using emailservice.Models;
using emailservice.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
namespace emailservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IHostingEnvironment _iHostingEnvironment;
        private readonly IEmailService _iEmailService;
        public Result _result;

        public EmailController(IEmailService emailService, IHostingEnvironment iHostingEnvironment)
        {
            _iEmailService = emailService;
            _iHostingEnvironment = iHostingEnvironment;
            _result = new Result();
        }
        // POST api/values               

        [HttpGet]
        public string GET() {
            return _iHostingEnvironment.WebRootPath;
        }

        [HttpPost]
        public async Task<Result> Post([FromBody] Order order)
         {
            try
            {
                if (ModelState.IsValid)
                {
                    await _iEmailService.SendEmail(order);

                    _result.Message = "Successful 👍";
                    _result.Status = Ok().StatusCode;
                    _result.StatusMessage = StatusCodes.Status200OK.ToString() + "OK 👍";
                    return _result;
                }
            }
            catch (Exception e)
            {
                _result.Message = e.Message;
                _result.Status = StatusCodes.Status500InternalServerError;
                _result.StatusMessage = StatusCodes.Status500InternalServerError.ToString() + "InternalServerError 👎";
            }


            return _result;
        }
    }
}
