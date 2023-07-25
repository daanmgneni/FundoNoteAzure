using FundoNotApplication.Entities;
using FundoNotApplication.Interface;
using FundoNotApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundoNotApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotes _notes;
        private readonly ILogger<UserController> _logger;

        public NotesController(INotes notes, ILogger<UserController> logger)
        {
            _notes = notes;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("Add")]

        public IActionResult AddNote(NoteModel newNote)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                NotesEntity note = _notes.AddNotes(emailId, newNote);

                if (note != null)
                    return Ok(new { success = true, message = "Note Added Successfully", data = note });
                else
                    return Ok(new { success = false, message = "Note Added UnSuccessfully" });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
            

        [Authorize]
        [HttpPost]
        [Route("GetAll")]

        public IActionResult ViewAllNotes()
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                IEnumerable<NotesEntity> allNotes = _notes.ViewAll(emailId);

                if (allNotes != null)
                    return Ok(new { success = true, message = "Notes Retrived Succesfully", data = allNotes });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
        }

        [Authorize]
        [HttpPost]
        [Route("GetNoteById")]

        public IActionResult ViewNoteById(string noteId)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                IEnumerable<NotesEntity> allNotes = _notes.ViewAll(emailId);

                if (allNotes != null)
                    return Ok(new { success = true, message = "Notes Retrived Succesfully", data = allNotes });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [Authorize]
        [HttpPost]
        [Route("Edit")]

        public IActionResult EditNote(string noteId, NoteModel newNote)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                NotesEntity note = _notes.EditNote(emailId, noteId, newNote);

                if (note != null)
                    return Ok(new { success = true, message = "Notes Updated Succesfully", data = note });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [Authorize]
        [HttpPut]
        [Route("Pin")]

        public IActionResult PinNote(string noteId)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _notes.PinNote(emailId, noteId);

                if (result != null)
                    return Ok(new { success = true, message = "Notes pinned Succesfully", data = result });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }


        }

        [Authorize]
        [HttpPut]
        [Route("Archive")]

        public IActionResult ArchiveNote(string noteId)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _notes.ArchiveNote(emailId, noteId);

                if (result != null)
                    return Ok(new { success = true, message = "Notes archived Succesfully", data = result });
                else
                    return Ok(new { success = false, message = "Something went wrong" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Trash")]

        public IActionResult TrashNote(string noteId)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _notes.TrashNote(emailId, noteId);

                if (result != null)
                    return Ok(new { success = true, message = "Notes trashed Succesfully", data = result });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [Authorize]
        [HttpPut]
        [Route("Colour")]

        public IActionResult ChnageColour(string noteId,string colour)
        {
            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _notes.AddColour(emailId, noteId, colour);

                if (result != null)
                    return Ok(new { success = true, message = "Colour added Succesfully", data = result });
                else
                    return Ok(new { success = false, message = "Something went wrong" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete")]

        public IActionResult DeleteNote(string noteId)
        {

            try
            {
                string emailId = User.FindFirstValue(ClaimTypes.Email);
                bool result = _notes.DeleteNote(emailId, noteId);

                if (result != null)
                    return Ok(new { success = true, message = "Notes deleted Succesfully", data = result });
                else
                    return Ok(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
    }
}
