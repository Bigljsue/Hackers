using Microsoft.EntityFrameworkCore;
using WPF_HackersList.Models;

namespace WPF_HackersList.DataBaseClasses.DataBaseConfiguration
{
    public class DataBaseConfiguration : DbContext
    {        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = DataBase.db");
        }

        public DbSet<PersonModel> People { get; set; }
    }
}
