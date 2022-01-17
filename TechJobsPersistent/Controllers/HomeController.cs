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
            employers = context.Employers.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            
            foreach(Employer e in employers)
            {
                SelectListItem item = new SelectListItem();
                item.Text = e.Name;
                item.Value = e.Id.ToString();
                item.Selected = false;
                items.Add(item);
            }

            List<Skill> skills = context.Skills.ToList();


            AddJobViewModel addJobViewModel = new AddJobViewModel(items, skills);

            return View(addJobViewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                Employer emp = context.Employers.Find(addJobViewModel.EmployerId);
                Job newJob = new Job
                {
                    Employer = emp,
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId
                };

                newJob.JobSkills = new List<JobSkill>();
                foreach (var skillId in selectedSkills)
                {
                    JobSkill newJobSkill = new JobSkill()
                    {
                        JobId = newJob.Id,
                        SkillId = int.Parse(skillId),
                    };

                    newJob.JobSkills.Add(newJobSkill);
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();

                string redirectUrl = "/List/Jobs?column=employer&value=" + emp.Name;

                return Redirect(redirectUrl);
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


/*public IActionResult Add()
{
    List<EventCategory> categories = context.Categories.ToList();
    AddEventViewModel addEventViewModel = new AddEventViewModel(categories);

    return View(addEventViewModel);
}

[HttpPost]
public IActionResult Add(AddEventViewModel addEventViewModel)
{
    if (ModelState.IsValid)
    {
        EventCategory theCategory = context.Categories.Find(addEventViewModel.CategoryId);
        Event newEvent = new Event
        {
            Name = addEventViewModel.Name,
            Description = addEventViewModel.Description,
            ContactEmail = addEventViewModel.ContactEmail,
            Category = theCategory
        };

        context.Events.Add(newEvent);
        context.SaveChanges();

        return Redirect("/Events");
    }

    return View(addEventViewModel);
}

public IActionResult Detail(int id)
        {
            Event theEvent = context.Events
               .Include(e => e.Category)
               .Single(e => e.Id == id);

            EventDetailViewModel viewModel = new EventDetailViewModel(theEvent);
            return View(v 
 */