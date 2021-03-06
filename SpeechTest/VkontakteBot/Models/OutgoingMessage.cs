﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest.VkontakteBot.Models
{
    public class OutgoingMessage : IOutgoingMessage
    {
        public string random_id { get { return DateTime.Now.ToFileTimeUtc().ToString(); } }
        public string peer_id { get; set; }
        public string message { get; set; }
        public string attachment { get; set; }
    }
}
