using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatabaseModel.Models
{
    public partial class TestContext : DbContext
    {
        public virtual DbSet<CategoryMaster> CategoryMaster { get; set; }
        public virtual DbSet<CompanyMaster> CompanyMaster { get; set; }
        public virtual DbSet<CreditNote> CreditNote { get; set; }
        public virtual DbSet<DamagedGoodsMaster> DamagedGoodsMaster { get; set; }
        public virtual DbSet<DebitNote> DebitNote { get; set; }
        public virtual DbSet<InventoryItemMaster> InventoryItemMaster { get; set; }
        public virtual DbSet<InvoiceDetailsMaster> InvoiceDetailsMaster { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMaster { get; set; }
        public virtual DbSet<InvoiceProductMaster> InvoiceProductMaster { get; set; }
        public virtual DbSet<InvoiceSettlementMaster> InvoiceSettlementMaster { get; set; }
        public virtual DbSet<LookupMaster> LookupMaster { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<ProductDescriptionMaster> ProductDescriptionMaster { get; set; }
        public virtual DbSet<ProductMaster> ProductMaster { get; set; }
        public virtual DbSet<PurchaseMaster> PurchaseMaster { get; set; }
        public virtual DbSet<SalesMaster> SalesMaster { get; set; }
        public virtual DbSet<SubCategoryMaster> SubCategoryMaster { get; set; }
        public virtual DbSet<SubLookupMaster> SubLookupMaster { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<VendorMaster> VendorMaster { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Test;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryMaster>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cgst)
                    .HasColumnName("CGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Igst)
                    .HasColumnName("IGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Sgst)
                    .HasColumnName("SGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CompanyMaster>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address3)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactNo1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactNo2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("EmailID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gstno)
                    .IsRequired()
                    .HasColumnName("GSTNo")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CreditNote>(entity =>
            {
                entity.HasKey(e => new { e.CrNoteNo, e.CrNoteDate, e.ProductId, e.OrderId, e.OrderDate });

                entity.Property(e => e.CrNoteNo).HasMaxLength(50);

                entity.Property(e => e.CrNoteDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");
            });

            modelBuilder.Entity<DamagedGoodsMaster>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.ProductId });

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Price).HasColumnType("decimal(14, 2)");
            });

            modelBuilder.Entity<DebitNote>(entity =>
            {
                entity.HasKey(e => new { e.DrNoteNo, e.DrNoteDate, e.ProductId });

                entity.Property(e => e.DrNoteNo).HasMaxLength(50);

                entity.Property(e => e.DrNoteDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InventoryItemMaster>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ItemName).IsRequired();

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Price).HasColumnType("decimal(14, 2)");
            });

            modelBuilder.Entity<InvoiceDetailsMaster>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceDate, e.InvoiceNo, e.OrderId, e.OrderDate });

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("EmailID")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(14, 2)");

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.PartysName)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.SettlementAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");
            });

            modelBuilder.Entity<InvoiceMaster>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceDate, e.InvoiceNo, e.OrderId, e.OrderDate, e.ProductId });

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Cgst)
                    .HasColumnName("CGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("EmailID")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Igst)
                    .HasColumnName("IGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.NetSale)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackagingAndForwarding)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.PartysName)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.Quantity).HasDefaultValueSql("('0')");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ReturnedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01 00:00:00:000')");

                entity.Property(e => e.SettlementAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.SettlementDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Sgst)
                    .HasColumnName("SGST")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");
            });

            modelBuilder.Entity<InvoiceProductMaster>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceDate, e.InvoiceNo, e.OrderId, e.OrderDate, e.ProductId });

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.Cgst)
                    .HasColumnName("CGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Cgstperc)
                    .HasColumnName("CGSTPerc")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Igst)
                    .HasColumnName("IGST")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Igstperc)
                    .HasColumnName("IGSTPerc")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.NetSale)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Quantity).HasDefaultValueSql("('0')");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.SettlementAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Sgst)
                    .HasColumnName("SGST")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.Sgstperc)
                    .HasColumnName("SGSTPerc")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShippingCgst)
                    .HasColumnName("Shipping_CGST")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.ShippingIgst)
                    .HasColumnName("Shipping_IGST")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.ShippingNetSale)
                    .HasColumnName("Shipping_NetSale")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.ShippingSgst)
                    .HasColumnName("Shipping_SGST")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.ShippingTotal)
                    .HasColumnName("Shipping_Total")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ShippingUnitPrice)
                    .HasColumnName("Shipping_UnitPrice")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(14, 2)");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<InvoiceSettlementMaster>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceDate, e.InvoiceNo, e.OrderId, e.OrderDate, e.ProductId, e.SettlementDate, e.SrNo });

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.SettlementDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.SrNo).HasDefaultValueSql("('1')");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.SettlementAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<LookupMaster>(entity =>
            {
                entity.HasKey(e => e.LookupId);

                entity.Property(e => e.LookupId)
                    .HasColumnName("LookupID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LookupName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ScreenId).HasColumnName("ScreenID");
            });

            modelBuilder.Entity<ProductDescriptionMaster>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ArtistName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.BlackAndWhitePoster)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Category).HasDefaultValueSql("('0')");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ColorPoster)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.DimensionsInInches)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.FrameMaterial)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Framed)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ImageLink)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Keywords)
                    .IsRequired()
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Mrpprice)
                    .HasColumnName("MRPPrice")
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Orientation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.OtherDimensions)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.OtherFeatures)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.OtherFrameDetails)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PackageContents)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PackagingInformation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PaintingStyle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PaperDepthGsm)
                    .IsRequired()
                    .HasColumnName("PaperDepth_gsm")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PaperFinish)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PaperType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.PosterDimensions)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosterType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosterWeightGrams)
                    .IsRequired()
                    .HasColumnName("PosterWeight_grams")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Publisher)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Shape)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ShippingDuration)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.SocialName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.SubCategory).HasDefaultValueSql("('0')");

                entity.Property(e => e.SupplierImage)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Variant)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.WarrantySummary)
                    .IsRequired()
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.WeightGrams)
                    .IsRequired()
                    .HasColumnName("Weight_grams")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.Width)
                    .HasColumnType("decimal(14, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ProductMaster>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.TotalAvailableQuota).HasDefaultValueSql("('0')");

                entity.Property(e => e.TotalDamage).HasDefaultValueSql("('0')");

                entity.Property(e => e.TotalSale).HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<PurchaseMaster>(entity =>
            {
                entity.HasKey(e => new { e.PurchaseDate, e.ProductId });

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Mrp)
                    .HasColumnName("MRP")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.Pp)
                    .HasColumnName("PP")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.Sp)
                    .HasColumnName("SP")
                    .HasColumnType("decimal(14, 2)");
            });

            modelBuilder.Entity<SalesMaster>(entity =>
            {
                entity.HasKey(e => new { e.SalesDate, e.ProductId, e.InvoiceNo });

                entity.Property(e => e.SalesDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.InvoiceNo).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Price).HasColumnType("decimal(14, 2)");
            });

            modelBuilder.Entity<SubCategoryMaster>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.SubCategoryId });

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<SubLookupMaster>(entity =>
            {
                entity.HasKey(e => new { e.LookupId, e.SubLookupId });

                entity.Property(e => e.LookupId).HasColumnName("LookupID");

                entity.Property(e => e.SubLookupId).HasColumnName("SubLookupID");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.SubLookupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("nchar(50)")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VendorMaster>(entity =>
            {
                entity.HasKey(e => e.VendorId);

                entity.Property(e => e.VendorId)
                    .HasColumnName("VendorID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Gstno)
                    .IsRequired()
                    .HasColumnName("GSTNo")
                    .HasMaxLength(12)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.PanNo)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}