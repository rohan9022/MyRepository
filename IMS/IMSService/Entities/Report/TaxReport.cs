using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class TaxReport
    {
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public decimal CGSTRate { get; set; }
        [DataMember]
        public decimal SGSTRate { get; set; }
        [DataMember]
        public decimal IGSTRate { get; set; }
        [DataMember]
        public decimal CGST { get; set; }
        [DataMember]
        public decimal SGST { get; set; }
        [DataMember]
        public decimal IGST { get; set; }
        [DataMember]
        public decimal Shipping_CGST { get; set; }
        [DataMember]
        public decimal Shipping_SGST { get; set; }
        [DataMember]
        public decimal Shipping_IGST { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public decimal Shipping_Total { get; set; }
    }
}