using Microsoft.AspNetCore.Mvc;
using SocialNetworkAPI.Models;
using Microsoft.Data.SqlClient;
using SocialNetworkAPI.DAL;
namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());


        }

        [HttpPost]
        [Route("Registration")]

        public Response Registration(Registration registration)
        {
            Response response = new Response();
            Dal dal = new Dal();
            response = dal.Registration(registration, connection);
           
            return response;
        }

        [HttpPost]
        [Route("Login")]
        public Response Login(Registration registration)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());
            Dal dal = new Dal();
            response = dal.Login(registration, connection);

            return response;
        }

        [HttpPost]
        [Route("UserApproval")]
        public Response UserApproval(Registration registration)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());
            Dal dal = new Dal();
            response = dal.UserApproval(registration, connection);

            return response;
        }

     
    }
}
