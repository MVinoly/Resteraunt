using Resteraunt.DTOs;
using Resteraunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Resteraunt.Profiles
{
    public class MenuItemProfile: Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemCreateDto>();
            CreateMap<MenuItemCreateDto, MenuItem>();
        }
    }
}
