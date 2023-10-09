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
                List<Train> TrainList = new List<Train>();
                TrainList = _trainCollection.Find(s => true).ToList();
                return new BaseResponseService().GetSuccessResponse(TrainList);
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
                Train Train = new Train();
                Train = _trainCollection.Find(s => s.Id == id).SingleOrDefault();
                return new BaseResponseService().GetSuccessResponse(Train);
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
                var filter = Builders<Train>.Filter.Eq(s => s.Id, id);
                var update = Builders<Train>.Update
                    .Set(s => s.Number, updatedTrain.Number)
                    .Set(s => s.Name, updatedTrain.Name)
                    .Set(s => s.Departure, updatedTrain.Departure)
                    .Set(s => s.DepartureDate, updatedTrain.DepartureDate)
                   
                    .Set(s => s.Arrival, updatedTrain.Arrival)
                    .Set(s => s.ArrivalDate, updatedTrain.ArrivalDate)
                    .Set(s => s.NoOfSeats, updatedTrain.NoOfSeats)
                    .Set(s => s.Fare, updatedTrain.Fare);

                _trainCollection.UpdateOne(filter, update);
                return new BaseResponseService().GetSuccessResponse(updatedTrain);
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
                var filter = Builders<Train>.Filter.Eq(s => s.Id, id);
                _trainCollection.DeleteOne(filter);
                return new BaseResponseService().GetSuccessResponse(filter);

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

    }
}
