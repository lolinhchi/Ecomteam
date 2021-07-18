﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamBee.Connect_Data;

namespace TeamBee.Migrations
{
    [DbContext(typeof(TeamBeeDbContext))]
    partial class TeamBeeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TeamBee.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateModify");

                    b.Property<string>("Name");

                    b.Property<string>("URL")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Blog_Category");
                });

            modelBuilder.Entity("TeamBee.Models.BlogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category_BlogId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Thumbnail");

                    b.Property<string>("Title");

                    b.Property<string>("URL");

                    b.HasKey("Id");

                    b.HasIndex("Category_BlogId");

                    b.ToTable("BlogItem");
                });

            modelBuilder.Entity("TeamBee.Models.CartDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cart_Id");

                    b.Property<DateTime>("DateCreate");

                    b.Property<int>("PriceSingle");

                    b.Property<int>("Product_Id");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("Cart_Id");

                    b.HasIndex("Product_Id");

                    b.ToTable("CartDetail");
                });

            modelBuilder.Entity("TeamBee.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateModify");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("URL")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TeamBee.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateModify");

                    b.Property<int>("Product_Id");

                    b.Property<double>("Stars");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("TeamBee.Models.NoCartDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cart_Id");

                    b.Property<DateTime>("DateCreate");

                    b.Property<int?>("NoOrderId");

                    b.Property<int>("PriceSingle");

                    b.Property<int>("Product_Id");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("Cart_Id");

                    b.HasIndex("NoOrderId");

                    b.HasIndex("Product_Id");

                    b.ToTable("NoCartDetail");
                });

            modelBuilder.Entity("TeamBee.Models.NoOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Pay");

                    b.Property<string>("ShipEmail");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ShipPhone")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Status");

                    b.Property<int>("TotalPrice");

                    b.Property<int>("TotalQuantity");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("User_Id");

                    b.ToTable("NoOrder");
                });

            modelBuilder.Entity("TeamBee.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("GhiChu");

                    b.Property<string>("Pay");

                    b.Property<string>("ShipEmail");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ShipPhone")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Status");

                    b.Property<int>("TotalPrice");

                    b.Property<int>("TotalQuantity");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("TeamBee.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category_Id");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateModify");

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Orders");

                    b.Property<int>("Price");

                    b.Property<int>("SalePrice");

                    b.Property<double>("Stars");

                    b.Property<int>("Status");

                    b.Property<string>("Thumbnail");

                    b.Property<string>("URL");

                    b.Property<int>("Views");

                    b.HasKey("Id");

                    b.HasIndex("Category_Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("TeamBee.Models.QuangCao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Anh");

                    b.Property<string>("LienKet");

                    b.Property<int>("PhanTram");

                    b.Property<int>("Status");

                    b.Property<string>("Ten");

                    b.HasKey("Id");

                    b.ToTable("QuangCao");
                });

            modelBuilder.Entity("TeamBee.Models.Tracking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cart_Id");

                    b.Property<string>("DiaChi");

                    b.Property<string>("GhiChu");

                    b.Property<string>("HoTen");

                    b.Property<string>("SDT");

                    b.Property<string>("ThongTin");

                    b.Property<DateTime>("Time");

                    b.Property<string>("TongTien");

                    b.HasKey("Id");

                    b.HasIndex("Cart_Id");

                    b.ToTable("Tracking");
                });
            
            modelBuilder.Entity("TeamBee.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Active");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("Confirm");

                    b.Property<string>("Email");

                    b.Property<string>("NameFirst")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("NameLast")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Permission");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("TeamBee.Models.BlogItem", b =>
                {
                    b.HasOne("TeamBee.Models.Blog", "Blog_Category")
                        .WithMany("BlogItem")
                        .HasForeignKey("Category_BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.CartDetail", b =>
                {
                    b.HasOne("TeamBee.Models.Order", "Cart")
                        .WithMany("CartDetails")
                        .HasForeignKey("Cart_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamBee.Models.Product", "Product")
                        .WithMany("CartDetails")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.Comment", b =>
                {
                    b.HasOne("TeamBee.Models.Product", "Product")
                        .WithMany("Comments")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamBee.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.NoCartDetail", b =>
                {
                    b.HasOne("TeamBee.Models.Order", "Cart")
                        .WithMany()
                        .HasForeignKey("Cart_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamBee.Models.NoOrder")
                        .WithMany("NoCartDetails")
                        .HasForeignKey("NoOrderId");

                    b.HasOne("TeamBee.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.NoOrder", b =>
                {
                    b.HasOne("TeamBee.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.Order", b =>
                {
                    b.HasOne("TeamBee.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.Product", b =>
                {
                    b.HasOne("TeamBee.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("Category_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamBee.Models.Tracking", b =>
                {
                    b.HasOne("TeamBee.Models.Order", "Cart")
                        .WithMany()
                        .HasForeignKey("Cart_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
