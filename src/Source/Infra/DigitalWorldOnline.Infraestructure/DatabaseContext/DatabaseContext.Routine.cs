using Microsoft.EntityFrameworkCore;
using DigitalWorldOnline.Commons.DTOs.Routine;
using DigitalWorldOnline.Infraestructure.ContextConfiguration.Routine;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<RoutineDTO> Routine { get; set; }

        internal static void RoutineEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoutineConfiguration());
        }
    }
}