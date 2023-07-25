using FundoNotApplication.Entities;
using FundoNotApplication.Models;

namespace FundoNotApplication.Interface;

public interface INotes
{
    //add note, view notes by id, edit notes, view all notes,pinnote, isarchive, istrash, add color, delete note
    NotesEntity AddNotes(string EmailId,NoteModel newNote);
    NotesEntity ViewById(string EmailId, string noteId);
    IEnumerable<NotesEntity> ViewAll(string EmailId);

    NotesEntity EditNote(string EmailId, string noteId, NoteModel updatedNote);
    bool ArchiveNote(string EmailId, string noteId);
    bool PinNote(string EmailId, string noteId);
    bool TrashNote(string EmailId, string noteId);
    bool AddColour(string EmailId, string noteId, string colour);
    bool DeleteNote(string EmailId, string noteId);
}
