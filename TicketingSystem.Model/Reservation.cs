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
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainId { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = String.Empty;
        public string Status { get; set; }  = String.Empty;
        public string From { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public double Price { get; set; } = 0;

    }
}
