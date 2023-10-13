/*
   File: UserMapVM.cs
   Description: This file contains the implementation of the UserMapVM class, 
   which represents a view model for user data with various properties, including
   ID, NIC,first name, last name, email, contact number, date of birth, address, gender, role, and active status.
   this is use for get response without sensetive data.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/10  
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model.ViewModels
{

    public class UserMapVM
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
        public string Role { get; set; } = string.Empty;
        public string ActiveStatus { get; set; } = string.Empty;


    }
}
