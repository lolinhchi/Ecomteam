using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Models;

namespace TeamBee.Connect_Data
{
    public class TeamBeeDbContext : DbContext
    {
        public TeamBeeDbContext(DbContextOptions<TeamBeeDbContext> options) : base(options) { }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<NoCartDetail> NoCartDetails { get; set; }
        public DbSet<NoOrder> NoOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<QuangCao> QuangCaos { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogItem> BlogItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            // Bảng User
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).IsUnique();

            // Bảng ProductBrand
           

            // Bảng ProductType
            modelBuilder.Entity<Category>().HasIndex(t => t.Name).IsUnique();

           
        }

    }
}
