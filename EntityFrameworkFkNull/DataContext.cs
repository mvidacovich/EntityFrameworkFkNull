using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EntityFrameworkFkNull
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            Configuration.AutoDetectChangesEnabled = true;
        }

        public DataContext(string connectionString)
            : base(connectionString)
        {
            Configuration.AutoDetectChangesEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new TicketConfiguration());
        }
    }

    public class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        public TicketConfiguration()
        {
            HasRequired(x => x.Owner)
                .WithMany()
                .HasForeignKey(x=>x.OwnerId);
            Property(x => x.OwnerId)
                .HasColumnName("Owner_Id");

            HasOptional(x => x.LockedByUser)
                .WithMany()
                .HasForeignKey(x => x.LockedByUserId);

            Property(x => x.ETag)
                .IsConcurrencyToken(true);
        }
    }

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Ticket
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public int? LockedByUserId { get; set; }
        public virtual User LockedByUser { get; set; }

        [Timestamp]
        public byte[] ETag { get; set; }
    }
}