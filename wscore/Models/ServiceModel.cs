using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Entities.Maintenance;
using WScore.Entities.Identity;

namespace WScore.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public string Company { get; set; }
        public int ModelId { get; set; }
        public string Model { get; set; }
        public int ColorId { get; set; }
        public string Color { get; set; }
        public int TransmissionId { get; set; }
        public string Transmission { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }

        public int UserId { get; set; }
        public String User { get; set; }
        public List<ServiceFaultModel> ServiceFaults { get; set; }
        public List<ServiceImage> ServiceImages { get; set; }
        public ServiceModel()
        {
            ServiceFaults = new List<ServiceFaultModel>();
        }
    }


  public class ServiceModelCustom
  {
    public int Id { get; set; }
    public string Description { get; set; }

    public int CompanyId { get; set; }
    public string Company { get; set; }
    public int ModelId { get; set; }
    public string Model { get; set; }
    public int ColorId { get; set; }
    public string Color { get; set; }
    public int TransmissionId { get; set; }
    public string Transmission { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethod { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public List<ServiceFaultModel> ServiceFaults { get; set; }
    public List<ServiceImage> ServiceImages { get; set; }
    public ServiceModelCustom()
    {
      ServiceFaults = new List<ServiceFaultModel>();
    }
  }

}
