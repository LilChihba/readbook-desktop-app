using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        public WeatherForecastController(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInfo("Вот информационное сообщение от нашего контроллера значений.");
            _logger.LogDebug("Вот отладочное сообщение от нашего контроллера значений.");
            _logger.LogWarn("Вот сообщение предупреждения от нашего контроллера значений.");
            _logger.LogError("Вот сообщение об ошибке от нашего контроллера значений.");
            return new string[] { "value1", "value2" };
        }
    }
}
