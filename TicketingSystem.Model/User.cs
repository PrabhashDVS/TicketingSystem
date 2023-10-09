using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Model
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string NIC { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool? ActiveStatus { get; set; } 
    }

}
