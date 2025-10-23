


using System.Net.Http.Json;

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
            var PassedStudents =  await httpClient.GetFromJsonAsync<List<Student>>("Passed");

            if (PassedStudents != null)
            {
                foreach (var student in PassedStudents)
                {
                    Console.WriteLine($"Student ID {student.Id} , Name : {student.Name} , Age : {student.Age}");

                }
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

            var Students = await httpClient.GetFromJsonAsync<List<Student>>("All");

            if (Students!=null)
            {
                foreach (var student in Students)
                {
                    Console.WriteLine($"Student ID {student.Id} , Name : {student.Name} , Age : {student.Age}");

                }
            }
            
        }
        catch(Exception ex)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;


        }





    }
}
