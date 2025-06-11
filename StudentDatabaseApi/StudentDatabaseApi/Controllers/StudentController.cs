//using StudentApi.Models;
//using StudentDatabaseApi.Models;  // Adjust namespace as per your project
//using System;
//using System.Linq;
//using System.Web.Http;

//namespace StudentApi.Controllers
//{
//    [RoutePrefix("api/students")]
//    public class StudentController : ApiController
//    {
//        private StudentContext db = new StudentContext();

//        [HttpGet, Route("")]
//        public IHttpActionResult GetAllStudents()
//        {
//            return Ok(db.Students.ToList());
//        }

//        [HttpGet, Route("{id}")]
//        public IHttpActionResult GetStudent(string id)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            var student = db.Students.Find(guidId);
//            if (student == null) return NotFound();

//            return Ok(student);
//        }

//        [HttpPost, Route("")]
//        public IHttpActionResult AddStudent([FromBody] Student student)
//        {
//            if (student == null || string.IsNullOrWhiteSpace(student.Name))
//                return BadRequest("Invalid student data.");

//            student.Id = Guid.NewGuid();  // Assuming Id is Guid type in your model
//            db.Students.Add(student);
//            db.SaveChanges();

//            return Ok(student);
//        }

//        [HttpPut, Route("{id}")]
//        public IHttpActionResult UpdateStudent(string id, [FromBody] Student updatedStudent)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            if (updatedStudent == null || string.IsNullOrWhiteSpace(updatedStudent.Name))
//                return BadRequest("Invalid student data.");

//            var student = db.Students.Find(guidId);
//            if (student == null) return NotFound();

//            student.Name = updatedStudent.Name;
//            student.Age = updatedStudent.Age;
//            student.Course = updatedStudent.Course;

//            db.SaveChanges();

//            return Ok(student);
//        }

//        [HttpDelete, Route("{id}")]
//        public IHttpActionResult DeleteStudent(string id)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            var student = db.Students.Find(guidId);
//            if (student == null) return NotFound();

//            db.Students.Remove(student);
//            db.SaveChanges();

//            return Ok();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//                db.Dispose();
//            base.Dispose(disposing);
//        }
//    }
//}


using StudentApi.Models;
using StudentDatabaseApi.Models;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Net;
using System.Web.Http;

namespace StudentApi.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        private readonly StudentContext _context = new StudentContext();

        // GET: api/students
        [HttpGet, Route("")]
        public IHttpActionResult GetStudents()
        {
            var students = _context.Students.Include(s => s.Course).ToList();
            return Ok(students);
        }

        // GET by ID with route name
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        public IHttpActionResult GetStudent(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        // POST method uses that route name
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostStudent(Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Students.Add(student);
            _context.SaveChanges();

            // Use the route name exactly as in GET above
            return CreatedAtRoute("GetStudentById", new { id = student.StudentId }, student);
        }



        //// PUT: api/students/5
        //[HttpPut, Route("{id:int}")]
        //public IHttpActionResult PutStudent(int id, Student student)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (id != student.StudentId)
        //        return BadRequest("ID mismatch.");

        //    var existingStudent = _context.Students.Find(id);
        //    if (existingStudent == null)
        //        return NotFound();

        //    // Update fields
        //    existingStudent.Name = student.Name;
        //    existingStudent.Age = student.Age;
        //    existingStudent.CourseId = student.CourseId;

        //    _context.SaveChanges();
        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        [HttpPut, Route("{id:int}")]
        public IHttpActionResult PutStudent(int id, Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != student.StudentId)
                return BadRequest("ID mismatch.");

            var existingStudent = _context.Students.Find(id);
            if (existingStudent == null)
                return NotFound();

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;
            existingStudent.CourseId = student.CourseId;

            _context.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/students/5
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
