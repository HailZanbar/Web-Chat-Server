﻿using Microsoft.AspNetCore.Mvc;
using HiooshServer.Services;
using HiooshServer.Models;

namespace HiooshServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService;
        }

        // return the contacts of the user
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // need to fix the async
            return Json(_contactsService.GetAllContacts());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
           if (!ModelState.IsValid)
            {
                _contactsService.AddContact(contact);
                return Created(string.Format("/api/contacts/{0}", contact.Id), contact);
            }
           return BadRequest();
        }

        [HttpPost("{id")]
        public async Task<IActionResult> Edit(string id, string nickname, string image, List<Message> chat)
        {
            if (ModelState.IsValid)
            {
                _contactsService.UpdateContact(id, nickname, image, chat);
                return NoContent();

            }
            return BadRequest();
        }

        [HttpDelete("{id")]
        public Task<IActionResult> Delete(string id)
        {
            _contactsService.RemoveContact(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public Contact Get(string id)
        {
            return _contactsService.GetContact(id);
        }
    }
}
