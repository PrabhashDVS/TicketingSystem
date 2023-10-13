/*
   File: ReservationService.cs
   Description: This file contains the implementation of the ReservationService class, 
   which provides various methods for handling reservations, including insertion, retrieval, update, and deletion.
   It interacts with MongoDB collections and uses a Mapper for mapping data objects.
   Author: Piyumantha H. P. A. H.
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/10
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
    public class ReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Train> _trainCollection;
        private readonly IMapper _mapper;

        public ReservationService(DatabaseSetting settings, IMongoClient mongoClient, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _userCollection = database.GetCollection<User>(settings.UserCollectionName);
            _trainCollection = database.GetCollection<Train>(settings.TrainCollectionName);
            _mapper = mapper;
        }

        /// <summary>
        /// Insert a reservation into the system after applying certain validation rules.
        /// </summary>
        /// <param name="reservation">The reservation to be inserted.</param>
        /// <returns>
        /// Returns a success response with the inserted reservation if successful, or
        /// a validation response with an error message if validation conditions are not met,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse InsertReservation(Reservation reservation)
        {
            try
            {
                var reservationsList = _reservationCollection.Find( _ => true).ToList();
                var reservationListByUser = new List<object>();
                foreach (var res in reservationsList)
                {
                    if(res.ReservationDate > DateTime.Today)
                    {
                        reservationListByUser.Add(res);
                    }
                }
                if(reservationListByUser.Count < 5)
                {
                    TimeSpan difference = reservation.ReservationDate - DateTime.Today;
                    int days = difference.Days;
                    if (days < 30)
                    {
                        _reservationCollection.InsertOne(reservation);
                        return new BaseResponseService().GetSuccessResponse(reservation);
                    }
                    return new BaseResponseService().GetValidatationResponse("Reservation Date should be 30 days from the Booking Date!");
                }
                return new BaseResponseService().GetValidatationResponse("You Reached Maximum Reserverion Can Made!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Retrieve a list of all reservations along with associated user and train details.
        /// </summary>
        /// <returns>
        /// Returns a success response with the list of reservations, including associated user and train information, if reservations are found,
        /// or a validation response with an error message if no reservations are found, or
        /// an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse GetAllReservations()
        {
            try
            {
                var reservations = _reservationCollection.Find( _ => true).ToList();
                if (reservations != null)
                {
                    var reservationList = new List<object>();

                    foreach (var reservation in reservations)
                    {
                        User user = _userCollection.Find(u => u.Id == reservation.UserId).SingleOrDefault();
                        Train train = _trainCollection.Find(t => t.Id == reservation.TrainId).SingleOrDefault();
                        UserMapVM userDetails = _mapper.Map<UserMapVM>(user);
                        var resObject = new { reservation, userDetails, train };
                        reservationList.Add(resObject);
                    }
                    return new BaseResponseService().GetSuccessResponse(reservationList);
                }
                return new BaseResponseService().GetValidatationResponse("Reservations Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }
        }

        /// <summary>
        /// Retrieve a reservation by its unique identifier along with associated user and train details.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to retrieve.</param>
        /// <returns>
        /// Returns a success response with the reservation, including associated user and train information, if found,
        /// or a validation response with an error message if the reservation is not found, or
        /// an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse GetReservationById(string id)
        {
            try
            {
                Reservation reservation = new Reservation();
                User user = new User();
                Train train = new Train();
                reservation = _reservationCollection.Find(s => s.Id == id).SingleOrDefault();
                if(reservation != null)
                {
                    user = _userCollection.Find(t => t.Id == reservation.UserId).SingleOrDefault();
                    train = _trainCollection.Find(t => t.Id == reservation.TrainId).SingleOrDefault();
                    UserMapVM userDetails = _mapper.Map<UserMapVM>(user);
                    var resObject = new { reservation = reservation, user = userDetails, train = train };
                    return new BaseResponseService().GetSuccessResponse(resObject);
                }
                return new BaseResponseService().GetValidatationResponse("Reservation Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Retrieve a list of reservations associated with a specific user by their unique identifier, along with associated user and train details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom reservations are to be retrieved.</param>
        /// <returns>
        /// Returns a success response with a list of reservations, including associated user and train information, if reservations are found,
        /// or a validation response with an error message if the user is not found or if the user has no reservations,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse GetReservationsByUserId(string userId)
        {
            try
            {
                var reservations = _reservationCollection.Find(r => r.UserId == userId).ToList();
                if(reservations != null)
                {
                    if(reservations[0].UserId != null)
                    {
                        var reservationList = new List<object>();

                        foreach (var reservation in reservations)
                        {
                            User user = _userCollection.Find(u => u.Id == reservation.UserId).SingleOrDefault();
                            Train train = _trainCollection.Find(t => t.Id == reservation.TrainId).SingleOrDefault();
                            UserMapVM userDetails = _mapper.Map<UserMapVM>(user);
                            var resObject = new { reservation, userDetails, train };
                            reservationList.Add(resObject);
                        }

                        return new BaseResponseService().GetSuccessResponse(reservationList);
                    }
                    return new BaseResponseService().GetValidatationResponse("User Not Found!");
                }
                return new BaseResponseService().GetValidatationResponse("User dose not have any Reservation!");

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }
        }

        /// <summary>
        /// Update the status of a reservation by its unique identifier, subject to specific constraints.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to update.</param>
        /// <param name="updatedReservation">The updated reservation details, including the new status.</param>
        /// <returns>
        /// Returns a success response with the updated reservation if the update is successful,
        /// a validation response with an error message if the reservation cannot be updated due to time constraints,
        /// or a validation response with an error message if the reservation is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse UpdateReservation(string id, Reservation updatedReservation)
        {
            try
            {
                Reservation reservation = new Reservation();
                reservation = _reservationCollection.Find(s => s.Id == id).SingleOrDefault();
                if (reservation != null)
                {
                    // calculate the date differnce in between reservation date current date
                    TimeSpan difference = reservation.ReservationDate - DateTime.Today;
                    int daysDifference = difference.Days;

                    if (daysDifference >= 5)
                    {
                        var filter = Builders<Reservation>.Filter.Eq(s => s.Id, id);
                        var update = Builders<Reservation>.Update.Set(s => s.Status, updatedReservation.Status);

                        _reservationCollection.UpdateOne(filter, update);
                        return new BaseResponseService().GetSuccessResponse(updatedReservation);
                    }
                    return new BaseResponseService().GetValidatationResponse("You Can Only Update Reservation at least 5 Days Before the Reservation!");
                }
                return new BaseResponseService().GetValidatationResponse("Reservation Not Found!");

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Delete a reservation by its unique identifier, subject to specific constraints.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to delete.</param>
        /// <returns>
        /// Returns a success response with a message if the deletion is successful,
        /// a validation response with an error message if the reservation cannot be deleted due to time constraints,
        /// or a validation response with an error message if the reservation is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse DeleteReservation(string id)
        {
            try
            {
                Reservation reservation = new Reservation();
                reservation = _reservationCollection.Find(s => s.Id == id).SingleOrDefault();
                if (reservation != null)
                {
                    // calculate the date differnce in between reservation date current date
                    TimeSpan difference = reservation.ReservationDate - DateTime.Today;
                    int daysDifference = difference.Days;
                    if(daysDifference >= 5)
                    {
                        var filter = Builders<Reservation>.Filter.Eq(s => s.Id, id);
                        _reservationCollection.DeleteOne(filter);
                        return new BaseResponseService().GetSuccessResponse("Reservation Canceling Successfully");
                    }
                    return new BaseResponseService().GetValidatationResponse("You Can Only Cancel Reservation at least 5 Days Before the Reservation!");
                }
                return new BaseResponseService().GetValidatationResponse("Reservation Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

       
    }
}
