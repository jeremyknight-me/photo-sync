﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Data.Sqlite;

#nullable disable

namespace PhotoSync.Data.Sqlite.Migrations
{
    [DbContext(typeof(PhotoSyncContext))]
    partial class PhotoSyncContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("PhotoSync.Domain.ExcludedFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PhotoLibraryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PhotoLibraryId");

                    b.ToTable("ExcludedFolders", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PhotoLibraryId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProcessAction")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("SizeBytes")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PhotoLibraryId");

                    b.ToTable("Photos", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.PhotoLibrary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastRefreshed")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastSynced")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceFolder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PhotoLibraries", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.ExcludedFolder", b =>
                {
                    b.HasOne("PhotoSync.Domain.PhotoLibrary", null)
                        .WithMany("ExcludedFolders")
                        .HasForeignKey("PhotoLibraryId");
                });

            modelBuilder.Entity("PhotoSync.Domain.Photo", b =>
                {
                    b.HasOne("PhotoSync.Domain.PhotoLibrary", null)
                        .WithMany("Photos")
                        .HasForeignKey("PhotoLibraryId");
                });

            modelBuilder.Entity("PhotoSync.Domain.PhotoLibrary", b =>
                {
                    b.Navigation("ExcludedFolders");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}