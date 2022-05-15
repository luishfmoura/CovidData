using CovidData.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidData.Data
{
    public class CovidDataContext : DbContext
    {
        public DbSet<CovidCase> covidCase { get; set; }
        public CovidDataContext(DbContextOptions<CovidDataContext> options) : base(options)
        {
        }
    }
}
