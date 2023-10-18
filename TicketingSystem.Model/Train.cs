/*
   File: Train.cs
   Description: This file contains the implementation of the Train class, which represents a train entity
   with various properties, including ID, number, name, departure information, stations, arrival information,
   seat count, fare, and active status. Additionally, the file defines the Station class used within the Train class and
   required other classes.
   Author: Weerasiri R. T. K. , Weerasinghe T. K. (add active status)
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/11
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
    public class Train 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;        
        public string Number { get; set; } 
        public string Name { get; set; } 
        public Station Departure { get; set; } = new Station();
        public DateTime DepartureDate { get; set; } 
        public  List <Station>Stations { get; set; } = new List<Station>();
        public Station Arrival { get; set; } = new Station();
        public DateTime ArrivalDate { get; set; }
        public int NoOfSeats { get; set; } = 0;
        public int NoOfAvailableSeats { get; set; } = 0;
        public double Fare { get; set; } = 0;
        public string ActiveStatus { get; set; } = string.Empty;

    }
        
    public class Station
    {
        public string Label { get; set; } 
        public string Value { get; set; } 
    }


}
