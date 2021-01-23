using Mesi.Io.SilentProtocol.Domain;
using Microsoft.EntityFrameworkCore;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db
{
    public class SilentProtocolDbContext : DbContext
    {
        public DbSet<SilentProtocolEntry> Entries { get; set; }
        
        public SilentProtocolDbContext(DbContextOptions<SilentProtocolDbContext> dbContextOptions): base(dbContextOptions)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SilentProtocolEntry>(b =>
            {
                b.ToTable("t_silent_protocol_entries");
                
                b.HasKey(e => e.Id);

                b.Property(e => e.Suspect)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("suspect");

                b.Property(e => e.Entry)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("entry");

                b.Property(e => e.TimeStamp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("time_stamp");

                b.Property(e => e.CreatedAtUtc)
                    .IsRequired()
                    .HasColumnName("created_at_utc");
            });
        }
    }
}