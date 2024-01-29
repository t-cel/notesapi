﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotesAPI.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NotesAPI.Migrations
{
    [DbContext(typeof(NotesDbContext))]
    partial class NotesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NotesAPI.Model.Note", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Notes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Content = "test email@test.xyz 123",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "EMAIL",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Content = "test +48123123123 abc",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "PHONE",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 3L,
                            Content = "test 123",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "NONE",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 4L,
                            Content = "test 2 512312522 abc",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "PHONE",
                            UserId = 1L
                        },
                        new
                        {
                            Id = 5L,
                            Content = "test 123",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "NONE",
                            UserId = 2L
                        },
                        new
                        {
                            Id = 6L,
                            Content = "test 123",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "NONE",
                            UserId = 2L
                        },
                        new
                        {
                            Id = 7L,
                            Content = "test 2 512312522 abc",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "PHONE",
                            UserId = 3L
                        },
                        new
                        {
                            Id = 8L,
                            Content = "test 2 512312522 abc",
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Tag = "PHONE",
                            UserId = 3L
                        });
                });

            modelBuilder.Entity("NotesAPI.Model.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateExpireUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("JwtId")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("NotesAPI.Model.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Email = "user1@test.xyz",
                            PasswordHash = "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO"
                        },
                        new
                        {
                            Id = 2L,
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Email = "user2@test.xyz",
                            PasswordHash = "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO"
                        },
                        new
                        {
                            Id = 3L,
                            DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc),
                            Email = "user3@test.xyz",
                            PasswordHash = "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}