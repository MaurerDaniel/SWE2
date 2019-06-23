using App1.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;

namespace ClassLibrary1
{
    public class BloggingContext : DbContext
    {
        public DbSet<PictureModel> Pictures { get; set; }
        public DbSet<FotographerModel> Fotographers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
    }
}