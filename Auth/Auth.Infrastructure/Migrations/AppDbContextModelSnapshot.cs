﻿// <auto-generated />
using System;
using Auth.Infrastructure.Database.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Auth.Domain.Models.AccountData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthVia")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("SocialId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountsData");
                });

            modelBuilder.Entity("Auth.Domain.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountDataId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountDataId")
                        .IsUnique();

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Auth.Domain.Models.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountDataId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountDataId")
                        .IsUnique();

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("Auth.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountDataId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountDataId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Auth.Domain.Models.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Auth.Domain.Models.AccountData", b =>
                {
                    b.HasOne("Auth.Domain.Models.UserRole", "Role")
                        .WithMany("AccountsData")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Auth.Domain.Models.Author", b =>
                {
                    b.HasOne("Auth.Domain.Models.AccountData", "AccountData")
                        .WithMany()
                        .HasForeignKey("AccountDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountData");
                });

            modelBuilder.Entity("Auth.Domain.Models.Publisher", b =>
                {
                    b.HasOne("Auth.Domain.Models.AccountData", "AccountData")
                        .WithMany()
                        .HasForeignKey("AccountDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountData");
                });

            modelBuilder.Entity("Auth.Domain.Models.User", b =>
                {
                    b.HasOne("Auth.Domain.Models.AccountData", "AccountData")
                        .WithMany()
                        .HasForeignKey("AccountDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountData");
                });

            modelBuilder.Entity("Auth.Domain.Models.UserRole", b =>
                {
                    b.Navigation("AccountsData");
                });
#pragma warning restore 612, 618
        }
    }
}
