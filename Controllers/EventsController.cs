using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using SocialNetworkAPI.Models;
using SocialNetworkAPI.DAL;

namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;

        public EventsController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("SocialNetwork").ToString());


        }
        [HttpPost]
        [Route("AddEvents")]
        public Response EventsAdd(Events events)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.AddEvent(events, connection);

            return response;
        }

        [HttpGet]
        [Route("EventsList")]
        public Response ListEvents(Events events)
        {
            Response response = new Response();

            Dal dal = new Dal();

            response = dal.EventsList(connection);

            return response;
        }





    }
}
