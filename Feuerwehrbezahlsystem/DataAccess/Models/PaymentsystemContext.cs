using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Models
{
    public partial class PaymentsystemContext : DbContext
    {
        public PaymentsystemContext()
        {
        }

        public PaymentsystemContext(DbContextOptions<PaymentsystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Price> Prices { get; set; } = null!;
        public virtual DbSet<Topup> Topups { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Paymentsystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article", "paymentsystem");

                entity.HasIndex(e => e.PriceId, "fk_Article_price1_idx");

                entity.Property(e => e.ArticleId).HasColumnName("article_id");

                entity.Property(e => e.ArticleAmount)
                    .HasMaxLength(45)
                    .HasColumnName("article_amount");

                entity.Property(e => e.ArticleName)
                    .HasMaxLength(45)
                    .HasColumnName("article_name");

                entity.Property(e => e.PriceId).HasColumnName("price_id");

                entity.HasOne(d => d.Price)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.PriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("article$fk_Article_price1");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment", "paymentsystem");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.PaymentDate)
                    .HasMaxLength(45)
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentText)
                    .HasMaxLength(45)
                    .HasColumnName("payment_text");

                entity.Property(e => e.PaymentTotal)
                    .HasMaxLength(45)
                    .HasColumnName("payment_total");

                entity.HasMany(d => d.ArticleArticles)
                    .WithMany(p => p.PaymentPayments)
                    .UsingEntity<Dictionary<string, object>>(
                        "PaymentHasArticle",
                        l => l.HasOne<Article>().WithMany().HasForeignKey("ArticleArticleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("payment_has_article$fk_Payment_has_Article_Article1"),
                        r => r.HasOne<Payment>().WithMany().HasForeignKey("PaymentPaymentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("payment_has_article$fk_Payment_has_Article_Payment1"),
                        j =>
                        {
                            j.HasKey("PaymentPaymentId", "ArticleArticleId").HasName("PK_payment_has_article_Payment_payment_id");

                            j.ToTable("payment_has_article", "paymentsystem");

                            j.HasIndex(new[] { "ArticleArticleId" }, "fk_Payment_has_Article_Article1_idx");

                            j.HasIndex(new[] { "PaymentPaymentId" }, "fk_Payment_has_Article_Payment1_idx");

                            j.IndexerProperty<int>("PaymentPaymentId").HasColumnName("Payment_payment_id");

                            j.IndexerProperty<int>("ArticleArticleId").HasColumnName("Article_article_id");
                        });
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.ToTable("price", "paymentsystem");

                entity.Property(e => e.PriceId).HasColumnName("price_id");

                entity.Property(e => e.PriceSinceDate)
                    .HasPrecision(0)
                    .HasColumnName("price_sinceDate");

                entity.Property(e => e.PriceUntilDate)
                    .HasPrecision(0)
                    .HasColumnName("price_untilDate");

                entity.Property(e => e.PriceValue).HasColumnName("price_value");
            });

            modelBuilder.Entity<Topup>(entity =>
            {
                entity.ToTable("topups", "paymentsystem");

                entity.HasIndex(e => e.TopupExecutorId, "fk_TopUps_User1_idx");

                entity.Property(e => e.TopupId).HasColumnName("topup_id");

                entity.Property(e => e.Text).HasMaxLength(45);

                entity.Property(e => e.TopupAmount).HasColumnName("topup_amount");

                entity.Property(e => e.TopupDate)
                    .HasPrecision(0)
                    .HasColumnName("topup_date");

                entity.Property(e => e.TopupExecutorId)
                    .HasMaxLength(100)
                    .HasColumnName("topup_executor_id");

                entity.HasOne(d => d.TopupExecutor)
                    .WithMany(p => p.Topups)
                    .HasForeignKey(d => d.TopupExecutorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("topups$fk_TopUps_User1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "paymentsystem");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .HasColumnName("user_id");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.Comment).HasMaxLength(300);

                entity.Property(e => e.OpenCheckoutAmount).HasMaxLength(45);

                entity.Property(e => e.OpenCheckoutDate).HasPrecision(0);

                entity.HasMany(d => d.Payments)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasPayment",
                        l => l.HasOne<Payment>().WithMany().HasForeignKey("PaymentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("user_has_payment$fk_User_has_Payment_Payment1"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("user_has_payment$fk_User_has_Payment_User1"),
                        j =>
                        {
                            j.HasKey("UserId", "PaymentId").HasName("PK_user_has_payment_user_id");

                            j.ToTable("user_has_payment", "paymentsystem");

                            j.HasIndex(new[] { "PaymentId" }, "fk_User_has_Payment_Payment1_idx");

                            j.HasIndex(new[] { "UserId" }, "fk_User_has_Payment_User1_idx");

                            j.IndexerProperty<string>("UserId").HasMaxLength(100).HasColumnName("user_id");

                            j.IndexerProperty<int>("PaymentId").HasColumnName("Payment_id");
                        });

                entity.HasMany(d => d.TopupsNavigation)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasTopup",
                        l => l.HasOne<Topup>().WithMany().HasForeignKey("TopupId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("user_has_topups$fk_User_has_TopUps_TopUps1"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("user_has_topups$fk_User_has_TopUps_User1"),
                        j =>
                        {
                            j.HasKey("UserId", "TopupId").HasName("PK_user_has_topups_user_id");

                            j.ToTable("user_has_topups", "paymentsystem");

                            j.HasIndex(new[] { "TopupId" }, "fk_User_has_TopUps_TopUps1_idx");

                            j.HasIndex(new[] { "UserId" }, "fk_User_has_TopUps_User1_idx");

                            j.IndexerProperty<string>("UserId").HasMaxLength(100).HasColumnName("user_id");

                            j.IndexerProperty<int>("TopupId").HasColumnName("topup_id");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
