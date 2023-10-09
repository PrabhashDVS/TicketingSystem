using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model
{
    [BsonIgnoreExtraElements]
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;        
        public string Number { get; set; } 
        public string Name { get; set; } 
        public Stations Departure { get; set; } = new Stations();
        public DateTime DepartureDate { get; set; } 
        public  List <Stations>Stations { get; set; } = new List<Stations>();
        public Stations Arrival { get; set; } = new Stations();
        public DateTime ArrivalDate { get; set; }
        public int NoOfSeats { get; set; } = 0;
        public string Fare { get; set; } = string.Empty;

    }

    public class Stations
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }


}
