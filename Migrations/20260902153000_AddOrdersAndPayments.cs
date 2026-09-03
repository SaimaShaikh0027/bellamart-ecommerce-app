using System;
using bellamart_ecommerce_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bellamart_ecommerce_app.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260902153000_AddOrdersAndPayments")]
public partial class AddOrdersAndPayments : Migration
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "9.0.11");

        modelBuilder.Entity("bellamart_ecommerce_app.Models.Order", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Address").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("CustomerName").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("ItemsJson").IsRequired().HasColumnType("TEXT");
            entity.Property<int>("OrderNumber").HasColumnType("INTEGER");
            entity.Property<string>("PaymentMethod").IsRequired().HasColumnType("TEXT");
            entity.Property<DateTime>("PlacedAt").HasColumnType("TEXT");
            entity.Property<decimal>("Total").HasColumnType("TEXT");
            entity.Property<int?>("UserId").HasColumnType("INTEGER");
            entity.HasKey("Id");
            entity.ToTable("Orders");
        });

        modelBuilder.Entity("bellamart_ecommerce_app.Models.Payment", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<decimal>("Amount").HasColumnType("TEXT");
            entity.Property<string>("Method").IsRequired().HasColumnType("TEXT");
            entity.Property<int>("OrderId").HasColumnType("INTEGER");
            entity.Property<DateTime>("RecordedAt").HasColumnType("TEXT");
            entity.Property<string>("Status").IsRequired().HasColumnType("TEXT");
            entity.Property<int?>("UserId").HasColumnType("INTEGER");
            entity.HasKey("Id");
            entity.HasIndex("OrderId");
            entity.ToTable("Payments");
        });

        modelBuilder.Entity("bellamart_ecommerce_app.Models.Product", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Category").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Description").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("ImageUrl").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Name").IsRequired().HasColumnType("TEXT");
            entity.Property<decimal>("Price").HasColumnType("TEXT");
            entity.Property<int>("Stock").HasColumnType("INTEGER");
            entity.HasKey("Id");
            entity.ToTable("Products");
        });

        modelBuilder.Entity("bellamart_ecommerce_app.Models.User", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Email").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Name").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Password").IsRequired().HasColumnType("TEXT");
            entity.HasKey("Id");
            entity.HasIndex("Email").IsUnique();
            entity.ToTable("Users");
        });

        modelBuilder.Entity("bellamart_ecommerce_app.Models.Payment", entity =>
        {
            entity.HasOne("bellamart_ecommerce_app.Models.Order", null)
                .WithMany()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
#pragma warning restore 612, 618
    }

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: true),
                CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                Total = table.Column<decimal>(type: "TEXT", nullable: false),
                PlacedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                PaymentMethod = table.Column<string>(type: "TEXT", nullable: false),
                Address = table.Column<string>(type: "TEXT", nullable: false),
                ItemsJson = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: true),
                Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                Method = table.Column<string>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false),
                RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Payments_OrderId",
            table: "Payments",
            column: "OrderId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Payments");
        migrationBuilder.DropTable(name: "Orders");
        migrationBuilder.DropIndex(name: "IX_Users_Email", table: "Users");
    }
}