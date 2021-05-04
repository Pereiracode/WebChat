using System;
using System.Collections.Generic;
using System.Text;

namespace WebChat.Services.Interfaces
{
    public interface IProfanityFilter
    {
        public string Filter(string text);
    }
}
