
using Microsoft.Data.SqlClient;
using Student.DTOs;
using System.Data;
using System.Numerics;

namespace StudentDataAccessLayer
{
    public static class clsStudentData
    {

        static string _ConnectionString = "server=.;DataBase=StudentsDB;User=sa;Password=123456;TrustServerCertificate=true;";



        public static List<StudentDTO> GetAllStudents()
        {

            List<StudentDTO> Students = new List<StudentDTO>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllStudents", sqlConnection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlConnection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Students.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Students;


        }


        public static List<StudentDTO> GetPassedStudents()
        {
            List<StudentDTO> Students = new List<StudentDTO>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPassedStudents", sqlConnection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlConnection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Students.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Students;

        }


        public static double GetAverageGrade()
        {
            double AverageGrade = 0;

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        AverageGrade = Convert.ToDouble(result);
                    }
                    else
                        AverageGrade = 0;

                }
            }

            return AverageGrade;
        }


        public static int AddStudent(StudentAddDTO newstudent)
        {


            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", newstudent.Name);
                command.Parameters.AddWithValue("@Age", newstudent.Age);
                command.Parameters.AddWithValue("@Grade", newstudent.Grade);
                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }


        public static bool UpdateStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentId", StudentDTO.Id);
                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }


        public static StudentDTO GetStudentByID(int studentId)
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        public static  bool DeleteStudent(int StudentID)
        {

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", StudentID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteNonQuery();
                return (rowsAffected >0);


            }
        }



    }
}
