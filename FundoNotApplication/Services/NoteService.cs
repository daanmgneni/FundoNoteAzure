using FundoNotApplication.Context;
using FundoNotApplication.Entities;
using FundoNotApplication.Interface;
using FundoNotApplication.Models;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FundoNotApplication.Services;

public class NoteService : INotes
{
    private readonly FundooContext _db;

    public NoteService(FundooContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }
    public NotesEntity AddNotes(string emailId, NoteModel newNote)
    {
        NotesEntity note = new()
        {
            NoteId = Guid.NewGuid().ToString(),
            EmailId = emailId,
            Title = newNote.Title,
            Description = newNote.Description,
            IsArchived = newNote.IsArchived,
            IsPinned = newNote.IsPinned,
            IsTrashed = newNote.IsTrashed,
            Reminder = newNote.Reminder,
            Labels = newNote.Labels,
            Collabs = newNote.Collabs,
            Colour = newNote.Colour,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        };

        _db.Notes.Add(note);
        int result = _db.SaveChanges();

        return (result > 0) ? note : null;
    }

    public IEnumerable<NotesEntity> ViewAll(string emailId)
    {
        IEnumerable<NotesEntity> noteForUser = _db.Notes.Where(x => x.EmailId == emailId);

        return noteForUser;
    }

    public NotesEntity ViewById (string emailId, string noteId)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        return noteForUser;
    }

    public NotesEntity EditNote(string emailId, string noteId, NoteModel updatedNote)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);

        if (noteForUser != null)
        {
            noteForUser.Title = updatedNote.Title;
            noteForUser.Description = updatedNote.Description;
            noteForUser.IsArchived = updatedNote.IsArchived;
            noteForUser.IsPinned = updatedNote.IsPinned;
            noteForUser.IsTrashed = updatedNote.IsTrashed;
            noteForUser.Reminder = updatedNote.Reminder;
            noteForUser.Labels = updatedNote.Labels;
            noteForUser.Collabs = updatedNote.Collabs;
            noteForUser.Colour = updatedNote.Colour;

            updatedNote.LastUpdatedAt = DateTime.Now;

            _db.Notes.Update(noteForUser);
            _db.SaveChanges();
        }
        return noteForUser;
    }

    public bool PinNote(string emailId, string noteId)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        if (noteForUser != null)
        {
            noteForUser.IsPinned = !noteForUser.IsPinned;
            _db.Notes.Update(noteForUser);
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public bool ArchiveNote(string emailId, string noteId)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        if (noteForUser != null)
        {
            noteForUser.IsArchived = !noteForUser.IsArchived;
            _db.Notes.Update(noteForUser);
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public bool TrashNote(string emailId, string noteId)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        if (noteForUser != null)
        {
            noteForUser.IsTrashed = !noteForUser.IsTrashed;
            _db.Notes.Update(noteForUser);
            _db.SaveChanges();
            return true;
        }
        return false;
    }
    public bool AddColour(string emailId, string noteId, string colour)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        if (noteForUser != null)
        {
            noteForUser.Colour = colour;
            _db.Notes.Update(noteForUser);
            _db.SaveChanges();
            return true;
        }
        return false;
    }
    public bool DeleteNote(string emailId,string noteId)
    {
        NotesEntity noteForUser = _db.Notes.FirstOrDefault(x => x.EmailId == emailId && x.NoteId == noteId);
        if (noteForUser != null)
        {
            _db.Notes.Remove(noteForUser);
            _db.SaveChanges();
            return true;
        }
        return false;
    }
}
