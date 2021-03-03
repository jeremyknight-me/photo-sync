﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Data;

namespace PhotoSync.Data.Migrations
{
    [DbContext(typeof(PhotoSyncContext))]
    [Migration("20210303002604_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("PhotoSync.Data.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DateCreated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsNew")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOrphaned")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProcessAction")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoSync.Data.Entities.Setting", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
