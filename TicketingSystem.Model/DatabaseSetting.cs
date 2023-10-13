/*
   File: DatabaseSetting.cs
   Description: This file contains the implementation of the DatabaseSetting class,
   which stores configuration settings related to database collections, connection strings, and database names.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/04
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model
{
    public class DatabaseSetting 
    {
        public string StudentCoursesCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string TrainCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; } 
    }
}
