using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model
{
    public class DatabaseSetting 
    {
        public string StudentCoursesCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string TrainCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; } 
    }
}
