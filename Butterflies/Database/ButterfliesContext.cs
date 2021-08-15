using Microsoft.EntityFrameworkCore;

namespace Butterflies.Database
{
    public class ButterfliesContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}