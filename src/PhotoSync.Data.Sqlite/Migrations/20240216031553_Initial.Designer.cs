﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Data.Sqlite;

#nullable disable

namespace PhotoSync.Data.Sqlite.Migrations
{
    [DbContext(typeof(PhotoSyncContext))]
    [Migration("20240216031553_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("PhotoSync.Domain.Entities.Destination", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastSynced")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PhotoLibraryId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FullPath")
                        .IsUnique();

                    b.HasIndex("PhotoLibraryId");

                    b.ToTable("Destinations", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.ExcludedFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SourceFolderId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RelativePath")
                        .IsUnique();

                    b.HasIndex("SourceFolderId");

                    b.ToTable("ExcludedFolders", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProcessAction")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("SizeBytes")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SourceFolderId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RelativePath")
                        .IsUnique();

                    b.HasIndex("SourceFolderId");

                    b.ToTable("Photos", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.PhotoLibrary", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PhotoLibraries", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.SourceFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastRefreshed")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PhotoLibraryId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FullPath")
                        .IsUnique();

                    b.HasIndex("PhotoLibraryId");

                    b.ToTable("SourceFolders", (string)null);
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.Destination", b =>
                {
                    b.HasOne("PhotoSync.Domain.Entities.PhotoLibrary", null)
                        .WithMany("Destinations")
                        .HasForeignKey("PhotoLibraryId");
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.ExcludedFolder", b =>
                {
                    b.HasOne("PhotoSync.Domain.Entities.SourceFolder", "SourceFolder")
                        .WithMany("ExcludedFolders")
                        .HasForeignKey("SourceFolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceFolder");
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.Photo", b =>
                {
                    b.HasOne("PhotoSync.Domain.Entities.SourceFolder", "SourceFolder")
                        .WithMany("Photos")
                        .HasForeignKey("SourceFolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceFolder");
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.SourceFolder", b =>
                {
                    b.HasOne("PhotoSync.Domain.Entities.PhotoLibrary", null)
                        .WithMany("SourceFolders")
                        .HasForeignKey("PhotoLibraryId");
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.PhotoLibrary", b =>
                {
                    b.Navigation("Destinations");

                    b.Navigation("SourceFolders");
                });

            modelBuilder.Entity("PhotoSync.Domain.Entities.SourceFolder", b =>
                {
                    b.Navigation("ExcludedFolders");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
