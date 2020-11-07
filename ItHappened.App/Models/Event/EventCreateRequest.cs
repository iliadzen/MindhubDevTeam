using System.Collections.Generic;

namespace ItHappened.App.Model
{
    public class EventCreateRequest
    {
        public string Title { get; set; }
        public CustomizationsCreateRequests Customizations { get; set; }
    }
}