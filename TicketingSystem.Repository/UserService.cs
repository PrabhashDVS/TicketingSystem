using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;
using TicketingSystem.Model.ViewModels;

namespace TicketingSystem.Repository
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMapper _mapper;

        public UserService(DatabaseSetting settings, IMongoClient mongoClient, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UserCollectionName);
            _mapper = mapper;
        }

        public BaseResponse InsertUser(User user)
        {
            try
            {
                User existingUser = new User();
                existingUser = _userCollection.Find(s => s.NIC == user.NIC).SingleOrDefault();
                if (existingUser == null)
                {
                    string plainTextPassword = user.Password;
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
                    user.Password = hashedPassword;
                    _userCollection.InsertOne(user);
                    return new BaseResponseService().GetSuccessResponse("User Created Successfully");
                }

                    return new BaseResponseService().GetValidatationResponse("User is Already Exists!"); 

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse ActiveDeactiveUser(string id)
        {
            try
            {
                User user = new User();
                user = _userCollection.Find(s => s.Id == id).SingleOrDefault();

                if (user != null)
                {
                    user.ActiveStatus = (user.ActiveStatus == "Active") ? "Deactive" : "Active";
                    _userCollection.ReplaceOne(s => s.Id == id, user);
                    UserMapVM userMapVm = _mapper.Map<UserMapVM>(user);

                    return new BaseResponseService().GetSuccessResponse(userMapVm);
                }

                return new BaseResponseService().GetValidatationResponse("User Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse GetAllUsers()
        {
            try
            {
                List<User> userList = new List<User>();
                userList = _userCollection.Find(s => true).ToList();
                if(userList != null)
                {
                    List<UserMapVM> userMapList = _mapper.Map<List<UserMapVM>>(userList);
                    return new BaseResponseService().GetSuccessResponse(userMapList);
                }
                return new BaseResponseService().GetValidatationResponse("Users Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse GetUserById(string id)
        {  
            try
            {                
                User user = new User();
                user = _userCollection.Find(s => s.Id == id).SingleOrDefault();
                UserMapVM userMapVm = _mapper.Map<UserMapVM>(user);
                if (user != null)
                {
                    return new BaseResponseService().GetSuccessResponse(userMapVm);
                }
                return new BaseResponseService().GetValidatationResponse("User Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse UpdateUser(string id, UserVM updatedUser)
        {
            try
            {
                User user = new User();
                user = _userCollection.Find(s => s.Id == id).SingleOrDefault();
                if (user != null)
                {
                    var filter = Builders<User>.Filter.Eq(s => s.Id, id);
                    var update = Builders<User>.Update
                        .Set(s => s.FirstName, updatedUser.FirstName)
                        .Set(s => s.LastName, updatedUser.LastName)
                        .Set(s => s.Email, updatedUser.Email)
                        .Set(s => s.ContactNo, updatedUser.ContactNo)
                        .Set(s => s.DOB, updatedUser.DOB)
                        .Set(s => s.Address, updatedUser.Address)
                        .Set(s => s.Gender, updatedUser.Gender);

                    _userCollection.UpdateOne(filter, update);
                    return new BaseResponseService().GetSuccessResponse(updatedUser);
                }
                return new BaseResponseService().GetValidatationResponse("User Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse DeleteUser(string id)
        {
            try
            {
                User user = new User();
                user = _userCollection.Find(s => s.Id == id).SingleOrDefault();
                if (user != null)
                {
                    var filter = Builders<User>.Filter.Eq(s => s.Id, id);
                    _userCollection.DeleteOne(filter);
                    return new BaseResponseService().GetSuccessResponse("User Deleted Successfully!");
                }
                return new BaseResponseService().GetValidatationResponse("User Not Found!");

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

    }
}
