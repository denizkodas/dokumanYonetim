using dokumanYonetim.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace dokumanYonetim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-PAF2NE0\\SQLEXPRESS;Database=dokumanYonetim;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            }
        }
        public DbSet<Document>? Documents { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<DocumentAccess>? DocumentAccesses { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Role>? Roles { get; set; }
       // public DbSet<UserRoles>? UserRoles { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DocumentAccess ve Document ilişkisi
            modelBuilder.Entity<DocumentAccess>()
                .HasOne(da => da.Document)
                .WithMany(d => d.DocumentAccesses)
                .HasForeignKey(da => da.DocumentId)
                .OnDelete(DeleteBehavior.Restrict); // veya SetNull

            // DocumentAccess ve User ilişkisi
            modelBuilder.Entity<DocumentAccess>()
                .HasOne(da => da.User)
                .WithMany(u => u.DocumentAccesses)
                .HasForeignKey(da => da.UserId)
                .OnDelete(DeleteBehavior.Restrict); // veya SetNull

            // Document ve Category ilişkisi
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // veya SetNull

            // Document ve User arasındaki ilişkiyi tanımlıyoruz
            modelBuilder.Entity<Document>()
                .HasOne(d => d.CreatedByUser)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışı


        }



    }
}
