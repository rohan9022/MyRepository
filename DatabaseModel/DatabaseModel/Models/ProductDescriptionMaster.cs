using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class ProductDescriptionMaster
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public int GroupId { get; set; }
        public string Size { get; set; }
        public decimal Mrpprice { get; set; }
        public string PosterType { get; set; }
        public string PosterDimensions { get; set; }
        public string DimensionsInInches { get; set; }
        public string PosterWeightGrams { get; set; }
        public string PackageContents { get; set; }
        public string PackagingInformation { get; set; }
        public string ShippingDuration { get; set; }
        public string Variant { get; set; }
        public string Color { get; set; }
        public string Keywords { get; set; }
        public string SocialName { get; set; }
        public string Publisher { get; set; }
        public string PaperType { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public string ArtistName { get; set; }
        public string PaintingStyle { get; set; }
        public string BlackAndWhitePoster { get; set; }
        public string ColorPoster { get; set; }
        public string PaperFinish { get; set; }
        public string Shape { get; set; }
        public string Orientation { get; set; }
        public string Framed { get; set; }
        public string FrameMaterial { get; set; }
        public string OtherFrameDetails { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string PaperDepthGsm { get; set; }
        public string WeightGrams { get; set; }
        public string OtherDimensions { get; set; }
        public string OtherFeatures { get; set; }
        public string SupplierImage { get; set; }
        public string ImageLink { get; set; }
        public string Note { get; set; }
        public string WarrantySummary { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
