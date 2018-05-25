﻿using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options) { }

        public DbSet<User> Users { get; set; }
        //public DbSet<Item> Items { get; set; }
    }
}
