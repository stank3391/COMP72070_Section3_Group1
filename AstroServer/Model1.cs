using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AstroServer
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Entity")
        {
        }

        public virtual DbSet<tbl_Message> tbl_Message { get; set; }
        public virtual DbSet<tbl_Post> tbl_Post { get; set; }
        public virtual DbSet<tbl_Token> tbl_Token { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Message>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Post>()
                .Property(e => e.Content)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Token>()
                .Property(e => e.AccessToken)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Token>()
                .Property(e => e.RefreshToken)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Users>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Users>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Users>()
                .Property(e => e.ProfilePic)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Message)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Message1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Post)
                .WithRequired(e => e.tbl_Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Token)
                .WithRequired(e => e.tbl_Users)
                .WillCascadeOnDelete(false);
        }
    }
}
