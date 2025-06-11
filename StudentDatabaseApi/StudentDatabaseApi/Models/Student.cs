//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace StudentDatabaseApi.Models
//{
//    public class Student
//    {
//        [Key]
//        public Guid Id { get; set; } 

//        public string Name { get; set; }

//        public int Age { get; set; }

//        public string Course { get; set; }
//    }
//}

using StudentApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDatabaseApi.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
