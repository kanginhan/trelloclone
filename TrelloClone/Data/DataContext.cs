using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<USERS> USERS { get; set; }
        public DbSet<BOARD> BOARD { get; set; }
        public DbSet<CARD_LIST> CARD_LIST { get; set; }
        public DbSet<CARD> CARD { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BOARD>().HasKey(m => new { m.EMAIL, m.BOARD_SEQ });
            modelBuilder.Entity<CARD_LIST>().HasKey(m => new { m.EMAIL, m.BOARD_SEQ, m.LIST_SEQ });
            modelBuilder.Entity<CARD>().HasKey(m => new { m.EMAIL, m.BOARD_SEQ, m.LIST_SEQ, m.CARD_SEQ });
        }
    }
}
