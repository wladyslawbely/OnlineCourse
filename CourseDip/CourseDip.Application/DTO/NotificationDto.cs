﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class NotificationDto
    {
        public int NotificationId {  get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
