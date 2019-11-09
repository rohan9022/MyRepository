using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class CreditNoteReport
    {
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public string CrNoteNo { get; set; }
        [DataMember]
        public DateTime CrNoteDate { get; set; }
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string InvoiceStatus { get; set; }
        [DataMember]
        public string BillTo { get; set; }
        [DataMember]
        public CompanyMaster CompanyDetails { get; set; }
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
        public ObservableCollection<InvDetails> lstDetails { get; set; }
    }
}