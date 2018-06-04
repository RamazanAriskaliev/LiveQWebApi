using AutoMapper;
using LiveQ.Api.Helpers;
using LiveQ.Api.Models;
using LiveQ.Api.ViewModels;
using LiveQ.Api.ViewModels.Mappinngs;
using System;

namespace DotNetGigs.ViewModels.Mappinngs
{
    public class DTOToEntityMappingProfile:Profile
    {
        public DTOToEntityMappingProfile()
        {
            CreateMap<RegistrationDTO, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
            CreateMap<CreateEventDTO, Event>()
                .ForMember(r => r.StartDate, opt => opt.MapFrom(s => DateConverter.ToDateTime(s.start)))
                .ForMember(r => r.EndDate, opt => opt.MapFrom(s => DateConverter.ToDateTime(s.end)))
                .ForMember(t => t.Subscribers, opt => opt.Ignore())
                .ForMember(t => t.CreationDate, opt => opt.Ignore())
                .ForMember(t => t.CreatorId, opt => opt.Ignore())
                .ForMember(t => t.Creator, opt => opt.Ignore())
                .ForMember(t => t.Description, opt => opt.MapFrom(s => s.description))
                .ForMember(t => t.TimeLimit, opt => opt.MapFrom(t => TimeSpan.Parse(t.timeLimit)))
                .ForMember(t => t.Title, opt => opt.MapFrom(t => t.title))
                .ForMember(t => t.Id, opt => opt.Ignore());
                
            CreateMap<Event, CreateEventDTO>()
                .ForMember(m => m.id, opt => opt.Ignore())
                .ForMember(r => r.start, opt => opt.MapFrom(s => DateConverter.ToUnixTime(s.StartDate)))
                .ForMember(r => r.end, opt => opt.MapFrom(s => DateConverter.ToUnixTime(s.EndDate)))
                .ForMember(t => t.description, opt => opt.MapFrom(s => s.Description))
                .ForMember(t => t.timeLimit, opt => opt.MapFrom(t => t.TimeLimit))
                .ForMember(t => t.title, opt => opt.MapFrom(t => t.Title));
            CreateMap<Event, EditEventDTO>()
                .ForMember(m => m.id, opt => opt.Ignore())
                .ForMember(t => t.description, opt => opt.MapFrom(s => s.Description))
                .ForMember(t => t.title, opt => opt.MapFrom(t => t.Title));

            CreateMap<EventSubscriber, EventSubscriberDTO>()
                .ForMember(m => m.id, opt => opt.Ignore())
                .ForMember(t => t.start, opt => opt.MapFrom(s => DateConverter.ToUnixTime(s.Start)))
                .ForMember(t => t.end, opt => opt.MapFrom(s => DateConverter.ToUnixTime(s.End)))
                .ForMember(t => t.targetEvent, opt => opt.MapFrom(t => new EventDTO()
                {
                    id = t.Event.Id,
                    title = t.Event.Title,
                    start = DateConverter.ToUnixTime(t.Event.StartDate),
                    end = DateConverter.ToUnixTime(t.Event.EndDate),
                    timeLimit = t.Event.TimeLimit.ToString(),
                    creator = new UserDTO()
                    {
                        id = t.Event.Creator.Id,
                        userName = t.Event.Creator.UserName,
                        firstName = t.Event.Creator.FirstName,
                        lastName = t.Event.Creator.LastName
                    }
                }));
    
            CreateMap<Event, EventDTO>().ConvertUsing<EventDTOConverter>();

            CreateMap<EventSubscriber, EventSubscriberDTO>().ConvertUsing<EventSubscriberDTOConverter>();



        }
    }
}
