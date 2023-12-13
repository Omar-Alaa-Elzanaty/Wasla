﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wasla.DataAccess;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    [DbContext(typeof(WaslaDb))]
    [Migration("20231213080158_editvehicleorg")]
    partial class editvehicleorg
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AdvertismentVehicle", b =>
                {
                    b.Property<int>("AdvertismentId")
                        .HasColumnType("int");

                    b.Property<int>("BussesId")
                        .HasColumnType("int");

                    b.HasKey("AdvertismentId", "BussesId");

                    b.HasIndex("BussesId");

                    b.ToTable("AdvertismentVehicle");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", "Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", "Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", "Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", "Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", "Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", "Account");
                });

            modelBuilder.Entity("Wasla.Model.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Accounts", "Account");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Wasla.Model.Models.Advertisment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("organizationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Advertisments");
                });

            modelBuilder.Entity("Wasla.Model.Models.CustomerTripOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("OrderState")
                        .HasColumnType("tinyint");

                    b.Property<string>("OrganizationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<float>("RequiredPackagesWeight")
                        .HasColumnType("real");

                    b.Property<int>("RequiredSeats")
                        .HasColumnType("int");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CustomerTripOrders");
                });

            modelBuilder.Entity("Wasla.Model.Models.DriverRate", b =>
                {
                    b.Property<string>("DriverId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte>("Rate")
                        .HasColumnType("tinyint");

                    b.HasKey("DriverId", "CustomerId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("DriverRates");
                });

            modelBuilder.Entity("Wasla.Model.Models.OrganizationRegisterRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebSiteLink")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RequestId");

                    b.ToTable("OrganizationsRegisters");
                });

            modelBuilder.Entity("Wasla.Model.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ReciverName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReciverPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReciverUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TripId")
                        .HasColumnType("int");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Wasla.Model.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SetNum")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TripId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Wasla.Model.Models.Set", b =>
                {
                    b.Property<int>("setNum")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("setNum", "TripId");

                    b.HasIndex("TripId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Wasla.Model.Models.Station", b =>
                {
                    b.Property<string>("OrganizationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Langtitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganizationId", "Name");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("Wasla.Model.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("AvailablePackageSpace")
                        .HasColumnType("real");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("DriverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Wasla.Model.Models.UserFollow", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FollowerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CustomerId", "FollowerId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("FollowerId")
                        .IsUnique();

                    b.ToTable("UserFollow");
                });

            modelBuilder.Entity("Wasla.Model.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdsSidesNumber")
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capcity")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LicenseNumber")
                        .HasColumnType("int");

                    b.Property<string>("LicenseWord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("PackageCapcity")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Wasla.Model.Models.VehicleRate", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.Property<byte>("Rate")
                        .HasColumnType("tinyint");

                    b.HasKey("CustomerId", "VehicleId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("VehicleId");

                    b.ToTable("VehicleRates");
                });

            modelBuilder.Entity("Wasla.Model.Models.Organization", b =>
                {
                    b.HasBaseType("Wasla.Model.Models.Account");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("MaxWeight")
                        .HasColumnType("real");

                    b.Property<float>("MinWeight")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteLink")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Organizations", "Account");
                });

            modelBuilder.Entity("Wasla.Model.Models.User", b =>
                {
                    b.HasBaseType("Wasla.Model.Models.Account");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Users", "Account");
                });

            modelBuilder.Entity("Wasla.Model.Models.Customer", b =>
                {
                    b.HasBaseType("Wasla.Model.Models.User");

                    b.Property<int>("points")
                        .HasColumnType("int");

                    b.ToTable("Customer", "Account");
                });

            modelBuilder.Entity("Wasla.Model.Models.Driver", b =>
                {
                    b.HasBaseType("Wasla.Model.Models.User");

                    b.Property<string>("LicenseImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Drivers", "Account");
                });

            modelBuilder.Entity("Wasla.Model.Models.Employee", b =>
                {
                    b.HasBaseType("Wasla.Model.Models.User");

                    b.Property<long>("NationalId")
                        .HasColumnType("bigint");

                    b.Property<string>("OrgId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("OrgId");

                    b.ToTable("Employees", "Account");
                });

            modelBuilder.Entity("AdvertismentVehicle", b =>
                {
                    b.HasOne("Wasla.Model.Models.Advertisment", null)
                        .WithMany()
                        .HasForeignKey("AdvertismentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("BussesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.Account", b =>
                {
                    b.OwnsMany("Wasla.Model.Helpers.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("AccountId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("RefToken")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AccountId", "Id");

                            b1.ToTable("RefreshToken", "Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Wasla.Model.Models.DriverRate", b =>
                {
                    b.HasOne("Wasla.Model.Models.Customer", "Customer")
                        .WithMany("DriversRate")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Driver", "Driver")
                        .WithMany("Rates")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("Wasla.Model.Models.Package", b =>
                {
                    b.HasOne("Wasla.Model.Models.Trip", "Trip")
                        .WithMany("Packages")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Wasla.Model.Models.Reservation", b =>
                {
                    b.HasOne("Wasla.Model.Models.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Trip", "Trip")
                        .WithMany("Reservations")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Wasla.Model.Models.Set", b =>
                {
                    b.HasOne("Wasla.Model.Models.Trip", null)
                        .WithMany("RecervedSets")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.Station", b =>
                {
                    b.HasOne("Wasla.Model.Models.Organization", null)
                        .WithMany("Stations")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.Trip", b =>
                {
                    b.HasOne("Wasla.Model.Models.Driver", "Driver")
                        .WithMany("Trips")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Organization", "Organization")
                        .WithMany("TripList")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Vehicle", "Vehicle")
                        .WithMany("Trips")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Organization");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Wasla.Model.Models.UserFollow", b =>
                {
                    b.HasOne("Wasla.Model.Models.Customer", "Customer")
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.UserFollow", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Customer", "Follower")
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.UserFollow", "FollowerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("Wasla.Model.Models.Vehicle", b =>
                {
                    b.HasOne("Wasla.Model.Models.Organization", null)
                        .WithMany("Vehicles")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("Wasla.Model.Models.VehicleRate", b =>
                {
                    b.HasOne("Wasla.Model.Models.Customer", "Customer")
                        .WithMany("VehicleRates")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Vehicle", "Vehicle")
                        .WithMany("Rates")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Wasla.Model.Models.Organization", b =>
                {
                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.Organization", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.User", b =>
                {
                    b.HasOne("Wasla.Model.Models.Account", null)
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.User", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.Customer", b =>
                {
                    b.HasOne("Wasla.Model.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wasla.Model.Models.Driver", b =>
                {
                    b.HasOne("Wasla.Model.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.Driver", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Organization", "Orgainzation")
                        .WithMany("Drivers")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Orgainzation");
                });

            modelBuilder.Entity("Wasla.Model.Models.Employee", b =>
                {
                    b.HasOne("Wasla.Model.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Wasla.Model.Models.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wasla.Model.Models.Organization", "Organization")
                        .WithMany("Employees")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Wasla.Model.Models.Trip", b =>
                {
                    b.Navigation("Packages");

                    b.Navigation("RecervedSets");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Wasla.Model.Models.Vehicle", b =>
                {
                    b.Navigation("Rates");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("Wasla.Model.Models.Organization", b =>
                {
                    b.Navigation("Drivers");

                    b.Navigation("Employees");

                    b.Navigation("Stations");

                    b.Navigation("TripList");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Wasla.Model.Models.Customer", b =>
                {
                    b.Navigation("DriversRate");

                    b.Navigation("Reservations");

                    b.Navigation("VehicleRates");
                });

            modelBuilder.Entity("Wasla.Model.Models.Driver", b =>
                {
                    b.Navigation("Rates");

                    b.Navigation("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
