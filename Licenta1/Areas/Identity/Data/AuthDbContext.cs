using DolphinsSunsetResort.Areas.Identity.Data;
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
	public  DbSet<SmsNotification> SmsNotification { get; set; }
	public  DbSet<EmailContact> EmailContact { get; set; }
	public  DbSet<EmailNotification> EmailNotification { get; set; }
    public DbSet<News> News { get; set; }


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
			entity.Property(e=>e.DiscountIsActive).HasColumnType("bit");
			entity.Property(e=>e.StartDate).HasColumnType("datetime");
			entity.Property(e => e.EndDate).HasColumnType("datetime");

		});
		builder.Entity<Room>(entity =>
		{
			entity.ToTable("Room");
			entity.HasKey(p => p.RoomId);


			entity.Property(e=>e.Name).HasMaxLength(50);
			entity.Property(e=>e.RoomType).HasMaxLength(50);
			entity.Property(e=>e.Description).HasMaxLength(255);
			entity.Property(e=>e.RoomStatus).HasMaxLength(50);

			entity.HasOne(p => p.Price)
				.WithMany(r => r.Rooms)
				.HasForeignKey(p => p.PriceId)
				.HasConstraintName("FK_Room_Price");


		});

		builder.Entity<Booking>(entity =>
		{
			entity.ToTable("Booking");

			entity.HasOne(p => p.AplicationUser)
			.WithMany(r => r.Bookings)
			.HasForeignKey(p => p.UserId)
			.HasConstraintName("FK_Booking_AplicationUser");

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


    }
}
