


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
        
        httpClient.BaseAddress = new Uri("http://localhost:5215/api/Students");

        await GetAllStudents();




        Console.ReadLine();





    }


    public static   async Task GetAllStudents()
    {
        
        try
        {

            Console.WriteLine("\n------------------------------------------------------\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nFeatching Students ......\n");

            Console.ForegroundColor = ConsoleColor.White;

            var Students = await httpClient.GetFromJsonAsync<List<Student>>("");

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
            Console.WriteLine( "An Error Accured :(" + ex.Message);
        }
       
       
        


    }
}
