﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using uwpUI.Core.Data;

namespace uwpUI.Core.Migrations
{
    [DbContext(typeof(BdoContext))]
    [Migration("20190804213538_BiggerItemName")]
    partial class BiggerItemName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("uwpUI.Core.Models.BdoItem", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Category")
                        .HasMaxLength(25);

                    b.Property<string>("Description")
                        .HasMaxLength(3000);

                    b.Property<int>("Grade");

                    b.Property<string>("Img")
                        .HasMaxLength(100);

                    b.Property<int?>("ItemGroupId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Weight")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.HasIndex("ItemGroupId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("uwpUI.Core.Models.ItemGroup", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Name")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("ItemGroups");
                });

            modelBuilder.Entity("uwpUI.Core.Models.BdoItem", b =>
                {
                    b.HasOne("uwpUI.Core.Models.ItemGroup")
                        .WithMany("Items")
                        .HasForeignKey("ItemGroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
