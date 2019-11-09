using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class InvoiceReport
    {
        [DataMember]
        public string BillTo { get; set; }

        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderNo { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string GSTNo { get; set; }

        [DataMember]
        public string PanNo { get; set; }

        [DataMember]
        public ObservableCollection<InvDetails> lstDetails { get; set; }

        [DataMember]
        public string TaxType { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }

        [DataMember]
        public decimal TransportNHandling { get; set; }

        [DataMember]
        public decimal InvoiceAmount { get; set; }

        [DataMember]
        public string AmtInWords { get; set; }

        [DataMember]
        public decimal SettlementAmount { get; set; }

        [DataMember]
        public decimal DifferenceAmount { get; set; }

        [DataMember]
        public string InvoiceStatus { get; set; }

        [DataMember]
        public string CommissionTab { get; set; }
    }

    [DataContract]
    public class InvDetails
    {
        [DataMember]
        public int SrNo { get; set; }

        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int QTY { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public decimal NetSale { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public decimal CGST { get; set; }

        [DataMember]
        public decimal SGST { get; set; }

        [DataMember]
        public decimal IGST { get; set; }

        [DataMember]
        public decimal CGSTPerc { get; set; }

        [DataMember]
        public decimal SGSTPerc { get; set; }

        [DataMember]
        public decimal IGSTPerc { get; set; }

        [DataMember]
        public decimal Shipping_UnitPrice { get; set; }

        [DataMember]
        public decimal Shipping_NetSale { get; set; }

        [DataMember]
        public decimal Shipping_CGST { get; set; }

        [DataMember]
        public decimal Shipping_SGST { get; set; }

        [DataMember]
        public decimal Shipping_IGST { get; set; }

        [DataMember]
        public decimal Shipping_Total { get; set; }

        [DataMember]
        public decimal SettlementAmount { get; set; }

        [DataMember]
        public decimal DifferenceAmount { get; set; }

        [DataMember]
        public int Status { get; set; }
    }
}