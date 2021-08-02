using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Resteraunt.Data;
using Resteraunt.DTOs;
using Resteraunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resteraunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReservationController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ReservationController>
        [HttpGet]
        public ActionResult Get()
        {
            /* Method 1
             * 
            var reservations = _context.Reservations.ToList();
            var menuItem = _context.MenuItems.ToList();
            var reservationMenus = _context.ReservationMenus.ToList();

            foreach(var reservation in reservations)
            {
                List<MenuItem> menuItemsToAdd = new List<MenuItem>();

                foreach(var rm in reservationMenus )
                {
                    if(rm.ReservationId == reservation.Id)
                    {
                        MenuItem mi = menuItem
                            .FirstOrDefault(mi => mi.Id == rm.MenuItemId);
                        menuItemsToAdd.Add(mi);
                    }
                }
                reservation.MenuItems = menuItemsToAdd;
            }
            */

            // Method 2 with Linq
            var reservations = _context
                .Reservations
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Name = r.Name,
                    Date = r.Date,
                    MenuItems = _context.ReservationMenus
                        .Where(rm => rm.ReservationId == r.Id)
                        .Select(mi => new MenuItem
                        {
                            Id = mi.MenuItem.Id,
                            Name = mi.MenuItem.Name,
                            Price = mi.MenuItem.Price
                        }).ToList()
                }).ToList();

            return Ok(reservations);
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            // Method 1
            //Reservation reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            //if (reservation == null)
            //{
            //    return NotFound($"Order with id : {id} doesn't exist");
            //}

            //List<MenuItem> menuItemsToAdd = new List<MenuItem>();

            //foreach (var rm in _context.ReservationMenus.ToList())
            //{
            //    if (rm.ReservationId == reservation.Id)
            //    {
            //        MenuItem mi = _context
            //            .MenuItems
            //            .FirstOrDefault(mi => mi.Id == rm.MenuItemId);

            //        menuItemsToAdd.Add(mi);
            //    }
            //}

            //reservation.MenuItems = menuItemsToAdd;

            // Method 2
            var reservation = _context.Reservations.Where(r => r.Id == id)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Name = r.Name,
                    Date = r.Date,
                    MenuItems = _context.ReservationMenus
                        .Where(rm => rm.ReservationId == r.Id)
                        .Select(mi => new MenuItem
                        {
                            Id = mi.MenuItem.Id,
                            Name = mi.MenuItem.Name,
                            Price = mi.MenuItem.Price
                        }).ToList()
                }).FirstOrDefault();

            if (reservation == null) return NotFound($"M2: Order with id : {id} doesn't exist");

            return Ok(reservation);
        }

        // POST api/<ReservationController>
        [HttpPost]
        public ActionResult Post(ReservationCreateDto value)
        {
            Reservation newReservation = _mapper.Map<Reservation>(value);
            _context.Reservations.Add(newReservation);
            _context.SaveChanges();

            foreach(int menuItemId in value.MenuItemIds)
            {
                var rm = new ReservationMenu
                {
                    ReservationId = newReservation.Id,
                    MenuItemId = menuItemId
                };

                _context.ReservationMenus.Add(rm);
            }
            _context.SaveChanges();

            return Ok();
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, ReservationCreateDto value)
        {
            var reservationFromDb = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservationFromDb == null) return NotFound();

            _mapper.Map(value, reservationFromDb);

            var menuItemsRange = _context
                .ReservationMenus
                .Where(rm => rm.ReservationId == id)
                .ToList();

            _context.ReservationMenus.RemoveRange(menuItemsRange);

            foreach(var menuItemId in value.MenuItemIds)
            {
                var rmNew = new ReservationMenu
                {
                    ReservationId = id,
                    MenuItemId = menuItemId
                };

                _context.ReservationMenus.Add(rmNew);
            }

            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Reservation reservationToDelete = _context.Reservations.Find(id);

            if (reservationToDelete == null) return NotFound();

            var menuItemsRange = _context
                .ReservationMenus
                .Where(rm => rm.ReservationId == id)
                .ToList();

            _context.ReservationMenus.RemoveRange(menuItemsRange);
            _context.Reservations.Remove(reservationToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}
