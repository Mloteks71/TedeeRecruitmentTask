using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tedee.Controllers.Trips.Dto;
using Tedee.ExceptionHandling.Exceptions;
using Tedee.Models;
using Tedee.Models.Enums;
using Tedee.Models.Validators;
using Tedee.Repositories.Interfaces;

namespace Tedee.Controllers.Trips
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly TedeeContext _context;
        private readonly ITripRepository _tripRepository;

        public TripsController(TedeeContext context, ITripRepository tripRepository)
        {
            _context = context;
            _tripRepository = tripRepository;
        }

        // GET: api/Trips
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripNoDescription>))]
        public async Task<ActionResult<IEnumerable<TripNoDescription>>> GetTrips()
        {
            return await _tripRepository.GetTrips().Select(x => new TripNoDescription(x)).ToListAsync();
        }

        // GET: api/Trips/country/2
        [HttpGet("country/{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripNoDescription>))]
        public async Task<ActionResult<IEnumerable<TripNoDescription>>> GetTripsByCountry([FromRoute] int countryId)
        {
            return await _tripRepository.GetTripsWithCountryId(countryId).Select(x => new TripNoDescription(x)).ToListAsync();
        }

        // GET: api/Trips/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripNoDescription))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripNoDescription>> GetTrip([FromRoute] Guid id)
        {
            if (!_tripRepository.TripExists(id))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided Id does not exist");
            }

            return new TripNoDescription(await _tripRepository.GetTrip(id).SingleAsync());
        }

        // PUT: api/Trips/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditTrip([FromRoute] Guid id, BaseTrip trip)
        {
            if (!_tripRepository.TripExists(id))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided Id does not exist");
            }

            var editedTrip = new Trip(trip);
            editedTrip.Id = id;

            _context.Entry(editedTrip).State = EntityState.Modified;

            return NoContent();
        }

        // POST: api/Trips
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Trip))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseTrip>> AddTrip(BaseTrip trip)
        {
            if (!_tripRepository.TripByNameExists(trip.Name))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided name already exists");
            }

            var newTrip = new Trip(trip);
            _context.Trips.Add(newTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AddTrip", new { id = newTrip.Id }, newTrip);
        }

        // DELETE: api/Trips/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTrip([FromRoute] Guid id)
        {
            if (!_tripRepository.TripExists(id))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided Id does not exist");
            }

            var trip = await _tripRepository.GetTrip(id).SingleAsync();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Trips/{tripId}/Register
        [HttpPost("{tripId}/Register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisteredEmail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegisteredEmail>> RegisterForTrip([FromRoute] Guid tripId, BaseRegisteredEmail email)
        {
            if (!_tripRepository.TripExists(tripId))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided Id does not exist");
            }

            var trip = await _tripRepository.GetTripWithEmails(tripId).SingleAsync();

            if (trip.Emails.Any(x => x.Email == email.Email))
            {
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "Provided email is already registered for this trip");
            }

            RegisteredEmail registeredEmail;

            if (_context.RegisteredEmails.Any(x => x.Email == email.Email))
            {
                registeredEmail = await _context.RegisteredEmails.FirstAsync(x => x.Email == email.Email);
            }
            else
            {
                registeredEmail = new(email);
            }

            trip.Emails.Add(registeredEmail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("RegisterForTrip", registeredEmail.Id, registeredEmail);
        }


        // POST: api/Trips/{tripId}/Unregister
        [HttpPost("{tripId}/Unregister")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseTrip>> UnregisterForTrip([FromRoute] Guid tripId, BaseRegisteredEmail email)
        {
            if (!_tripRepository.TripExists(tripId))
            {
                throw new HttpResponseException(StatusCodes.Status404NotFound, "Trip with provided Id does not exist");
            }

            var trip = await _tripRepository.GetTripWithEmails(tripId).SingleAsync();

            if (!trip.Emails.Any(x => x.Email == email.Email))
            {
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "Provided email is not registered for this trip");
            }

            var emailToDelete = trip.Emails.First(x => x.Email == email.Email);
            trip.Emails.Remove(emailToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
