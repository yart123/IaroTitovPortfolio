using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("MyStory")]
        public IActionResult PersonalStory()
        {
            return View();
        }

        [HttpGet]
        [Route("Projects")]
        public IActionResult Projects()
        {
            return View();
        }
    }
}