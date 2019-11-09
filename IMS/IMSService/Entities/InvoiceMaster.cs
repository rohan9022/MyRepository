using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class InvoiceMaster
    {
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public DateTime InvoiceDate { get; set; }
        [DataMember]
        public string PartyName { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public string ContactNo { get; set; }
        [DataMember]
        public int Vendor { get; set; }
        [DataMember]
        public ObservableCollection<InvoiceProduct> lstInvoiceProduct { get; set; }
    }
}