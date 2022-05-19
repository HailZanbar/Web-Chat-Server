﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        [HttpGet("{user}")]
        public IActionResult Index(string user)
        {
            if (_contactsService.GetUser(user) == null)
            {
                // need to take care to return view that user is not login
                return BadRequest();
            }
            return Json(_contactsService.GetAllContacts(user));
        }

        [HttpPost]
        public IActionResult Create([FromBody] JsonElement fields)
        {
          if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                // get the fields from the body request
                string id = fields.GetProperty("id").ToString();
                string name = fields.GetProperty("name").ToString();
                string server = fields.GetProperty("server").ToString();

                Contact contact = new Contact(id, name, server);
                _contactsService.AddContact(fields.GetProperty("userid").ToString(), contact);
                return Created(string.Format("/api/contacts/{0}", contact.id), contact);
            }
            // need to take care to return view that user is not login
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, [FromBody] JsonElement fields)
        {
            if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                // get the name and the server fields from the body request
                string name = fields.GetProperty("name").ToString();
                string server = fields.GetProperty("server").ToString();

                _contactsService.UpdateContact(fields.GetProperty("userid").ToString(), id, name, server);
                return NoContent();

            }
            // need to take care to return view that user is not login
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id, [FromBody] JsonElement fields)
        {
            if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                _contactsService.RemoveContact(fields.GetProperty("userid").ToString(), id);
                return NoContent();
            }
            // if not login
            return BadRequest();
        }

        //[Route("api/contacts/{user}/{id}")]
        [HttpGet("{user}/{id}")]
        public IActionResult Get(string user, string id)
        {
            if (_contactsService.GetUser(user) != null)
            {
                Contact? contact = _contactsService.GetContact(user, id);
                if (contact != null)
                {
                    return Json(contact);
                }
            }
            // need to take care the view if not login
            return NotFound();
        }

        // need to check how to add the "messages" to the url
        //[Route("api/contacts/{user}/{id}/messages")]
        [HttpGet("{user}/{id}/messages")]
        public IActionResult GetMessages(string user, string id)
        {
            if (_contactsService.GetUser(user) != null)
            {
                Contact? contact = _contactsService.GetContact(user, id);
                if (contact != null)
                {
                    return Json(_contactsService.GetMessages(user, id));
                }
            }
            // need to take care the part of view if not login
            return BadRequest();
        }

      
        [HttpPost("{id}/messages")]
        public IActionResult AddMessage(string id , [FromBody] JsonElement fields)
        {
            if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                // get the content field from the body request
                string content = fields.GetProperty("content").ToString();

                List<Message> messages = _contactsService.GetMessages(fields.GetProperty("userid").ToString(), id);

                // the id of the new message
                int id_of_last;
                if (messages.Count == 0)
                {
                    id_of_last = 0;
                } else
                {
                    id_of_last = messages[messages.Count - 1].id;
                }

                Message message = new Message(id_of_last + 1, content, true, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), "Text");
                _contactsService.AddMessage(fields.GetProperty("userid").ToString(), id, message);
                return Created(string.Format("/api/contacts/{0}/messages/{1}", id, message.id), message);
            }
            // need to take care if is not 
            return BadRequest();
        }

        //[Route("api/contacts/{user}/{id}/messages/{id2}")]
        [HttpGet("{user}/{id1}/messages/{id2}")]
        public IActionResult GetMessage(string user, string id1, int id2)
        {
            if (_contactsService.GetUser(user) != null)
            {
                Message? message = _contactsService.GetMessage(user, id1, id2);
                if (message != null)
                {
                    return Json(message);

                }
            }
            return BadRequest();
        }

        [HttpPut("{id1}/messages/{id2}")]
        public IActionResult UpdateMessage(string id1, int id2, [FromBody] JsonElement fields)
        {
            if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                Message? message = _contactsService.GetMessage(fields.GetProperty("userid").ToString(), id1, id2);
                if (message != null)
                {
                    // get the content field from the body request
                    string content = fields.GetProperty("content").ToString();

                    _contactsService.UpdateMessage(fields.GetProperty("userid").ToString(), id1, id2, content);
                    return NoContent();

                }
            }
            // need to take care of the view if not login
            return BadRequest();
        }

        [HttpDelete("{id1}/messages/{id2}")]
        public IActionResult DeleteMessage(string id1, int id2, [FromBody] JsonElement fields)
        {
            if (_contactsService.GetUser(fields.GetProperty("userid").ToString()) != null)
            {
                Message? message = _contactsService.GetMessage(fields.GetProperty("userid").ToString(), id1, id2);
                if (message != null)
                {
                    _contactsService.RemoveMessage(fields.GetProperty("userid").ToString(), id1, id2);
                    return NoContent();

                }
            }
            return BadRequest();
        }
    }
}
