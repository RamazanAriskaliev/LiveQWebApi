using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveQ.Api.Models
{
    public class Event
    {
        public Event()
        {
            this.Subscribers = new HashSet<EventSubscriber>();
        }
    
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        
        public string Description { get; set; } // Описание события

        public DateTime StartDate { get; set; } //Дата и Время начала события
        public DateTime EndDate { get; set; } //Дата и Время окончания события
        public DateTime CreationDate { get; set; } // Дата создания 
        public TimeSpan TimeLimit { get; set; } //Время которое дается каждому подписчику

        [ForeignKey("Creator")]
        public string CreatorId { get; set; } //Вторичный ключ на создателя события

        public ICollection<EventSubscriber> Subscribers{ get; set; } 
        public AppUser Creator { get; set; } //Создатель события

        //Проверка времени события, если время начала события прошло то подписаться нельзя
        public bool IsPastDue()
        {
            var temp = DateTime.Compare(this.StartDate, DateTime.Now);
            if (temp <= 0)
            {
                return false;
            }
            return true;
        }

    }
}