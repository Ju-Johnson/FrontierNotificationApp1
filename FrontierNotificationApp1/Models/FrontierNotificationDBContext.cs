using System;
using FrontierNotificationApp1.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FrontierNotificationApp1.Models;

namespace FrontierNotificationApp1.Models
{
	
	public partial class FrontierNotificationDBContext : IdentityDbContext<ApplicationUser>
    {
        
        public virtual DbSet<ChangeResquests> ChangeResquests { get; set; }
        public virtual DbSet<Incidents> Incidents { get; set; }
        

		//Moved database configuration to provider in Startup.cs
		

		//This constructor allows configuration to be passed into the Db context by dependency injection
		public FrontierNotificationDBContext(DbContextOptions<FrontierNotificationDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChangeResquests>(entity =>
            {
                entity.Property(e => e.EndDateTime).HasColumnType("smalldatetime");

				entity.Property(e => e.Number)
					.IsRequired()
					.HasMaxLength(20)
					.IsUnicode(false);

				entity.Property(e => e.Impact)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .IsRequired()
                    .HasColumnType("text");

				
			});

            modelBuilder.Entity<Incidents>(entity =>
            {
                entity.Property(e => e.CurrentDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Impact)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InitialDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                

                entity.Property(e => e.Sdpticket)
                    .HasColumnName("SDPTicket")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.VendorTicket)
                    .HasMaxLength(20)
                    .IsUnicode(false);

				
			});

            
        }

		public DbSet<FrontierNotificationApp1.Models.Account.ApplicationUser> ApplicationUser { get; set; }

		public DbSet<FrontierNotificationApp1.Models.ITteams> ITteams { get; set; }

		public DbSet<FrontierNotificationApp1.Models.ITcontacts> ITcontacts { get; set; }
    }
}
