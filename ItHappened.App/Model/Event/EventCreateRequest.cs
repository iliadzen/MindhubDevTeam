using System.Collections.Generic;

namespace ItHappened.App.Model
{
    public class EventCreateRequest
    {
        public string Title { get; set; }
        public List<ICustomizationCreateRequest> Customizations { get; set; }
    }
}