
using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using SocialNetworkAPI.Models;
using SocialNetworkAPI.DAL;

namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;

        public NewsController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());


        }

        [HttpPost]
        [Route("AddNews")]
        public Response NewsAdd(News news)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.AddNews(news, connection);

            return response;
        }

        [HttpGet]
        [Route("NewsList")]
        public Response ListNews(News news)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.NewsList(connection);

            return response;
        }

    }
}
