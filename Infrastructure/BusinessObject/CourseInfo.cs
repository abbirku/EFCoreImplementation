using Infrastructure.Entities;

namespace Infrastructure.BusinessObject
{
    public class CourseInfo
    {
        public Course Course { get; set; }

        public ValidationModel IsValid()
        {
            if (Course == null)
                return new ValidationModel { IsValid = false, Message = "No course has been provided" };

            if(string.IsNullOrEmpty(Course.Title))
                return new ValidationModel { IsValid = false, Message = "Cource name can not be null or empty" };

            if(Course.SeatCount == 0)
                return new ValidationModel { IsValid = false, Message = "Each course should have at least 10 seats" };

            if (Course.Fee < 500)
                return new ValidationModel { IsValid = false, Message = "Cource Fee must be greater then 500 Taka" };

            return new ValidationModel { IsValid = true, Message = "Course information is valid" };
        }
    }
}
