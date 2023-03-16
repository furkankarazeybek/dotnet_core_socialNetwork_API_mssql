using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SocialNetworkAPI.DAL;
using SocialNetworkAPI.Models;

namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;

        public ArticleController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());


        }

        [HttpPost]
        [Route("AddArticle")]
        public Response ArticleAdd(Article article)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.AddArticle(article, connection);

            return response;
        }

        [HttpGet]
        [Route("ArticleList")]
        public Response ListArticle(Article article)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.ArticleList(article, connection);

            return response;
        }

        [HttpPost]
        [Route("ArticleApproval")]
        public Response ApprovalArticle(Article article)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());
            Dal dal = new Dal();
            response = dal.ArticleApproval(article, connection);

            return response;
        }

    }
}
