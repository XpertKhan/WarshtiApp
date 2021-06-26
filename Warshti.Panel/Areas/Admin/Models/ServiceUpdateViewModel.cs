using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class ServiceUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public List<SelectListItem> Companies { get; set; }

        [Required]
        public int ModelId { get; set; }
        public List<SelectListItem> Models { get; set; }

        [Required]
        public int ColorId { get; set; }
        public List<SelectListItem> Colors { get; set; }

        [Required]
        public int TransmissionId { get; set; }
        public List<SelectListItem> Transmissions { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public List<SelectListItem> Users { get; set; }

        public int ServiceStatus { get; set; }

        //public virtual ICollection<ServiceFault> ServiceFaults { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
        //public Service()
        //{
        //    ServiceFaults = new HashSet<ServiceFault>();
        //    Orders = new HashSet<Order>();
        //}
        public List<ServiceFaultViewModel> Faults { get; set; }
        public ServiceUpdateViewModel()
        {
            Companies = new List<SelectListItem>();
            Faults = new List<ServiceFaultViewModel>();
        }
    }
}
