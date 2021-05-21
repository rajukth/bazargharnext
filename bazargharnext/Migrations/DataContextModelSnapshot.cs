﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bazargharnext.Models;

namespace bazargharnext.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("bazargharnext.Models.Category", b =>
                {
                    b.Property<int>("Category_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category_description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Category_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Category_id");

                    b.ToTable("category");
                });

            modelBuilder.Entity("bazargharnext.Models.Customer", b =>
                {
                    b.Property<int>("Customerid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Customerid");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("bazargharnext.Models.GalleryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Product_Id1")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id1");

                    b.ToTable("Gallery");
                });

            modelBuilder.Entity("bazargharnext.Models.Product", b =>
                {
                    b.Property<int>("Product_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Category_id")
                        .HasColumnType("int");

                    b.Property<int?>("Category_id1")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Product_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Userid")
                        .HasColumnType("int");

                    b.HasKey("Product_Id");

                    b.HasIndex("Category_id1");

                    b.ToTable("product");
                });

            modelBuilder.Entity("bazargharnext.Models.Product_Details", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Product_Id1")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id1");

                    b.ToTable("product_details");
                });

            modelBuilder.Entity("bazargharnext.Models.User", b =>
                {
                    b.Property<int>("Userid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Contact")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Photo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Userid");

                    b.ToTable("user");
                });

            modelBuilder.Entity("bazargharnext.Models.GalleryModel", b =>
                {
                    b.HasOne("bazargharnext.Models.Product", null)
                        .WithMany("GalleryModel")
                        .HasForeignKey("Product_Id1");
                });

            modelBuilder.Entity("bazargharnext.Models.Product", b =>
                {
                    b.HasOne("bazargharnext.Models.Category", null)
                        .WithMany("Product")
                        .HasForeignKey("Category_id1");
                });

            modelBuilder.Entity("bazargharnext.Models.Product_Details", b =>
                {
                    b.HasOne("bazargharnext.Models.Product", "Product")
                        .WithMany("Product_Details")
                        .HasForeignKey("Product_Id1");
                });
#pragma warning restore 612, 618
        }
    }
}
