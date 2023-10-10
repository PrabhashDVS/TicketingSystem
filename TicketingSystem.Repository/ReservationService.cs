    using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;

namespace TicketingSystem.Repository
{
    public class ReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Train> _trainCollection;

        public ReservationService(DatabaseSetting settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _userCollection = database.GetCollection<User>(settings.UserCollectionName);
            _trainCollection = database.GetCollection<Train>(settings.TrainCollectionName);

        }

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
                    _reservationCollection.InsertOne(reservation);
                    return new BaseResponseService().GetSuccessResponse(reservation);
                }
                return new BaseResponseService().GetValidatationResponse("You Reached Maximum Reserverion Can Made!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }
    
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

                        var resObject = new { reservation, user, train };
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
                    var resObject = new { reservation = reservation, user = user, train = train };
                    return new BaseResponseService().GetSuccessResponse(resObject);
                }
                return new BaseResponseService().GetValidatationResponse("Reservation Not Found!");
            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

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

                            var resObject = new { reservation, user, train };
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


        public BaseResponse UpdateReservation(string id, Reservation updatedReservation)
        {
            try
            {
                Reservation reservation = new Reservation();
                reservation = _reservationCollection.Find(s => s.Id == id).SingleOrDefault();
                if (reservation != null)
                {
                    var filter = Builders<Reservation>.Filter.Eq(s => s.Id, id);
                    var update = Builders<Reservation>.Update.Set(s => s.Status, updatedReservation.Status);

                    _reservationCollection.UpdateOne(filter, update);
                    return new BaseResponseService().GetSuccessResponse(updatedReservation);
                }
                return new BaseResponseService().GetValidatationResponse("Reservation Not Found!");

            }
            catch (Exception ex)
            {
                return new BaseResponseService().GetErrorResponse(ex);
            }

        }

        public BaseResponse DeleteReservation(string id)
        {
            try
            {
                Reservation reservation = new Reservation();
                reservation = _reservationCollection.Find(s => s.Id == id).SingleOrDefault();
                if (reservation != null)
                {
                    var filter = Builders<Reservation>.Filter.Eq(s => s.Id, id);
                    _reservationCollection.DeleteOne(filter);
                    return new BaseResponseService().GetSuccessResponse("Reservation Deleted Successfully");
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
