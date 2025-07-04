﻿using System.ComponentModel.DataAnnotations;
using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record UserRequest(
        string Username,
        [EmailAddress] string Email
        );
}
