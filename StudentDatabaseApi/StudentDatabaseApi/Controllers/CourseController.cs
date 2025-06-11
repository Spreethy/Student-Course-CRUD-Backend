//using StudentApi.Models;
//using System;
//using System.Linq;
//using System.Web.Http;

//namespace StudentApi.Controllers
//{
//    [RoutePrefix("api/courses")]
//    public class CourseController : ApiController
//    {
//        private StudentContext db = new StudentContext();

//        // GET api/courses
//        [HttpGet, Route("")]
//        public IHttpActionResult GetAllCourses()
//        {
//            return Ok(db.Courses.ToList());
//        }

//        // GET api/courses/{id}
//        [HttpGet, Route("{id}")]
//        public IHttpActionResult GetCourse(string id)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            var course = db.Courses.Find(guidId);
//            if (course == null) return NotFound();

//            return Ok(course);
//        }

//        // POST api/courses
//        [HttpPost, Route("")]
//        public IHttpActionResult AddCourse([FromBody] Course course)
//        {
//            if (course == null || string.IsNullOrWhiteSpace(course.CourseName))
//                return BadRequest("Invalid course data.");

//            course.CourseId = Guid.NewGuid();
//            db.Courses.Add(course);
//            db.SaveChanges();

//            return Ok(course);
//        }

//        // PUT api/courses/{id}
//        [HttpPut, Route("{id}")]
//        public IHttpActionResult UpdateCourse(string id, [FromBody] Course updatedCourse)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            if (updatedCourse == null || string.IsNullOrWhiteSpace(updatedCourse.CourseName))
//                return BadRequest("Invalid course data.");

//            var course = db.Courses.Find(guidId);
//            if (course == null) return NotFound();

//            course.CourseName = updatedCourse.CourseName;
//            course.Credits = updatedCourse.Credits;

//            db.SaveChanges();

//            return Ok(course);
//        }

//        // DELETE api/courses/{id}
//        [HttpDelete, Route("{id}")]
//        public IHttpActionResult DeleteCourse(string id)
//        {
//            if (!Guid.TryParse(id, out Guid guidId))
//                return BadRequest("Invalid ID format.");

//            var course = db.Courses.Find(guidId);
//            if (course == null) return NotFound();

//            db.Courses.Remove(course);
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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace StudentDatabaseApi.Controllers
{
    [RoutePrefix("api/courses")]
    public class CourseController : ApiController
    {
        private readonly StudentContext _context = new StudentContext();

        // GET: api/courses
        [HttpGet, Route("")]
        public IEnumerable<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        //// GET: api/courses/5
        //[HttpGet, Route("{id:int}")]
        //public IHttpActionResult GetCourse(int id)
        //{
        //    var course = _context.Courses.Find(id);
        //    if (course == null)
        //        return NotFound();

        //    return Ok(course);
        //}
        [HttpGet, Route("{id:int}", Name = "GetCourseById")]
        public IHttpActionResult GetCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            return Ok(course);
        }


        //// POST: api/courses
        //[HttpPost, Route("")]
        //public IHttpActionResult PostCourse([FromBody] Course course)
        //{
        //    if (course == null || string.IsNullOrWhiteSpace(course.CourseName))
        //        return BadRequest("Invalid course data.");

        //    _context.Courses.Add(course);
        //    _context.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
        //}

        [HttpPost, Route("")]
        public IHttpActionResult PostCourse([FromBody] Course course)
        {
            if (course == null || string.IsNullOrWhiteSpace(course.CourseName))
                return BadRequest("Invalid course data.");

            _context.Courses.Add(course);
            _context.SaveChanges();

            return CreatedAtRoute("GetCourseById", new { id = course.CourseId }, course);
        }

        // PUT: api/courses/5
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult PutCourse(int id, [FromBody] Course updatedCourse)
        {
            if (updatedCourse == null || updatedCourse.CourseId != id)
                return BadRequest("Course data is invalid or mismatched id.");

            var existingCourse = _context.Courses.Find(id);
            if (existingCourse == null)
                return NotFound();

            existingCourse.CourseName = updatedCourse.CourseName;
            existingCourse.Credits = updatedCourse.Credits;

            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/courses/5
        //[HttpDelete, Route("{id:int}")]
        //public IHttpActionResult DeleteCourse(int id)
        //{
        //    var course = _context.Courses.Find(id);
        //    if (course == null)
        //        return NotFound();

        //    _context.Courses.Remove(course);
        //    _context.SaveChanges();

        //    return Ok(course);
        //}


        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult DeleteCourse(int id)
        {
            // Check if students exist with this course id
            bool hasStudents = _context.Students.Any(s => s.CourseId == id);
            if (hasStudents)
            {
                return Content(HttpStatusCode.Conflict, new { message = "Cannot delete this course because it is assigned to one or more students." });
            }


            var course = new Course { CourseId = id };
            _context.Courses.Attach(course);
            _context.Courses.Remove(course);

            try
            {
                _context.SaveChanges();
                return Ok(course);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Content(HttpStatusCode.Conflict, new { message = "The course was already deleted or updated by another process." });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}

