using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAppMovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryConteoller : ControllerBase
    {
        private readonly ILogger<QueryConteoller> _logger;

        public QueryConteoller(ILogger<QueryConteoller> logger)
        {
            _logger = logger;
        }
    }
}