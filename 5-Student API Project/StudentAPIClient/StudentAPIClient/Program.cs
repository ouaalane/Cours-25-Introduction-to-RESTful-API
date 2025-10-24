using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Net.Http;
using global::System.Threading;
using global::System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int Grade { get; set; }


}

partial class Program
{
   

    public static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main(string[] args)
    {
        httpClient.BaseAddress = new Uri("http://localhost:5215/api/Students/");



        await GetAllStudents();

        await GetPassedStudents();

        await GetAverageGrade();

        await GetStudentById(-1);
        await GetStudentById(500);
        await GetStudentById(10);

        Student newstudent = new Student
        {
            Id = 0,
            Name = "Mohamed ouaalane",
            Age = 19,
            Grade = 100
        };
         await AddNewStudent(newstudent);

        await GetAllStudents();


        // delete student 1 (simulation)
        await DeleteStudent(1);

        await GetAllStudents();


        Student s = new Student
        {
            Name = "batol tahdaoui",
            Age = 60,
            Grade=60
        };
        await UpdateStudent(2, s);



        await GetStudentById(2);

        Console.ReadLine();





    }


    public static async Task GetPassedStudents()
    {
        Console.WriteLine("\n----------------------------------------\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetching Passed Students  ... ");
        Console.ForegroundColor = ConsoleColor.White;


        try
        {
            var httpmessage = await httpClient.GetAsync("Passed");

            if (httpmessage.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var PassedStudents = await httpmessage.Content.ReadFromJsonAsync<List<Student>>();
                if (PassedStudents != null)
                {
                    foreach (var student in PassedStudents)
                    {
                        Console.WriteLine($"Student ID {student.Id} , Name : {student.Name} , Age : {student.Age}");

                    }
                }
            }
            else if (httpmessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("no students passed :(");
            }
            

            
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
            
        }
        

        
    }


    public static   async Task GetAllStudents()
    {

        
        try
        {
           

            Console.WriteLine("\n------------------------------------------------------\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nFeatching Students ......\n");

            Console.ForegroundColor = ConsoleColor.White;

            HttpResponseMessage HttpMessage = await httpClient.GetAsync("All");


            if (HttpMessage.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var Students  =   await  HttpMessage.Content.ReadFromJsonAsync<List<Student>>();

            
                foreach (var student in Students)
                {
                    Console.WriteLine($"Student ID {student.Id} , Name : {student.Name} , Age : {student.Age}");

                }


            }
            else if (HttpMessage.StatusCode== System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("no student found ! :(");
            }
            
          

           
               
           
            
        }
        catch(Exception ex)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;


        }





    }


    static async Task GetAverageGrade()
    {
        try
        {
            Console.WriteLine("\n_____________________________");
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("\nFetching average grade...\n");
            Console.ForegroundColor = ConsoleColor.White;   


            var httpResponse = await httpClient.GetAsync("AverageGrade");

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var averageGrade  =  await httpResponse.Content.ReadFromJsonAsync<float>();
                Console.WriteLine($"Average Grade: {averageGrade}");

            }
            else if (httpResponse.StatusCode==System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("not student found");
            }

           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static async Task GetStudentById(int id)
    {
        try
        {
            Console.WriteLine("\n_____________________________");
            Console.WriteLine($"\nFetching student with ID {id}...\n");

            var response = await httpClient.GetAsync($"{id}");
           
           


            if (response.IsSuccessStatusCode)
            {
                var student = await response.Content.ReadFromJsonAsync<Student>();
                if (student != null)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Bad Request: Not accepted ID {id}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Not Found: Student with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static async Task AddNewStudent(Student  newstudent)
    {
        try
        {
            Console.WriteLine("\n_____________________________");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAdding a new student...\n");
            Console.ForegroundColor = ConsoleColor.White;

            var response = await httpClient.PostAsJsonAsync("",newstudent);

            if (response.StatusCode==System.Net.HttpStatusCode.Created)
            {
                var addedstudent=  await response.Content.ReadFromJsonAsync<Student>();
                Console.WriteLine($"New student added succesfully: {addedstudent.Id} ,Name : {addedstudent.Name}, Age : {addedstudent.Age} , Grade : {addedstudent.Grade}");
                
                
            }
            else if (response.StatusCode==System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Bade Request : Invalid student data ");
                
            }

            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static async Task DeleteStudent (int id )
    {
        try
        {
            Console.ForegroundColor  = ConsoleColor.Green ;
            Console.WriteLine("\n_____________________________");
            Console.WriteLine($"\nDeleting student with ID {id}...\n");
            Console.ForegroundColor = ConsoleColor.White ;

            var reponse =  await httpClient.DeleteAsync($"{id}");

            if (reponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Student with id : {id} has been deleted :)");
            }
            else if (reponse.StatusCode==System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Bad request : Ivalid student id ");
            }
            else if (reponse.StatusCode==System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Not Found : student not found :(");
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured  :  {ex.Message} ");
        }
    }



    static async Task UpdateStudent(int id , Student UpdatedStudent)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Green ;
            Console.WriteLine("\n------------------------------------\n");
            Console.WriteLine("Updating Student Data  : ");
            Console.ForegroundColor= ConsoleColor.White ;


            var response = await httpClient.PutAsJsonAsync($"{id}", UpdatedStudent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Student with id : {id} Updated succesfully :)" );
            }
            else if (response.StatusCode ==System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Bad request : student data not valid  :(");
            }
            else if (response.StatusCode==System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("not found :  student was not found  :(");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"an error occured  :(");

        }
    }

}
