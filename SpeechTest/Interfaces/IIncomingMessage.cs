﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest
{
    public interface IIncomingMessage
    {
        string peer_id { get; }
        string MessageType { get; }

        string text { get; }
        List<dynamic> attachments { get; }

    }
  
}
