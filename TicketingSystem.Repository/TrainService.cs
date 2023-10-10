using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;
using static System.Collections.Specialized.BitVector32;

namespace TicketingSystem.Repository
{
    public class TrainService
    {
        private readonly IMongoCollection<Train> _trainCollection;

        public TrainService(DatabaseSetting settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trainCollection = database.GetCollection<Train>(settings.TrainCollectionName);
        }

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

        public BaseResponse DeleteTrain(string id)
        {
            try
            {

                Train train = new Train();
                train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();
                if (train != null)
                {
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
