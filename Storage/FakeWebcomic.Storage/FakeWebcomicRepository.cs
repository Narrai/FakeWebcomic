using System.Collections.Generic;
using System.Linq;
using FakeWebcomic.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage
{
    public class FakeWebcomicRepository
    {
        private readonly FakeWebcomicContext _ctx;
        public FakeWebcomicRepository(FakeWebcomicContext context)
        {
            _ctx = context;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public DbSet<User> GetUsers()
        {
            return _ctx.Users;
        }

        public DbSet<ComicBook> GetComicBooks()
        {
            return _ctx.ComicBooks;
        }

        public DbSet<ComicPage> GetComicPages()
        {
            return _ctx.ComicPages;
        }

        public void AddUser(string name)
        {
            _ctx.Add(new User { Name = name, IsAdmin = false });
            _ctx.SaveChanges();
        }

    }
}
