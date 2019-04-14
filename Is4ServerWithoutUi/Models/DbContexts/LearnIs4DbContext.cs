using Is4ServerWithoutUi.Models.Entities;
using Is4ServerWithoutUi.Models.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Is4ServerWithoutUi.Models.DbContexts
{
    public class LearnIs4DbContext : DbContext
    {
        #region Properties

        public virtual DbSet<User> Users { get; set; }

        #endregion

        #region Constructors

        public LearnIs4DbContext(DbContextOptions<LearnIs4DbContext> options) : base(options)
        {
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
        }

        #endregion
    }
}