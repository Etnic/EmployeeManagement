using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasOne(d => d.Department)
                   .WithMany(p => p.Employees)
                   .HasForeignKey(d => d.IdDepartment)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Employee_id_department");

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasColumnName("name")
                   .HasMaxLength(255);

                entity.Property(e => e.Email)
                  .IsRequired()
                  .HasColumnName("email")
                  .HasMaxLength(255);

                entity.Property(e => e.PhotoPath)
                  .HasColumnName("photopath")
                  .HasMaxLength(255);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.Property(e => e.IdDepartment).HasColumnName("id_department");                

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character(20)");
            });
        }












//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseNpgsql("Host=localhost;Database=sqlex;Username=postgres;Password=Lokpiuj14");
//            }
//        }
    }
}
