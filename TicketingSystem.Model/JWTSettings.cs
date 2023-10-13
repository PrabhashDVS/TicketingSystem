/*
   File: JWTSettings.cs
   Description: This file contains the implementation of the JWTSettings class,
   which holds configuration settings for JSON Web Token (JWT) authentication, including the issuer, audience, and secret key.
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
    public class JWTSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
    }
}
