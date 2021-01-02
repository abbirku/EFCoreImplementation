using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class RegistrationDTO
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string CourseTitle { get; set; }
        public string RegistrationDate { get; set; }
        public string IsPaymentComplete { get; set; }
    }
}
