﻿using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DolphinsSunsetResort.Data;

public class AuthDbContext : IdentityDbContext<AplicationUser>
{
	public AuthDbContext(DbContextOptions<AuthDbContext> options)
		: base(options)
	{
	}

	public DbSet<Room> Rooms { get; set; }
	public DbSet<Price> Prices { get; set; }

	public DbSet<Booking> Bookings { get; set; }

	public DbSet<BookingRoom> BookingRooms { get; set; }

	public DbSet<Cart> Carts { get; set; }
	public DbSet<SmsNotification> SmsNotification { get; set; }
	public DbSet<EmailContact> EmailContact { get; set; }
	public DbSet<EmailNotification> EmailNotification { get; set; }

	public DbSet<EmailNews> EmailNews { get; set; }
	public DbSet<News> News { get; set; }

	public DbSet<AppFile> AppFiles { get; set; }

	public DbSet<DictionaryRecommendation> DictionaryRecommendations { get; set; }

	public DbSet<MenuItem> MenuItems { get; set; }

	public DbSet<MenuItemCategory> MenuItemCategories { get; set; }


	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		// Customize the ASP.NET Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);
		builder.Entity<Price>(entity =>
		{
			entity.HasKey(p => p.PriceId);

			entity.Property(e => e.BasePrice);
			entity.Property(e => e.Discount);
			entity.Property(e => e.DiscountIsActive).HasColumnType("bit");
			entity.Property(e => e.StartDate).HasColumnType("datetime");
			entity.Property(e => e.EndDate).HasColumnType("datetime");

			entity.HasOne(e => e.MenuItem)
			.WithOne(e => e.Price)
			.HasForeignKey<MenuItem>(e => e.PriceId)
			.HasConstraintName("FK_MenuItem_Price");

		});
		builder.Entity<Room>(entity =>
		{
			entity.ToTable("Room");
			entity.HasKey(p => p.RoomId);


			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.RoomType).HasMaxLength(50);
			entity.Property(e => e.Description).HasMaxLength(255);
			entity.Property(e => e.RoomStatus).HasMaxLength(50);

			entity.HasOne(p => p.Price)
				.WithMany(r => r.Rooms)
				.HasForeignKey(p => p.PriceId)
				.HasConstraintName("FK_Room_Price");


		});

		builder.Entity<DictionaryRecommendation>(entity =>
		{
			entity.ToTable("DictionaryRecommendation");
			entity.HasKey(p => p.RecommendationId);


		});

		builder.Entity<Booking>(entity =>
		{
			entity.ToTable("Booking");

			entity.HasOne(p => p.AplicationUser)
			.WithMany(r => r.Bookings)
			.HasForeignKey(p => p.UserId)
			.HasConstraintName("FK_Booking_AplicationUser");

			entity.HasOne(p => p.DictionaryRecommendation)
			.WithMany(r => r.Bookings)
			.HasForeignKey(p => p.RecommendationId)
			.HasConstraintName("FK_Booking_DictionaryRecommandation");

		});

		builder.Entity<BookingRoom>(entity =>
		{
			entity.ToTable("BookingRoom");

			entity.HasOne(p => p.Booking)
			.WithMany(r => r.BookingRooms)
			.HasForeignKey(p => p.BookingId)
			.HasConstraintName("FK_BookingRoom_Booking");

			entity.HasOne(p => p.Room)
			.WithMany(r => r.BookingRooms)
			.HasForeignKey(p => p.RoomId)
			.HasConstraintName("FK_BookingRoom_Room");

		});

		builder.Entity<Cart>(entity =>
		{
			entity.ToTable("Cart");
			entity.HasKey(p => p.RecordId);


		});

		builder.Entity<News>(entity =>
		{
			entity.HasOne(p => p.Image)
				.WithOne(r => r.News)
				.HasForeignKey<News>(p => p.ImageId)  
				.OnDelete(DeleteBehavior.Cascade)    
				.HasConstraintName("FK_News_AppFiles");
		});

		builder.Entity<MenuItem>(entity =>
		{
			entity.HasOne(p => p.Image)
				.WithOne(r => r.MenuItem)
				.HasForeignKey<MenuItem>(p => p.ImageId) 
				.OnDelete(DeleteBehavior.Cascade)       
				.HasConstraintName("FK_MenuItem_AppFiles");
		});


		builder.Entity<MenuItemCategory>(entity =>
		{
			entity.HasMany<MenuItem>(p => p.MenuItems)
			.WithOne(r => r.MenuItemCategory)
			.HasForeignKey(p => p.CategoryId)
			.HasConstraintName("FK_MenuItemCategory_MenuItem");
		});


	}
}
