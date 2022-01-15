using System;
using System.ComponentModel.DataAnnotations;

namespace TechJobsPersistent.Models
{
    public class AddEmployerViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        public AddEmployerViewModel(string newName, string newLocation)
        {
            Name = newName;
            Location = newLocation;
        }

        public AddEmployerViewModel()
        {

        }

        internal static void Add(Employer newEmployer)
        {
            throw new NotImplementedException();
        }
    }
}
