/*
   File: UserService.cs
   Description: This file contains the implementation of the UserService class, which provides methods for managing
   user records, including insertion, activation/deactivation, retrieval, update, and deletion of user accounts.
   It includes password hashing for security and uses AutoMapper for data mapping.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/08  
*/

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

        /// <summary>
        /// Insert a new user into the system, subject to constraints.
        /// </summary>
        /// <param name="user">The user object to be inserted.</param>
        /// <returns>
        /// Returns a success response with a message if the insertion is successful,
        /// a validation response with an error message if the user already exists,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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

        /// <summary>
        /// Activate or deactivate a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to activate or deactivate.</param>
        /// <returns>
        /// Returns a success response with the user's updated status if activation or deactivation is successful,
        /// a validation response with an error message if the user is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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

        /// <summary>
        /// Retrieve a list of all users in the system.
        /// </summary>
        /// <returns>
        /// Returns a success response with the list of users if users are found,
        /// or a validation response with an error message if no users are found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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

        /// <summary>
        /// Retrieve a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// Returns a success response with the user details if the user is found,
        /// a validation response with an error message if the user is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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

        /// <summary>
        /// Update a user's details by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="updatedUser">The updated user details.</param>
        /// <returns>
        /// Returns a success response with the updated user details if the update is successful,
        /// a validation response with an error message if the user is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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

        /// <summary>
        /// Delete a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>
        /// Returns a success response with a message if the deletion is successful,
        /// a validation response with an error message if the user is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
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
