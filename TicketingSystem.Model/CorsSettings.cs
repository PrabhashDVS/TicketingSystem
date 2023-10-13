/*
   File: CorsSettings.cs
   Description: This file contains the implementation of the CorsSettings class, 
   which represents the allowed origins for Cross-Origin Resource Sharing (CORS) configuration.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/05
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model
{
    public class CorsSettings
    {
        public string AllowedOrigins { get; set; }
    }
}
