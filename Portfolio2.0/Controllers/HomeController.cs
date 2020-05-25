using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Config;
        private readonly StorageConnector storageConnector;

        public HomeController(IConfiguration config)
        {
            Config = config;
            storageConnector = new StorageConnector(Config["PortfolioApp:Connections:PortfolioStorage"]);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.LeftLink = new KeyValuePair<string, string>("My Story", "/MyStory");
            ViewBag.RightLink = new KeyValuePair<string, string>("Projects", "/Projects");

            IEnumerable<SkillGroupEntity> skills = await storageConnector.LoadSkillGroups();
            return View(skills);
        }

        [HttpGet]
        [Route("MyStory")]
        public async Task<IActionResult> PersonalStoryAsync()
        {
            ViewBag.LeftLink = new KeyValuePair<string, string>("Home", "/");
            ViewBag.RightLink = new KeyValuePair<string, string>("Projects", "/Projects");

            IEnumerable<EventEntity> events = await storageConnector.LoadEvents();
            return View(events);
        }

        [HttpGet]
        [Route("Projects")]
        public async Task<IActionResult> ProjectsAsync()
        {
            ViewBag.LeftLink = new KeyValuePair<string, string>("My Story", "/MyStory");
            ViewBag.RightLink = new KeyValuePair<string, string>("Home", "/");

            IEnumerable<ProjectEntity> projects = await storageConnector.LoadProjects();

            return View(projects);
        }
    }
}