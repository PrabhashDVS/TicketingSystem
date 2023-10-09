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
        public User Login(string nic, string password)
        {
            try
            {
                User user = new User();

                user = _userCollection.Find(s => s.NIC == nic).SingleOrDefault();

                if (user.ActiveStatus == false)
                {
                    throw new Exception("User is not Active!");
                }
                else if (user == null)
                {
                    throw new Exception("NIC is incorrect!");
                }
                else if(!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    throw new Exception("Password is incorrect!");
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
