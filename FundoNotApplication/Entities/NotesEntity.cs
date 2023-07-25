namespace FundoNotApplication.Entities
{
    public class NotesEntity
    {
        public string NoteId { get; set; }
        public string EmailId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPinned { get; set; }
        public bool IsArchived { get; set; }

        public bool IsTrashed { get; set; }
        public string Colour { get; set; }
        public string Reminder { get; set; }
        public List<string> Labels { get; set; } = new();
        public List<string> Collabs { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }




    }
}
