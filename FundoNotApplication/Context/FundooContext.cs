using FundoNotApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundoNotApplication.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options) : base(options)
        {

        }

        // DbSet representing the Users table,Note Table in the database.
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<NotesEntity> Notes { get; set; }

        // This method is called by EF Core when creating the model for the database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the UserEntity .
                 modelBuilder.Entity<UserEntity>()
                .HasNoDiscriminator() // Disabling table-per-hierarchy inheritance for UserEntity.
                .HasManualThroughput(400) // Setting manual throughput for the UserEntity container in Azure Cosmos DB.
                .HasKey(x => x.EmailId); // Defining the primary key for the Users table as the EmailId property.

            modelBuilder.Entity<UserEntity>()
                .ToContainer("UserContainer") // Assigning the container name for the UserEntity in Azure Cosmos DB.
                .HasPartitionKey(x => x.EmailId); // Specifying the partition key for the UserEntity container.

            // Configuring the NotesEntity entity.
            modelBuilder.Entity<NotesEntity>()
                .HasNoDiscriminator() // Disabling table-per-hierarchy inheritance for NotesEntity.
                .HasManualThroughput(600) // Setting manual throughput for the NotesEntity container in Azure Cosmos DB.
                .HasKey(x => x.NoteId); // Defining the primary key for the Notes table as the NoteId property.

            modelBuilder.Entity<NotesEntity>()
                .ToContainer("NotesContainer") // Assigning the container name for the NotesEntity in Azure Cosmos DB.
                .HasPartitionKey(x => x.NoteId); // Specifying the partition key for the NotesEntity container.
        }
    }
}
