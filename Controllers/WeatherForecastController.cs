using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMovies.Logic;

namespace WebAppMovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private Queries queries = new Queries();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] Query[] q)
        {
            queries.QueriesSolution(q);
        }
    }
}