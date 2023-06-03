using AwesomeDevEvents.API.Entities;

namespace AwesomeDevEvents.API.Persistence
{
    public class DevEventsDbContext
    {
        public DevEventsDbContext()
        {
            DevEvents = new List<DevEvent>();
        }

        public List<DevEvent> DevEvents { get; set; }
    }
}
