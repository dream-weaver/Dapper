using System;
using System.Collections.Generic;
using Dapper.Entities;
using Dapper.Entity;
using Dapper.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.Web.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IContactContribRepository repository;

        public AddressesController(IContactContribRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/Addresses
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> Get()
        {
            try
            {
                var contacts = repository.GetAllContacts();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get contacts ");
            }
           
        }

        // GET: api/Addresses/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Contact> Get(int id)
        {
            try
            {
                return Ok(repository.GetFullContact(id));
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get contact by id");
            }
        }

        // POST: api/Addresses
        [HttpPost]
        public IActionResult Post([FromBody] Contact contact)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");
                repository.Save(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save contacts");
            }
        }

        // PUT: api/Addresses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
