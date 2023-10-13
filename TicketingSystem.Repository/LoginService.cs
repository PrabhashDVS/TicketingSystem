/*
   File: LoginService.cs
   Description: This file contains the implementation of the LoginService class, which handles user login functionality, 
   including NIC, password and user active status validation. It interacts with the user collection in the MongoDB database.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/10  
*/

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;

namespace TicketingSystem.Repository
{
    public class LoginService
    {
        private readonly IMongoCollection<User> _userCollection;

        public LoginService(DatabaseSetting settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UserCollectionName);
        }

        /// <summary>
        /// Authenticate a user based on their NIC (National Identification Card) and password.
        /// </summary>
        /// <param name="nic">The NIC provided by the user for authentication.</param>
        /// <param name="password">The password provided by the user for authentication.</param>
        /// <returns>Returns a user object if authentication is successful; otherwise, throws an exception with an error message.</returns>
        public User Login(string nic, string password)
        {
            try
            {
                User user = new User();

                user = _userCollection.Find(s => s.NIC == nic).SingleOrDefault();
                
                if (user == null)
                {
                    throw new Exception("NIC is Incorrect!");
                }
                else if(!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    throw new Exception("Password is Incorrect!");
                }
                else if (user.ActiveStatus == "Deactive")
                {
                    throw new Exception("User is not Active!");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
