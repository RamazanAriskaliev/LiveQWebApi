using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LiveQ.Api.Services;
using LiveQ.Api.ViewModels;
using LiveQ.Api.Models;
using LiveQ.Api.Helpers;
using LiveQ.Api.ViewModels.Validations;
using LiveQ.Api.ViewModels.Validations.Common;
using FluentValidation.Results;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace LiveQ.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ValidateModel]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly UserManager<AppUser> _userManager;
        private readonly JsonSerializerSettings _serializerSettings;

        public EventsController(IEventService eventService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _eventService = eventService;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        #region create
        [Authorize(Policy = "CreatorsOnly")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CreateEventDTO model)
        {
            String userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Event _event = AutoMapper.Mapper.Map<Event>(model);
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1000",
                    message = "User with name " + userName + " is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            var status = await _eventService.CreateEvent(_event, user);
            if (!status)
            {
                return new StatusCodeResult(500);
            }
            var _data = AutoMapper.Mapper.Map<CreateEventDTO>(_event);
            var response = new
            {
                status = "success",
                code = "200",
                message = "Ok",
                data = _data
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
        #endregion

        [Authorize(Policy = "CreatorsOnly")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EditEventDTO model)
        {
            String userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Event _event = AutoMapper.Mapper.Map<Event>(model);
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1000",
                    message = "User with name " + userName + " is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            var status = await _eventService.UpdateEvent(_event, user);
            if (!status)
            {
                return new StatusCodeResult(500);
            }
            var _data = AutoMapper.Mapper.Map<EventDTO>(_event);
            var response = new
            {
                status = "success",
                code = "200",
                message = "Ok",
                data = _data
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        // GET api/events
        [HttpGet("{start}/{end}/{name}")]
        public async Task<IActionResult> Get(string start, string end, string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1000",
                    message = "User with name " + name + " is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            var events = _eventService.GetEvents(start, end, user.Id);
            var _data = AutoMapper.Mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(events);
            var response = new
            {
                status = "success",
                code = "200",
                message = "Ok",
                data = _data
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        // GET api/events/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var _event = _eventService.GetEvent(id);
            if (_event == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1002",
                    message = "Event with such id is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            var _data = AutoMapper.Mapper.Map<Event, EventDTO>(_event);
            var response = new
            {
                status = "success",
                code = "200",
                message = "Ok",
                data = _data
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "CreatorsOnly")]
        public void Delete(int id)
        {
        }
    }
}
