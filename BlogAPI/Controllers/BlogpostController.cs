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
        public object AddNewPost([FromBody]AddNewPostDTO AddNewPostDTO)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`,`updateTime`, `blogId`) VALUES (@Title,@Content,@postTime,@updateTime,@blogId)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Title", AddNewPostDTO.Title);
            cmd.Parameters.AddWithValue("@Content", AddNewPostDTO.Content);
            cmd.Parameters.AddWithValue("@postTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@blogId", AddNewPostDTO.blogId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres felvétel", result = AddNewPostDTO};
        }

        [HttpDelete]
        public object DeletePost(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `blogpost` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres törlés", result = "" };
        }

        [HttpPut]
        public object UpdatePost([FromQuery] int id, UpdatePostDTO updatePostDTO)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            var sql = @"UPDATE `blogpost` SET `Title`=@title,`Content`=@content,`updateTime`=@updateTime WHERE `Id` = @id;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@title", updatePostDTO.Title);
            cmd.Parameters.AddWithValue("@content", updatePostDTO.Content);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres frissítés", result = updatePostDTO };
        }
    }
}
