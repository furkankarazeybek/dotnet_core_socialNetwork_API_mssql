using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using SocialNetworkAPI.Models;
using SocialNetworkAPI.DAL;

namespace SocialNetworkAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : Controller
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;

        public StaffController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());


        }

        [HttpPost]
        [Route("StaffRegistration")]

        public Response StaffRegistration(Staff staff)
        {
            Response response = new Response();
            Dal dal = new Dal();
            response = dal.StaffRegistration(staff, connection);

            return response;
        }


        [HttpPost]
        [Route("DeleteStaff")]

        public Response StaffDelete(Staff staff)
        {
            Response response = new Response();
            Dal dal = new Dal();
            response = dal.DeleteStaff(staff, connection);

            return response;
        }
    }
}
