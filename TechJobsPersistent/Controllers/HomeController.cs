using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;
        private List<Employer> employers;

        public HomeController(JobDbContext dbContext)
        {

            context = dbContext;
            employers = dbContext.Employers.ToList();
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

           //List<Employer> = employers 
           Console.WriteLine(jobs);
            return View(jobs);

        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            AddJobViewModel viewModel = new AddJobViewModel();
            List<SelectListItem> items = new List<SelectListItem>();
            
            foreach(Employer e in employers)
            {
                SelectListItem item = new SelectListItem();
                item.Text = e.Name;
                item.Value = e.Id.ToString();
                item.Selected = false;
                items.Add(item);
            }

            viewModel.Employers = items;

            return View(viewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel)
        {
            if (ModelState.IsValid)
            {
                Employer emp = context.Employers.Find(addJobViewModel.EmployerId);
                Job newJob = new Job
                {
                    Employer = emp,
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId
                };
                context.Jobs.Add(newJob);
                context.SaveChanges();

                return Redirect("/Job");
            }
            return View("AddJob", addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
