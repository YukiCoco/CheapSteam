using System;
using ChpStmScraper.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
namespace ChpStmScraper
{
    public class ScraperDbContext : DbContext
    {
        public DbSet<Goods> Goods {get ;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.ConnectionString);
            //var verionArray = Configuration.MysqlVersion.Split(".");
            //options.UseMySql(Configuration.ConnectionString,new MySqlServerVersion(new Version(int.Parse(verionArray[0]),int.Parse(verionArray[1]),int.Parse(verionArray[2]))));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Goods>()
            // .Property(g => g.UpdateTime)
            // .HasDefaultValueSql("now()");
        }
    }
}