/*
   File: Reservation.cs
   Description: This file contains the implementation of the Reservation class, 
   which represents a reservation entity with various properties, including ID, train and user IDs, status, 
   stations, price, reservation date, and the number of reserved seats.
   Author: Piyumantha H. P. A. H.
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/10
*/

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
        public Station From { get; set; } = new Station();
        public Station To { get; set; } = new Station();
        public double Price { get; set; } = 0;
        public DateTime ReservationDate { get; set; } = DateTime.MinValue;
        public int NoOfReservedSeats { get; set; } = 0;

    }

}
