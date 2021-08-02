using AutoMapper;
using Resteraunt.DTOs;
using Resteraunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resteraunt.Profiles
{
    public class ReservationProfile: Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationCreateDto>();
            CreateMap<ReservationCreateDto, Reservation>();
        }
    }
}
