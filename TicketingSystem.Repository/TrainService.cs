/*
   File: TrainService.cs
   Description: This file contains the implementation of the TrainService class, which provides methods for managing trains,
   including insertion, activation, retrieval, update, and deletion of train records. It interacts with 
   MongoDB collections and handles interactions with reservations to prevent deletion of trains with active reservations.
   Author: Weerasiri R. T. K. , Weerasinghe T. K.(ActiveTrain Implementation)
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/11
*/
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;
using TicketingSystem.Model.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace TicketingSystem.Repository
{
    public class TrainService
    {
        private readonly IMongoCollection<Train> _trainCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;

        public TrainService(DatabaseSetting settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trainCollection = database.GetCollection<Train>(settings.TrainCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(settings.ReservationCollectionName);
        }

        /// <summary>
        /// Insert a new train into the system.
        /// </summary>
        /// <param name="Train">The train object to be inserted.</param>
        /// <returns>
        /// Returns a success response with the inserted train if the insertion is successful,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse InsertTrain(Train Train)
        {
            try
            {                
                _trainCollection.InsertOne(Train);
                return new BaseResponseService().GetSuccessResponse(Train);
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Activate a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to activate.</param>
        /// <returns>
        /// Returns a success response with the activated train if activation is successful,
        /// a validation response with an error message if the train is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse ActiveTrain(string id)
        {
            try
            {
                Train train = new Train();
                train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();

                if (train != null)
                {
                    train.ActiveStatus = "Active";
                    _trainCollection.ReplaceOne(s => s.Id == id, train);

                    return new BaseResponseService().GetSuccessResponse(train);
                }
                return new BaseResponseService().GetValidatationResponse("Train Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Retrieve a list of all trains in the system.
        /// </summary>
        /// <returns>
        /// Returns a success response with the list of trains if trains are found,
        /// or a validation response with an error message if no trains are found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse GetAllTrains()
        {
            try
            {
                List<Train> trainList = new List<Train>();
                trainList = _trainCollection.Find(s => true).ToList();
                if (trainList != null)
                {
                    return new BaseResponseService().GetSuccessResponse(trainList);
                }
                return new BaseResponseService().GetValidatationResponse("Trains Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Retrieve a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to retrieve.</param>
        /// <returns>
        /// Returns a success response with the train if found,
        /// a validation response with an error message if the train is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse GetTrainById(string id)
        {
            try
            {
                Train train = new Train();
                train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();
                if (train != null)
                {
                    return new BaseResponseService().GetSuccessResponse(train);
                }
                return new BaseResponseService().GetValidatationResponse("Train Not Found!");

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Update the details of a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to update.</param>
        /// <param name="updatedTrain">The updated train details, including properties such as number, name, departure, etc.</param>
        /// <returns>
        /// Returns a success response with the updated train if the update is successful,
        /// a validation response with an error message if the train is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse UpdateTrain(string id, Train updatedTrain)
        {
            try
            {
                Train train = new Train();
                train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();
                if (train != null)
                {
                    var filter = Builders<Train>.Filter.Eq(s => s.Id, id);
                    var update = Builders<Train>.Update
                        .Set(s => s.Number, updatedTrain.Number)
                        .Set(s => s.Name, updatedTrain.Name)
                        .Set(s => s.Departure, updatedTrain.Departure)
                        .Set(s => s.DepartureDate, updatedTrain.DepartureDate)
                        .Set(s => s.Stations, updatedTrain.Stations)
                        .Set(s => s.Arrival, updatedTrain.Arrival)
                        .Set(s => s.ArrivalDate, updatedTrain.ArrivalDate)
                        .Set(s => s.NoOfSeats, updatedTrain.NoOfSeats)
                        .Set(s => s.Fare, updatedTrain.Fare);

                    _trainCollection.UpdateOne(filter, update);
                    return new BaseResponseService().GetSuccessResponse(updatedTrain);
                }
                return new BaseResponseService().GetValidatationResponse("Train Not Found!");
                
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        /// <summary>
        /// Delete a train by its unique identifier, subject to specific constraints.
        /// </summary>
        /// <param name="id">The unique identifier of the train to delete.</param>
        /// <returns>
        /// Returns a success response with a message if the deletion is successful,
        /// a validation response with an error message if the train cannot be deleted due to active reservations,
        /// a validation response with an error message if the train is not found,
        /// or an error response with details of any exception that occurs during the process.
        /// </returns>
        public BaseResponse DeleteTrain(string id)
        {
            try
            {
                List <Reservation> reservationList = new List<Reservation>();
                reservationList = _reservationCollection.Find(_ => true).ToList();
                Train train = new Train();
                train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();
                if (train != null)
                {
                    foreach(var reservation in reservationList)
                    {
                        var checkTrainId = reservation.TrainId;
                        if(checkTrainId == train.Id)
                        {
                            return new BaseResponseService().GetValidatationResponse("Cannot Cancel Train Since Train has Active Reservation!");
                        }
                    }
                    var filter = Builders<Train>.Filter.Eq(s => s.Id, id);
                    _trainCollection.DeleteOne(filter);
                    return new BaseResponseService().GetSuccessResponse("The Train Deleted Successfully!");
                }
                return new BaseResponseService().GetValidatationResponse("Train Not Found!");                

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

    }
}
