using Microsoft.EntityFrameworkCore;
using WPF_HackersList.Models;

namespace WPF_HackersList.DataBaseClasses.DataBaseConfiguration
{
    public class SecondDataBaseConfiguration : DbContext
    {
        private string SecondDataBasePath { get; set; }

        public SecondDataBaseConfiguration(string secondDataBase)
        {
            SecondDataBasePath = secondDataBase;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source = {SecondDataBasePath}");
        }

        public DbSet<PersonModel> People { get; set; }
    }
}
