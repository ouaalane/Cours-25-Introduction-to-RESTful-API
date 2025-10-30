using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Common;
using Student.DTOs;
using StudentBuisnessLayer;
using StudentDataAccessLayer;
using System.Data;
using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.RateLimiting;

namespace StudentApi.Controllers 
{

    [ApiVersion(1,Deprecated =false)]
    [ApiController] // Marks the class as a Web API controller with enhanced features.
   // [Route("[controller]")] // Sets the route for this controller to "students", based on the controller name.
    [Route("api/v{v:apiVersion}/Students")]

    public class StudentsController : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {
        
        [HttpGet("All",Name ="GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        

        // pagination , filtring,sorting & seraching
        // 
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents([FromQuery] int PageNumber,[FromQuery]int PageSize) // Define a method to get all students.
        {

            var StudentsList = clsStudent.GetAllStudents();
            StudentsList = StudentsList.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            if (StudentsList.Count==0)
            {
                return NotFound("No student available");
            }
            return Ok(StudentsList); // Returns the list of students.
        }




      
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("Passed", Name = "GetPassedStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {

            var StudentsList  = clsStudent.GetPassedStudents();
            if (!StudentsList.Any())
                return NotFound("There is no Passed Students");

            return Ok(StudentsList);
        }




        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        public ActionResult<double> GetAverageGrade()
        {

            var AverageGrade = clsStudent.GetAverageGrade();
            if (AverageGrade==0)
            {
                return NotFound("No students found to calculate average");
            }

            return Ok(AverageGrade);    
        }




        [HttpGet("{id}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> GetStudentByID(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var  student =  clsStudent.Find(id);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student.StudentDTO);
        }







        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        

        [HttpPost(Name = "AddNewStudent")]
        public ActionResult<StudentDTO> AddNewStudent(StudentAddDTO newstudent)
        {
            if (newstudent == null || string.IsNullOrEmpty(newstudent.Name) || newstudent.Age <= 0)
            {
                return BadRequest("Invalid student data.");
            }



            clsStudent Student  = new clsStudent(new StudentDTO(0, newstudent.Name,newstudent.Age,newstudent.Grade));
            
            if (Student.Save())
            {
                return CreatedAtRoute("GetStudentByID", new { id = Student.Id }, Student.StudentDTO);
            }
            return StatusCode(500, "Failed to save student");
            

           
        }



      

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}", Name = "DeleteStudent")]
        public ActionResult DeleteStudent(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Not Accepted id : {id}");
            }

            if (clsStudent.DeleteStudent(id))
                return Ok($"Student with ID {id} has been deleted.");
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");
        }




    
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpPut("{id}", Name = "UpdateStudent")]
        public ActionResult<StudentDTO> UpdateStudent(int id, StudentAddDTO UpdatedStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid student data");
            }

            clsStudent student = clsStudent.Find(id) ;
            if (student==null)
            {
                return NotFound("Person Is not Found");
            }

            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;
            
            
            if (student.Save())
            {
                return Ok(student.StudentDTO);
            }
            else
            {
                return StatusCode(500, "could not update student");
            }



        }

    }




 


}
