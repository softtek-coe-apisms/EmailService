using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace emailservice.Models
{
    public class Result
    {
        public string Message { get; set; } = String.Empty;
        public int Status { get; set; }
        public String StatusMessage { get; set; } = String.Empty;
    }
}
