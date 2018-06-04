using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveQ.Api.Models
{
    public class EventSubscriber
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [ForeignKey("Subscriber")]
        public string SubscriberId { get; set; }

        public DateTime JoinDate { get; set; } //Время и дата подписки на событие
        public Event Event { get; set; }
        public AppUser Subscriber { get; set; }
        public DateTime Start { get; set; } //Время начала приема
        public DateTime End { get; set; } //Время окончания приема
    }
}