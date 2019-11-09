using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class InvoiceDetailsService
    {
        public InvoiceDetailsService()
        {
            ICustomer = new List<Customer>();
            IProduct = new List<ProductDetails>();
        }

        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public decimal InvoiceAmount { get; set; }

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
        public decimal Shipping_Total { get; set; }

        [DataMember]
        public decimal SettlementAmount { get; set; }

        [DataMember]
        public int Vendor { get; set; }

        [DataMember]
        public List<Customer> ICustomer { get; set; }

        [DataMember]
        public List<ProductDetails> IProduct { get; set; }

        [DataMember]
        public int FinalStatus { get; set; }
    }

    [DataContract]
    public class ProductDetails
    {
        public ProductDetails()
        {
            ISettlement = new List<SettlementDetails>();
        }

        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string ProductID { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public decimal NetSale { get; set; }

        [DataMember]
        public decimal CGST { get; set; }

        [DataMember]
        public decimal SGST { get; set; }

        [DataMember]
        public decimal IGST { get; set; }

        [DataMember]
        public decimal Total { get; set; }

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
        public int Status { get; set; }

        [DataMember]
        public string TaxGroup { get; set; }

        [DataMember]
        public string StringStatus { get; set; }

        [DataMember]
        public List<SettlementDetails> ISettlement { get; set; }
    }

    [DataContract]
    public class SettlementDetails
    {
        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string ProductID { get; set; }

        [DataMember]
        public DateTime SettlementDate { get; set; }

        [DataMember]
        public int SrNo { get; set; }

        [DataMember]
        public decimal SettlementAmount { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string StringStatus { get; set; }
    }

    [DataContract]
    public class Customer
    {
        [DataMember]
        public string PartyName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string EmailID { get; set; }

        [DataMember]
        public string MobileNo { get; set; }
    }

    [DataContract]
    public class InvoiceCustomerDetails
    {
        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string PartyName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string EmailID { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public string FinalStatus { get; set; }
    }
}