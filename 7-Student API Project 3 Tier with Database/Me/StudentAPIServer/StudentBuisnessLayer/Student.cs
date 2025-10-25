using Student.DTOs;
using StudentDataAccessLayer;

namespace StudentBuisnessLayer
{
    public class clsStudent
    {



        public enum enMode {AddNew=0,Update=1};
        public int Id { get; set; }
        public string Name { get; set; }
        public int  Age  { get; set; }

        public int Grade { get; set; }

        private enMode Mode =  enMode.AddNew;




        public StudentDTO StudentDTO
        {
            get
            {
                return new StudentDTO(this.Id, this.Name,this.Age, this.Grade);
            }
        }


        public StudentAddDTO StudentAddDTO
        {
            get
            {
                return new StudentAddDTO(this.Name,this.Age,this.Grade);
            }
        }


        public clsStudent(StudentDTO student,enMode mode=enMode.AddNew)
        {
           this.Id = student.Id;
            this.Name = student.Name;
            this.Age = student.Age;
            this.Grade = student.Grade;

            this.Mode = mode;
        }


        private bool _AddNewStudent()
        {
             this.Id = clsStudentData.AddStudent(this.StudentAddDTO);
            return (this.Id >0);

        }


        private bool _UpdateStudent()
        {
            return clsStudentData.UpdateStudent(StudentDTO);
        }
        public static List<StudentDTO> GetAllStudents()
        {

            return clsStudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return clsStudentData.GetPassedStudents();
        }


        public static double GetAverageGrade()
        {
            return clsStudentData.GetAverageGrade();
        }


        public static bool DeleteStudent(int id )
        {
            return clsStudentData.DeleteStudent(id);
        }

        public static clsStudent Find(int studentID)
        {
            StudentDTO student = clsStudentData.GetStudentByID(studentID);
            if (student == null)
            {
                return null;
            }
            return new clsStudent(student, enMode.Update);
          
        }
        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                   
                   if ( _AddNewStudent())
                   {
                        this.Mode = enMode.Update;
                        return true;
                   }
                   else
                   {
                        return false;
                   }
                      
                   
    
                case enMode.Update:
                   return   _UpdateStudent();
                    
            }

            return false;
               
        }
    }
}
