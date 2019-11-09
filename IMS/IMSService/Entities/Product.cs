using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int TotalSale { get; set; }
        [DataMember]
        public int TotalQuota { get; set; }
        [DataMember]
        public int TotalDamage { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string PosterType { get; set; }
        [DataMember]
        public string PosterDimensions { get; set; }
        [DataMember]
        public string DimensionsInInches { get; set; }
        [DataMember]
        public string PosterWeight { get; set; }
        [DataMember]
        public string PackageContents { get; set; }
        [DataMember]
        public string PackageInformation { get; set; }
        [DataMember]
        public string ShippingDuration { get; set; }
        [DataMember]
        public string Variant { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Keywords { get; set; }
        [DataMember]
        public string SocialName { get; set; }
        [DataMember]
        public string Publisher { get; set; }
        [DataMember]
        public string PaperType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Category { get; set; }
        [DataMember]
        public int SubCategory { get; set; }
        [DataMember]
        public string ArtistName { get; set; }
        [DataMember]
        public string PaintingStyle { get; set; }
        [DataMember]
        public string BlackWhitePoster { get; set; }
        [DataMember]
        public string ColorPoster { get; set; }
        [DataMember]
        public string PaperFinish { get; set; }
        [DataMember]
        public string Shape { get; set; }
        [DataMember]
        public string Orientation { get; set; }
        [DataMember]
        public string Framed { get; set; }
        [DataMember]
        public string FrameMaterial { get; set; }
        [DataMember]
        public string OtherFrameDetails { get; set; }
        [DataMember]
        public decimal Width { get; set; }
        [DataMember]
        public decimal Height { get; set; }
        [DataMember]
        public string PaperDepth { get; set; }
        [DataMember]
        public string Weight { get; set; }
        [DataMember]
        public string OtherDimensions { get; set; }
        [DataMember]
        public string OtherFeatures { get; set; }
        [DataMember]
        public string SupplierImage { get; set; }
        [DataMember]
        public string ImageLink { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string WarrantySummary { get; set; }
    }
}