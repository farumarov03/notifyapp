﻿using System;
namespace CardService.src.Models
{
    public class EventModel
    {
        public string OrderType { get; set; }
        public string SessionId { get; set; }
        public DateTime EventDate { get; set; }
        public string WebsiteUrl { get; set; }
    }
}

