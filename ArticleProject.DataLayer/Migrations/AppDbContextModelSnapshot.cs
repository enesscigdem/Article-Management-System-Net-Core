﻿// <auto-generated />
using System;
using ArticleProject.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArticleProject.DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArticleCategory", b =>
                {
                    b.Property<Guid>("ArticlesArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoriesCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArticlesArticleId", "CategoriesCategoryId");

                    b.HasIndex("CategoriesCategoryId");

                    b.ToTable("ArticleCategory");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Article", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            ArticleId = new Guid("b6c88083-570e-41b1-97b7-98829935a76b"),
                            AuthorId = new Guid("9614dd78-111c-42ec-8f02-379368493c0a"),
                            Content = "Lorem Ipsum, Çiçero'nun MÖ 45 yılında yazdığı \"de Finibus Bonorum et Malorum – İyi ve Kötünün Uç Sınırları\" eserindeki 1.30.32 sayılı paragrafında yer alır. Bu eser Rönesans döneminde etik teorileri üzerine bilimsel inceleme konusu haline gelmiştir. Lorem Ipsum 1500'lü yıllardan itibaren aşağıdaki formuyla standartlaşmıştır: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            CreationDate = new DateTime(2023, 10, 31, 17, 2, 33, 48, DateTimeKind.Local).AddTicks(309),
                            Image = "-",
                            IsActive = true,
                            Likes = 20,
                            Title = "Asp.net Core Deneme Makalesi 1",
                            Views = 41028
                        });
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = new Guid("3ced153f-93fb-4415-a5e8-2f97d6ae5d73"),
                            CategoryName = "Magazin",
                            Description = "Magazin Açıklaması",
                            IsActive = false
                        },
                        new
                        {
                            CategoryId = new Guid("6cc90f01-56b1-41eb-80fe-1fa1ce6180be"),
                            CategoryName = "Spor",
                            Description = "Spor Açıklaması",
                            IsActive = false
                        },
                        new
                        {
                            CategoryId = new Guid("ed391fd7-4a9b-45a1-a53f-d32ecc000aaa"),
                            CategoryName = "Gündem",
                            Description = "Gündem Açıklaması",
                            IsActive = false
                        },
                        new
                        {
                            CategoryId = new Guid("8ac5594c-8771-45db-aab0-3799e1086981"),
                            CategoryName = "Haber",
                            Description = "Haber Açıklaması",
                            IsActive = false
                        },
                        new
                        {
                            CategoryId = new Guid("87cfa4eb-fb6f-457b-bcc4-d470bc452b59"),
                            CategoryName = "Teknoloji",
                            Description = "Teknoloji Açıklaması",
                            IsActive = false
                        });
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Comment", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommentId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Follow", b =>
                {
                    b.Property<Guid>("FollowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FollowId");

                    b.HasIndex("FollowerId");

                    b.HasIndex("FollowingId");

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Like", b =>
                {
                    b.Property<Guid>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LikeId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("55a91adc-9a93-4900-af1a-2daf35b2360e"),
                            Email = "enescigdeem@gmail.com",
                            FirstName = "Enes",
                            IsActive = true,
                            LastName = "Çiğdem",
                            Password = "123456",
                            ProfilePicture = "-",
                            Role = "ADMIN",
                            UserName = "enescigdeem"
                        },
                        new
                        {
                            UserId = new Guid("9614dd78-111c-42ec-8f02-379368493c0a"),
                            Email = "busecinar@gmail.com",
                            FirstName = "Buse",
                            IsActive = true,
                            LastName = "Çınar",
                            Password = "123456",
                            ProfilePicture = "-",
                            Role = "USER",
                            UserName = "busecinar"
                        });
                });

            modelBuilder.Entity("ArticleCategory", b =>
                {
                    b.HasOne("ArticleProject.EntityLayer.Entities.Article", null)
                        .WithMany()
                        .HasForeignKey("ArticlesArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArticleProject.EntityLayer.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Article", b =>
                {
                    b.HasOne("ArticleProject.EntityLayer.Entities.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Comment", b =>
                {
                    b.HasOne("ArticleProject.EntityLayer.Entities.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArticleProject.EntityLayer.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Follow", b =>
                {
                    b.HasOne("ArticleProject.EntityLayer.Entities.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ArticleProject.EntityLayer.Entities.User", "Following")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Like", b =>
                {
                    b.HasOne("ArticleProject.EntityLayer.Entities.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArticleProject.EntityLayer.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ArticleProject.EntityLayer.Entities.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Comments");

                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
