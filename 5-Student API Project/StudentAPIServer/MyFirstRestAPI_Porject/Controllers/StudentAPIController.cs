using Microsoft.AspNetCore.Mvc; 
using StudentApi.Models;
using StudentApi.DataSimulation;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Common;

namespace StudentApi.Controllers 
{
    [ApiController] // Marks the class as a Web API controller with enhanced features.
   // [Route("[controller]")] // Sets the route for this controller to "students", based on the controller name.
    [Route("api/Students")]

    public class StudentsController : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {
        
        [HttpGet("All",Name ="GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        
        public ActionResult<IEnumerable<Student>> GetAllStudents() // Define a method to get all students.
        {
            if (DataSimulation.StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("No student available");
            }
            return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("Passed",Name ="GetPassedStudensts")]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            

            if (!StudentDataSimulation.StudentsList.Any())
                return NotFound("There is no Passed Students"); 

            return Ok(StudentDataSimulation.StudentsList.Where(st=>st.Grade>=60)); 
        }




        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [HttpGet("AverageGrade", Name ="GetAverageGrade")]
        public ActionResult<double> GetAverageGrade()
        {
            
            if (StudentDataSimulation.StudentsList.Count==0)
            {
                return NotFound("No students found to calculate average");
            }

            return StudentDataSimulation.StudentsList.Average(student => student.Grade);
        }




        [HttpGet("{ID}",Name ="GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> GetStudentByID(int ID)
        {
            if (ID<0)
            {
                return BadRequest();
            }

            var student  =  DataSimulation.StudentDataSimulation.StudentsList.Find(student=>student.Id==ID);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }







        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        [HttpPost(Name ="AddNewStudent")]
        public ActionResult<Student> AddNewStudent(Student newstudent)
        {
            if ( newstudent==null || string.IsNullOrEmpty(newstudent.Name ) || newstudent.Age<=0)
            {
                return BadRequest("Invalid student data.");
            }

            newstudent.Id = StudentDataSimulation.StudentsList.Count()>0?DataSimulation.StudentDataSimulation.StudentsList.Max(st=>st.Id)+1:1;
            StudentDataSimulation.StudentsList.Add(newstudent);
           
            return CreatedAtRoute("GetStudentByID", new {  id= newstudent.Id },newstudent);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}",Name ="DeleteStudent")]
        public ActionResult DeleteStudent(int id)
        {
            if (id<0)
            {
                return BadRequest($"Not Accespted id : {id}");
            }

            var student  = DataSimulation.StudentDataSimulation.StudentsList.Find(st=>st.Id==id);
            if (student == null)
            {
                return NotFound($"Student with id : {id} not found ");
            }

            DataSimulation.StudentDataSimulation.StudentsList.Remove(student);
            return Ok($"student with id {id} has been deleted");
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}",Name ="UpdateStudent")]
        public  ActionResult <Student> UpdateStudent(int id ,Student UpdatedStudent )
        {
            if (id < 0 ||
                UpdatedStudent==null ||
                string.IsNullOrEmpty(UpdatedStudent.Name)|| UpdatedStudent.Id<0 ||UpdatedStudent.Grade<0)
            {
                return  BadRequest("Invalid student data");
            }

            var student = DataSimulation.StudentDataSimulation.StudentsList.FirstOrDefault(st=>st.Id==id);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;

            return Ok(student);

               

            
               
        }

    }
}
