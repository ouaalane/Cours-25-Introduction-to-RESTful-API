
using System.ComponentModel.DataAnnotations;

namespace Student.DTOs
{
    public class StudentDTO
    {

        public StudentDTO(int id,string name,int age , int grade)
        {
            this .Id = id;  
            this .Name = name;
            this.Age = age;
            this.Grade = grade;
        }


        
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Range(1, 150)]
        public int Age { get; set; }

        [Range(0, 100)]
        public int Grade { get; set; }


    }


    public class StudentAddDTO
    {


       
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1,150)]
        public int Age { get; set; }

        [Required]
        [Range(0,100)]
        public int Grade { get; set; }


        public  StudentAddDTO(string name, int age, int grade)
        {
         
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }
       
    


    }



}
