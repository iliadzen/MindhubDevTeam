using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class TrackerCreateRequest
    {
        public string Title { get; set; }
        public string Customizations { get; set; }
    }
}