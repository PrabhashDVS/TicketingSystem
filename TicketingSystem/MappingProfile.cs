/*
   File: MappingProfile.cs
   Description: This file contains the implementation of the MappingProfile class, 
   which defines AutoMapper mappings between different models and view models.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/10  
*/

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TicketingSystem.Model.ViewModels;

namespace TicketingSystem.Model
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserMapVM>();
        }
    }


}
