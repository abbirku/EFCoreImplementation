using Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Course : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SeatCount { get; set; }
        public int Fee { get; set; }
        public List<StudentRegistration> Students { get; set; }
    }
}
