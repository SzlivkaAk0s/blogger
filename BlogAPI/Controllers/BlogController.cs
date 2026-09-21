using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=blog;";
        [HttpGet]
        public List<Blogger> GetBloggers()
        {
            List<Blogger> bloggers = new List<Blogger>();

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = "SELECT * FROM `blogger`;";

            var cmd = new MySqlCommand(sql, connection);

            var data = cmd.ExecuteReader();

            while(data.Read())
            {
                var blogger = new Blogger()
                {
                    Id = data.GetInt32("id"),
                    Name = data.GetString("name"),
                    Email = data.GetString("email"),
                    Age = data.GetInt32("age"),
                    Password = data.GetString("password"),
                    RegistrationTime = data.GetDateTime("registrationTime")
                };
                bloggers.Add(blogger);
            }

            connection.Close();

            return bloggers;
        }

        [HttpPost]
        public object AddNewBlogger([FromBody] AddNewBloggerDTO addNewBloggerDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@name", addNewBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", addNewBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", addNewBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", addNewBloggerDto.Password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new {message = "Sikeres felvétel", result = addNewBloggerDto};
        }

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `blogger` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres törlés", result = "" };
        }

        [HttpPut]
        public object UpdateBlogger([FromQuery]int id, UpdateBloggerDTO updateBloggerDTO)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            var sql = @"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE `Id` = @id;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDTO.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres frissítés", result = updateBloggerDTO};
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT * FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            object? data = null;

            if (datareader.Read() == true)
            { 
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32("id"),
                    Name = datareader.GetString("name"),
                    Email = datareader.GetString("email"),
                    Age = datareader.GetInt32("age"),
                    Password = datareader.GetString("password"),
                    RegistrationTime = datareader.GetDateTime("registrationTime")
                };
                data = new { message = "Sikeres lekérdezés", result = blogger };
            }
            else
            {
                data = new { message = "Nincs ilyen blogger", result = "" };
            }

            connection.Close();
            return data;
        }
    }
}
