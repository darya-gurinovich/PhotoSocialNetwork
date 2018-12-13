using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PhotoSocialNetwork.Models
{
    public partial class PhotoSocialNetworkContext : DbContext
    {
        public PhotoSocialNetworkContext()
        {
        }

        public PhotoSocialNetworkContext(DbContextOptions<PhotoSocialNetworkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlockingStatus> BlockingStatus { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostBlockingLogs> PostBlockingLogs { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<RemovalLogs> RemovalLogs { get; set; }
        public virtual DbSet<UserAccessRights> UserAccessRights { get; set; }
        public virtual DbSet<UserBlockingLogs> UserBlockingLogs { get; set; }
        public virtual DbSet<UserPermissions> UserPermissions { get; set; }
        public virtual DbSet<UserRegistrationLogs> UserRegistrationLogs { get; set; }
        public virtual DbSet<UserRelationships> UserRelationships { get; set; }
        public virtual DbSet<UserRelationshipStatus> UserRelationshipStatus { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-FP82SIF\\SQLEXPRESS;Initial Catalog=PhotoSocialNetwork;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockingStatus>(entity =>
            {
                entity.Property(e => e.BlockingName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).IsRequired();

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<PostBlockingLogs>(entity =>
            {
                entity.HasKey(e => e.PostBlockingLogId);

                entity.Property(e => e.BlockingDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BlockedByUser)
                    .WithMany(p => p.PostBlockingLogs)
                    .HasForeignKey(d => d.BlockedByUserId)
                    .HasConstraintName("FK_PostBlockingLogs_BlockedByUser");

                entity.HasOne(d => d.BlockedPost)
                    .WithMany(p => p.PostBlockingLogs)
                    .HasForeignKey(d => d.BlockedPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostBlockingLogs_BlockedPost");

                entity.HasOne(d => d.BlockingStatus)
                    .WithMany(p => p.PostBlockingLogs)
                    .HasForeignKey(d => d.BlockingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostBlockingLogs_BlockingStatus");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profile)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Profile_User");
            });

            modelBuilder.Entity<RemovalLogs>(entity =>
            {
                entity.HasKey(e => e.RemovalLogId);

                entity.Property(e => e.RemovalDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RemovedObjectInfo)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.RemovedByUser)
                    .WithMany(p => p.RemovalLogs)
                    .HasForeignKey(d => d.RemovedByUserId)
                    .HasConstraintName("FK_RemovalLogs_RemovedByUser");
            });

            modelBuilder.Entity<UserAccessRights>(entity =>
            {
                entity.HasKey(e => e.UserAccessRightId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAccessRights)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserAccessRight_User");

                entity.HasOne(d => d.UserPermission)
                    .WithMany(p => p.UserAccessRights)
                    .HasForeignKey(d => d.UserPermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccessRight_UserPermissions");
            });

            modelBuilder.Entity<UserBlockingLogs>(entity =>
            {
                entity.HasKey(e => e.UserBlockingLogId);

                entity.Property(e => e.BlockingDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BlockedByUser)
                    .WithMany(p => p.UserBlockingLogsBlockedByUser)
                    .HasForeignKey(d => d.BlockedByUserId)
                    .HasConstraintName("FK_UserBlockingLogs_BlockedByUser");

                entity.HasOne(d => d.BlockedUser)
                    .WithMany(p => p.UserBlockingLogsBlockedUser)
                    .HasForeignKey(d => d.BlockedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBlockingLogs_BlockedUser");

                entity.HasOne(d => d.BlockingStatus)
                    .WithMany(p => p.UserBlockingLogs)
                    .HasForeignKey(d => d.BlockingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBlockingLogs_BlockingStatus");
            });

            modelBuilder.Entity<UserPermissions>(entity =>
            {
                entity.HasKey(e => e.UserPermissionId);

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRegistrationLogs>(entity =>
            {
                entity.HasKey(e => e.UserRegistrationLogId);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRegistrationLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRegistrationLogs_User");
            });

            modelBuilder.Entity<UserRelationships>(entity =>
            {
                entity.HasKey(e => e.UserRelationshipId);

                entity.HasOne(d => d.DependentUser)
                    .WithMany(p => p.UserRelationshipsDependentUser)
                    .HasForeignKey(d => d.DependentUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelationship_DependentUser");

                entity.HasOne(d => d.MainUser)
                    .WithMany(p => p.UserRelationshipsMainUser)
                    .HasForeignKey(d => d.MainUserId)
                    .HasConstraintName("FK_UserRelationship_MainUser");

                entity.HasOne(d => d.UserRelationshipStatus)
                    .WithMany(p => p.UserRelationships)
                    .HasForeignKey(d => d.UserRelationshipStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelationships_UserRelationshipStatus");
            });

            modelBuilder.Entity<UserRelationshipStatus>(entity =>
            {
                entity.Property(e => e.UserRelationshipStatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__A9D10534A5EBB64C")
                    .IsUnique();

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Users__5E55825B5F36171A")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogInDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });
        }
    }
}
