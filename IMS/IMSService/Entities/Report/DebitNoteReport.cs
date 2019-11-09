using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class DebitNoteReport
    {
        [DataMember]
        public string DrNoteNo { get; set; }
        [DataMember]
        public DateTime DrNoteDate { get; set; }
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string InvoiceStatus { get; set; }
        [DataMember]
        public CompanyMaster CompanyDetails { get; set; }
        [DataMember]
        public ObservableCollection<InvDetails> lstDetails { get; set; }
    }
}