using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=blog;";
        [HttpGet]
        public List<Blogpost> GetPosts()
        {
            List<Blogpost> posts = new List<Blogpost>();

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = "SELECT * FROM `blogpost`;";

            var cmd = new MySqlCommand(sql, connection);

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var post = new Blogpost()
                {
                    Id = data.GetInt32("id"),
                    Title = data.GetString("title"),
                    Content = data.GetString("content"),
                    postTime = data.GetDateTime("postTime"),
                    updateTime = data.GetDateTime("updateTime"),
                    blogId = data.GetInt32("blogId"),
                };
                posts.Add(post);
            }

            connection.Close();

            return posts;
        }
        [HttpPost]
        public object AddNewPost([FromBody]Blogpost AddNewPostDTO)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES ('[value-2]','[value-3]','[value-4]','[value-5]','[value-6]')";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@title", AddNewPostDTO.Title);
            cmd.Parameters.AddWithValue("@content", AddNewPostDTO.Content);
            cmd.Parameters.AddWithValue("@postTime", AddNewPostDTO.postTime);
            cmd.Parameters.AddWithValue("@updateTime", AddNewPostDTO.updateTime);
            cmd.Parameters.AddWithValue("@blogId", AddNewPostDTO.blogId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres felvétel", result = AddNewPostDTO};
        }
    }
}
