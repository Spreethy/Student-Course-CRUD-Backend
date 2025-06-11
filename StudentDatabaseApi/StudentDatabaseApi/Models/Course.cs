//using System;
//using System.ComponentModel.DataAnnotations;

//namespace StudentApi.Models
//{
//    public class Course
//    {
//        [Key]
//        public int  CourseId { get; set; } = Guid.NewGuid();

//        [Required]
//        public string CourseName { get; set; }

//        public int? Credits { get; set; }
//    }
//}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentDatabaseApi.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public int Credits { get; set; }
    }
}
