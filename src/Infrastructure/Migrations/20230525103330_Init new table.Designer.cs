﻿// <auto-generated />
using System;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230525103330_Init new table")]
    partial class Initnewtable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.AuthorEntity", b =>
                {
                    b.Property<Guid>("AuthorGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorLastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AuthorGuid");

                    b.ToTable("Authors", (string)null);

                    b.HasData(
                        new
                        {
                            AuthorGuid = new Guid("d791c427-0df6-410f-bf4a-bf623fe73888"),
                            AuthorLastName = "Kanneman",
                            AuthorName = "Daniel"
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookAuthorEntity", b =>
                {
                    b.Property<Guid>("AuthorGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.HasKey("AuthorGuid", "BookGuid");

                    b.ToTable("AuthorBook", (string)null);

                    b.HasData(
                        new
                        {
                            AuthorGuid = new Guid("d791c427-0df6-410f-bf4a-bf623fe73888"),
                            BookGuid = new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"),
                            Guid = new Guid("8af1bf87-a423-4795-af25-afbb6662b35d")
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookEntity", b =>
                {
                    b.Property<Guid>("BookGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BookFakeName")
                        .HasColumnType("text");

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BookPictureURL")
                        .HasColumnType("text");

                    b.Property<Guid>("CategoryGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("ISBN10")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ISBN13")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PagesCount")
                        .HasColumnType("integer");

                    b.Property<string>("PublishYear")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PublisherGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserGuid")
                        .HasColumnType("uuid");

                    b.HasKey("BookGuid");

                    b.HasIndex("CategoryGuid");

                    b.HasIndex("PublisherGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CategoryEntity", b =>
                {
                    b.Property<Guid>("CategoryGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CategoryGuid");

                    b.ToTable("Categories", (string)null);

                    b.HasData(
                        new
                        {
                            CategoryGuid = new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"),
                            CategoryName = "Computers - Programming"
                        });
                });

            modelBuilder.Entity("Domain.Entities.PublisherEntity", b =>
                {
                    b.Property<Guid>("PublisherGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PublisherGuid");

                    b.ToTable("Publishers", (string)null);

                    b.HasData(
                        new
                        {
                            PublisherGuid = new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284"),
                            PublisherName = "Manning Publications"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserGuid")
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

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SocialId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserGuid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.UserFavoriteBookEntity", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BookEntityBookGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("BookId", "UserId");

                    b.HasIndex("BookEntityBookGuid");

                    b.ToTable("FavoriteBooks");
                });

            modelBuilder.Entity("Domain.Entities.BookEntity", b =>
                {
                    b.HasOne("Domain.Entities.CategoryEntity", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.PublisherEntity", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany("FavoriteBooks")
                        .HasForeignKey("UserGuid");

                    b.Navigation("Category");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("Domain.Entities.UserFavoriteBookEntity", b =>
                {
                    b.HasOne("Domain.Entities.BookEntity", null)
                        .WithMany("FavoriteBooks")
                        .HasForeignKey("BookEntityBookGuid");
                });

            modelBuilder.Entity("Domain.Entities.BookEntity", b =>
                {
                    b.Navigation("FavoriteBooks");
                });

            modelBuilder.Entity("Domain.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Domain.Entities.PublisherEntity", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("FavoriteBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
