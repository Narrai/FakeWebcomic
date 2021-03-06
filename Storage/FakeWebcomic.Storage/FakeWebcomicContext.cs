using System.Drawing;
using FakeWebcomic.Storage.Helper;
using FakeWebcomic.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage
{
    public class FakeWebcomicContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<ComicPage> ComicPages { get; set; }
        public FakeWebcomicContext(DbContextOptions<FakeWebcomicContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(u => u.EntityId);
            builder.Entity<ComicBook>().HasKey(u => u.EntityId);
            builder.Entity<ComicPage>().HasKey(u => u.EntityId);

            // TODO: Connect Relationship between ComicBook and ComicPage
            builder.Entity<ComicPage>()
                .HasOne(b => b.ComicBook)
                .WithMany(p => p.ComicPages)
                .HasForeignKey(u => u.ComicBookId);

            // TODO: Seed DB
            seedDB(builder);
        }

        private static void seedDB(ModelBuilder builder)
        {
            // Store and Define Objects
            User u1 = new User { EntityId = 1L, Name = "Kevin", IsAdmin = true };
            User u2 = new User { EntityId = 2L, Name = "Robert", IsAdmin = false };

            // Using Test Image
            var obj = new ImageConvertor();
            var img = Image.FromFile("Images/test_img.jpg");
            byte[] imgByteArr = obj.ConvertImageToByteArray(img, ".jpg");

            ComicPage p1 = new ComicPage { EntityId = 1L, ComicBookId = 1L, PageNumber = 1, Image = imgByteArr, PageTitle = "Tin goes to the amuesment park" };
            ComicBook b1 = new ComicBook { EntityId = 1L, Title = "Tin Tin's Adventure", Author = "Hergé", Genre = "Adventure", EditionNumber = 32, Description = "Description goes here" };

            // Insert the seed data to MS SQL DB
            builder.Entity<User>().HasData(u1);
            builder.Entity<User>().HasData(u2);

            builder.Entity<ComicBook>().HasData(b1);
            builder.Entity<ComicPage>().HasData(p1);
        }
    }
}
