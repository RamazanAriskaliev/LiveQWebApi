using LiveQ.Api.Models;
using LiveQ.Api.Services;
using LiveQ.Api.ViewModels;
using LiveQ.Api.ViewModels.Validations.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiveQ.Api.Controllers
{
    [Authorize(Policy = "SubscribersOnly")]
    [Route("api/[controller]")]
    [ValidateModel]
    public class SubscriptionController : Controller
    {
        private readonly IEventSubscriptionService _subscriptionService;
        private readonly IEventService _eventService;
        private readonly UserManager<AppUser> _userManager;
        private readonly JsonSerializerSettings _serializerSettings;

        public SubscriptionController(IEventSubscriptionService subscriptionService, IEventService eventService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _subscriptionService = subscriptionService;
            _eventService = eventService;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet("{start}/{end}")]
        public async Task<IActionResult> Get(string start, string end)
        {
            String userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            var events = _subscriptionService.GetSubscriptions(start, end, user.Id);
            var _data = AutoMapper.Mapper.Map<IEnumerable<EventSubscriber>, IEnumerable<EventSubscriberDTO>>(events);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id)
        {
            Event _event = _eventService.GetEvent(id);
            if (_event == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1000",
                    message = "User with such 'id' is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            string userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            if (_eventService.IsSubscriber(_event, user))
            {
                var result = new
                {
                    status = "error",
                    code = "3000",
                    message = "You can't subscribe to the same event twice!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            if (_eventService.IsPastDue(_event))
            {
                var result = new
                {
                    status = "error",
                    code = "3001",
                    message = "You can not make an appointment for an event that has already passed!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            EventSubscriber subscription = _subscriptionService.InitializeSubscription(_event, user);
            var status = await _subscriptionService.CreateSubscriptionAsync(subscription, user);
            if (!status)
            {
                return new StatusCodeResult(500);
            }
            var _data = AutoMapper.Mapper.Map<EventSubscriberDTO>(subscription);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            EventSubscriber es = _subscriptionService.GetSubscription(id);
            if (es == null)
            {
                var result = new
                {
                    status = "error",
                    code = "1000",
                    message = "Subscription with such 'id' is not exist!"
                };
                var error = JsonConvert.SerializeObject(result, _serializerSettings);
                return new OkObjectResult(error);
            }
            var status = _subscriptionService.DeleteSubscriptionAsync(es);
            if (!status)
            {
                return new StatusCodeResult(500);
            }
            var response = new
            {
                status = "success",
                code = "200",
                message = "Ok"
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var es = _subscriptionService.GetSubscription(id);
            if (es == null)
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
            var _data = AutoMapper.Mapper.Map<EventSubscriber, EventSubscriberDTO>(es);
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
    }
}
