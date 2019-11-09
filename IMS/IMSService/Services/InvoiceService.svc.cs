using IMSLibrary;
using IMSService.Entities;
using IMSLibrary.Helper;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using IMSService.Helper;

namespace IMSService.Services
{
    [ServiceContract]
    public interface IInvoiceService
    {
        #region ==Invoice==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<InvoiceDetailsService> GetInvoiceDetails(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string OrderID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool UpdateInvoiceDetails(InvoiceDetailsService invoiceDetails, Customer customerDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool UpdateInvoiceProduct(ProductDetails productDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool UpdateInvoiceSettlement(SettlementDetails settlementDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteInvoiceDetails(InvoiceDetailsService invoiceDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteInvoiceProduct(ProductDetails productDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteInvoiceSettlement(SettlementDetails settlementDetails, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        decimal FindGstRate(string ProductID, string TaxType);

        #endregion ==Invoice==

        #region ==Patches==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<InvoiceCustomerDetails> InvoiceDetailsPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<ProductDetails> InvoiceProductPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string ProductID, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<SettlementDetails> InvoiceSettlementPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string ProductID, DateTime FromSettlementDate, DateTime ToSettlementDate, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        Tuple<string, string, bool> UploadPatchSheet(byte[] sheetData, string FileName, int UserID, int SheetType);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        Tuple<string, string, bool> UploadSheetUpdateDelete(byte[] sheetData, string FileName, int UserID, int SheetType);

        #endregion ==Patches==
    }

    public class InvoiceService : IInvoiceService
    {
        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[Const.ImsConnectionString].ConnectionString;
        }

        private readonly DateTime BlankDate = new DateTime(1900, 01, 01);

        #region ==Invoice==

        public List<InvoiceDetailsService> GetInvoiceDetails(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string OrderID)
        {
            try
            {
                int SearchMode = 0;
                if (!string.IsNullOrEmpty(OrderID)) SearchMode = 1;
                else
                {
                    if (FromInvoiceNo > 0 && ToInvoiceNo > 0) SearchMode = 2;
                    if (FromInvoiceDate.Date > BlankDate && ToInvoiceDate.Date > BlankDate) SearchMode = 3;
                    if (FromInvoiceNo > 0 && ToInvoiceNo > 0 && FromInvoiceDate.Date > BlankDate && ToInvoiceDate.Date > BlankDate) SearchMode = 4;
                }

                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intFromInvoiceNo", FromInvoiceNo),
                    [1] = new SqlParameter("@intToInvoiceNo", ToInvoiceNo),
                    [2] = new SqlParameter("@datetimeFromInvoiceDate", FromInvoiceDate),
                    [3] = new SqlParameter("@datetimeToInvoiceDate", ToInvoiceDate),
                    [4] = new SqlParameter("@nvarcharOrderID", OrderID),
                    [5] = new SqlParameter("@intSearchMode", SearchMode)
                };
                DataSet ds = SqlUtility.ExecuteQueryWithDS(GetConnectionString(), "sp_im_GetInvoiceDetails", sqlParam);
                List<InvoiceDetailsService> lstInvDetails = new List<InvoiceDetailsService>();
                List<ProductDetails> lstProduct = new List<ProductDetails>();
                List<SettlementDetails> lstSettlement = new List<SettlementDetails>();
                DateTime InvDate = new DateTime();
                DateTime OrdDate = new DateTime();
                int InvNo = 0;
                string OrdID = string.Empty;
                string PrdID = string.Empty;
                //Settlement
                DataTable dt = ds.Tables[2];
                foreach (DataRow dr in dt.Rows)
                {
                    lstSettlement.Add(new SettlementDetails
                    {
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                        OrderID = Converter.ToString(dr["OrderID"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        ProductID = Converter.ToString(dr["ProductID"]),
                        SettlementDate = Converter.ToDateTime(dr["SettlementDate"]),
                        SrNo = Converter.ToInt(dr["SrNo"]),
                        SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]),
                        Status = Converter.ToInt(dr["Status"])
                    });
                }

                //Product
                dt = ds.Tables[1];
                foreach (DataRow dr in dt.Rows)
                {
                    InvDate = Converter.ToDateTime(dr["InvoiceDate"]);
                    OrdDate = Converter.ToDateTime(dr["OrderDate"]);
                    InvNo = Converter.ToInt(dr["InvoiceNo"]);
                    OrdID = Converter.ToString(dr["OrderID"]);
                    PrdID = Converter.ToString(dr["ProductID"]);

                    lstProduct.Add(new ProductDetails
                    {
                        InvoiceNo = InvNo,
                        InvoiceDate = InvDate,
                        OrderID = OrdID,
                        OrderDate = OrdDate,
                        ProductID = PrdID,
                        Quantity = Converter.ToInt(dr["Quantity"]),
                        Rate = Converter.ToDecimal(dr["Rate"]),
                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        NetSale = Converter.ToDecimal(dr["NetSale"]),

                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),

                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),

                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),

                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),

                        Total = Converter.ToDecimal(dr["Total"]),
                        SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]),
                        Status = Converter.ToInt(dr["Status"]),
                        ISettlement = lstSettlement.Where(s => s.InvoiceNo == InvNo && s.OrderID == OrdID && s.InvoiceDate == InvDate && s.OrderDate == OrdDate && s.ProductID == PrdID).ToList()
                    });
                }

                //Invoice
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    InvDate = Converter.ToDateTime(dr["InvoiceDate"]);
                    OrdDate = Converter.ToDateTime(dr["OrderDate"]);
                    InvNo = Converter.ToInt(dr["InvoiceNo"]);
                    OrdID = Converter.ToString(dr["OrderID"]);

                    List<Customer> lstCust = new List<Customer>();
                    lstCust.Add(new Customer
                    {
                        Address = Converter.ToString(dr["Address"]),
                        EmailID = Converter.ToString(dr["EmailID"]),
                        MobileNo = Converter.ToString(dr["MobileNo"]),
                        PartyName = Converter.ToString(dr["PartysName"])
                    });

                    List<ProductDetails> lstInvoiceProduct = lstProduct.Where(s => s.InvoiceNo == InvNo && s.OrderID == OrdID && s.InvoiceDate == InvDate && s.OrderDate == OrdDate).ToList();

                    lstInvDetails.Add(new InvoiceDetailsService
                    {
                        InvoiceNo = InvNo,
                        InvoiceDate = InvDate,
                        OrderID = OrdID,
                        OrderDate = OrdDate,
                        InvoiceAmount = Converter.ToDecimal(dr["InvoiceAmount"]),

                        CGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.CGST),
                        SGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.SGST),
                        IGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.IGST),

                        Shipping_CGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.Shipping_CGST),
                        Shipping_SGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.Shipping_SGST),
                        Shipping_IGST = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.Shipping_IGST),
                        Shipping_Total = lstInvoiceProduct.Where(p => p.Status != 8).Sum(p => p.Shipping_Total),

                        Vendor = Converter.ToInt(dr["VendorID"]),
                        SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]),
                        FinalStatus = Converter.ToInt(dr["FinalStatus"]),
                        ICustomer = lstCust,
                        IProduct = lstInvoiceProduct
                    });
                }

                return lstInvDetails;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateInvoiceDetails(InvoiceDetailsService invoiceDetails, Customer customerDetails, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", invoiceDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", invoiceDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", invoiceDetails.OrderDate),
                    [4] = new SqlParameter("@nvarcharPartysName", customerDetails.PartyName),
                    [5] = new SqlParameter("@nvarcharAddress", customerDetails.Address),
                    [6] = new SqlParameter("@nvarcharEmailID", customerDetails.EmailID),
                    [7] = new SqlParameter("@nvarcharMobileNo", customerDetails.MobileNo),
                    [8] = new SqlParameter("@nvarcharVendor", invoiceDetails.Vendor),
                    [9] = new SqlParameter("@nvarcharFinalStatus", invoiceDetails.FinalStatus),
                    [10] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };

                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceDetails", sqlParam);

                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateInvoiceProduct(ProductDetails productDetails, int UserID)
        {
            try
            {
                if (productDetails.IGST > 0.00M)
                {
                    productDetails.IGSTPerc = FindGstRate(productDetails.ProductID, "IGST");
                    productDetails.UnitPrice = Math.Round(((productDetails.Rate / (productDetails.IGSTPerc + 100)) * 100), 2);
                    productDetails.IGST = (productDetails.Rate - productDetails.UnitPrice) * productDetails.Quantity;

                    if (productDetails.Shipping_Total > 0.00M)
                    {
                        productDetails.Shipping_UnitPrice = Math.Round(((productDetails.Shipping_Total / (productDetails.IGSTPerc + 100)) * 100), 2);
                        productDetails.Shipping_IGST = productDetails.Shipping_Total - productDetails.Shipping_UnitPrice;

                        productDetails.Shipping_UnitPrice = (productDetails.Shipping_Total - productDetails.Shipping_IGST) / productDetails.Quantity;
                        productDetails.Shipping_NetSale = productDetails.Shipping_UnitPrice * productDetails.Quantity;
                    }
                }
                else
                {
                    productDetails.CGSTPerc = FindGstRate(productDetails.ProductID, "CGST");
                    productDetails.UnitPrice = Math.Round(((productDetails.Rate / (productDetails.CGSTPerc + 100)) * 100), 2);
                    productDetails.CGST = (productDetails.Rate - productDetails.UnitPrice) * productDetails.Quantity;

                    productDetails.SGSTPerc = FindGstRate(productDetails.ProductID, "SGST");
                    productDetails.UnitPrice = Math.Round(((productDetails.Rate / (productDetails.SGSTPerc + 100)) * 100), 2);
                    productDetails.SGST = (productDetails.Rate - productDetails.UnitPrice) * productDetails.Quantity;

                    productDetails.UnitPrice = (productDetails.Rate * productDetails.Quantity) - productDetails.CGST - productDetails.SGST;

                    if (productDetails.Shipping_Total > 0.00M)
                    {
                        productDetails.Shipping_UnitPrice = Math.Round(((productDetails.Shipping_Total / (productDetails.CGSTPerc + 100)) * 100), 2);
                        productDetails.Shipping_CGST = productDetails.Shipping_Total - productDetails.Shipping_UnitPrice;

                        productDetails.Shipping_UnitPrice = Math.Round(((productDetails.Shipping_Total / (productDetails.SGSTPerc + 100)) * 100), 2);
                        productDetails.Shipping_SGST = productDetails.Shipping_Total - productDetails.Shipping_UnitPrice;

                        productDetails.Shipping_UnitPrice = (productDetails.Shipping_Total - productDetails.Shipping_CGST - productDetails.Shipping_SGST) / productDetails.Quantity;
                        productDetails.Shipping_NetSale = productDetails.Shipping_UnitPrice * productDetails.Quantity;
                    }
                }

                productDetails.NetSale = productDetails.UnitPrice * productDetails.Quantity;
                productDetails.Total = (productDetails.Rate * productDetails.Quantity) + Math.Round(productDetails.Shipping_Total, 2);

                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", productDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", productDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", productDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", productDetails.OrderDate),
                    [4] = new SqlParameter("@nvarcharProductID", productDetails.ProductID),

                    [5] = new SqlParameter("@decimalRate", productDetails.Rate),
                    [6] = new SqlParameter("@intQuantity", productDetails.Quantity),
                    [7] = new SqlParameter("@decimalTotal", productDetails.Total),
                    [8] = new SqlParameter("@decimalNetSale", productDetails.NetSale),
                    [9] = new SqlParameter("@decimalCGST", productDetails.CGST),
                    [10] = new SqlParameter("@decimalSGST", productDetails.SGST),
                    [11] = new SqlParameter("@decimalIGST", productDetails.IGST),
                    [12] = new SqlParameter("@decimalUnitPrice", productDetails.UnitPrice),

                    [13] = new SqlParameter("@decimalShipping_Total", productDetails.Shipping_Total),
                    [14] = new SqlParameter("@decimalShipping_CGST", productDetails.Shipping_CGST),
                    [15] = new SqlParameter("@decimalShipping_SGST", productDetails.Shipping_SGST),
                    [16] = new SqlParameter("@decimalShipping_IGST", productDetails.Shipping_IGST),
                    [17] = new SqlParameter("@decimalShipping_UnitPrice", productDetails.Shipping_UnitPrice),
                    [18] = new SqlParameter("@decimalShipping_NetSale", productDetails.Shipping_NetSale),

                    [19] = new SqlParameter("@decimalCGSTPerc", productDetails.CGSTPerc),
                    [20] = new SqlParameter("@decimalSGSTPerc", productDetails.SGSTPerc),
                    [21] = new SqlParameter("@decimalIGSTPerc", productDetails.IGSTPerc),

                    [22] = new SqlParameter("@nvarcharStatus", productDetails.Status),
                    [23] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };

                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceProduct", sqlParam);

                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateInvoiceSettlement(SettlementDetails settlementDetails, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", settlementDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", settlementDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", settlementDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", settlementDetails.OrderDate),
                    [4] = new SqlParameter("@nvarcharProductID", settlementDetails.ProductID),
                    [5] = new SqlParameter("@datetimeSettlementDate", settlementDetails.SettlementDate),
                    [6] = new SqlParameter("@decimalSettlementAmount", settlementDetails.SettlementAmount),
                    [7] = new SqlParameter("@nvarcharStatus", settlementDetails.Status),
                    [8] = new SqlParameter("@intCreatedModifiedBy", UserID),
                    [9] = new SqlParameter("@intSrNo", settlementDetails.SrNo)
                };
                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceSettlement", sqlParam);
                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteInvoiceDetails(InvoiceDetailsService invoiceDetails, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", invoiceDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", invoiceDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", invoiceDetails.OrderDate)
                };
                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceDetails", sqlParam);
                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteInvoiceProduct(ProductDetails productDetails, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", productDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", productDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", productDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", productDetails.OrderDate),
                    [4] = new SqlParameter("@nvarcharProductID", productDetails.ProductID)
                };
                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceProduct", sqlParam);
                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteInvoiceSettlement(SettlementDetails settlementDetails, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@datetimeInvoiceDate", settlementDetails.InvoiceDate),
                    [1] = new SqlParameter("@intInvoiceNo", settlementDetails.InvoiceNo),
                    [2] = new SqlParameter("@nvarcharOrderID", settlementDetails.OrderID),
                    [3] = new SqlParameter("@datetimeOrderDate", settlementDetails.OrderDate),
                    [4] = new SqlParameter("@nvarcharProductID", settlementDetails.ProductID),
                    [5] = new SqlParameter("@datetimeSettlementDate", settlementDetails.SettlementDate),
                    [6] = new SqlParameter("@intSrNo", settlementDetails.SrNo)
                };
                int Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceSettlement", sqlParam);
                if (Result >= Const.Success) return true;
                if (Result == Const.InvoiceDetailsNotFound)
                {
                    throw new FaultException("Invoice Details Not Found");
                }
                if (Result == Const.Error)
                {
                    throw new FaultException("Error");
                }
                if (Result == Const.VendorIDNotFound)
                {
                    throw new FaultException("Vendor ID Not Found");
                }
                if (Result == Const.InvoiceStatusNotFound)
                {
                    throw new FaultException("Invoice Status Not Found");
                }
                if (Result == Const.NetworkError)
                {
                    throw new FaultException("Network Error");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Invoice==

        #region ==Patches==

        public List<InvoiceCustomerDetails> InvoiceDetailsPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, int UserID)
        {
            try
            {
                List<InvoiceCustomerDetails> lstInvoiceDetails = new List<InvoiceCustomerDetails>();
                string query = "Select InvoiceDate,InvoiceNo,OrderDate,OrderID,PartysName,Address,EmailID,MobileNo,(Select VendorName From VendorMaster Where VendorID=IDM.VendorID) As Vendor, (Select SubLookupName From SubLookupMaster Where LookupID = 14 And SubLookupID=IDM.FinalStatus) As FinalStatus From InvoiceDetailsMaster IDM Where ";
                if (FromInvoiceNo > 0 && ToInvoiceNo > 0)
                {
                    query += "InvoiceNo >= " + FromInvoiceNo + " And InvoiceNo <= " + ToInvoiceNo;
                }
                if (FromInvoiceDate.Date > BlankDate && ToInvoiceDate.Date > BlankDate)
                {
                    query += "InvoiceDate >= '" + FromInvoiceDate.Date + "' And InvoiceDate <= '" + ToInvoiceDate.Date + "'";
                }

                SqlConnection sqlCon = new SqlConnection(GetConnectionString());
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        InvoiceCustomerDetails invoiceDetails = new InvoiceCustomerDetails();
                        invoiceDetails.InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]);
                        invoiceDetails.InvoiceNo = Converter.ToInt(dr["InvoiceNo"]);
                        invoiceDetails.OrderDate = Converter.ToDateTime(dr["OrderDate"]);
                        invoiceDetails.OrderID = Converter.ToString(dr["OrderID"]);
                        invoiceDetails.PartyName = Converter.ToString(dr["PartysName"]);
                        invoiceDetails.Address = Converter.ToString(dr["Address"]);
                        invoiceDetails.EmailID = Converter.ToString(dr["EmailID"]);
                        invoiceDetails.MobileNo = Converter.ToString(dr["MobileNo"]);
                        invoiceDetails.Vendor = Converter.ToString(dr["Vendor"]);
                        invoiceDetails.FinalStatus = Converter.ToString(dr["FinalStatus"]);
                        lstInvoiceDetails.Add(invoiceDetails);
                    }
                }
                dr.Close();
                cmd.Dispose();
                sqlCon.Close();
                return lstInvoiceDetails;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<ProductDetails> InvoiceProductPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string ProductID, int UserID)
        {
            try
            {
                List<ProductDetails> lstInvoiceProduct = new List<ProductDetails>();
                string query = "Select InvoiceDate,InvoiceNo,OrderDate,OrderID,ProductID,Quantity,Rate,Shipping_Total,IGST,CGST,SGST,(Select SubLookupName From SubLookupMaster Where LookupID = 14 And SubLookupID=IPM.Status) As Status From InvoiceProductMaster IPM Where ";
                bool condition = false;
                if (FromInvoiceNo > 0 && ToInvoiceNo > 0)
                {
                    query += "InvoiceNo >= " + FromInvoiceNo + " And InvoiceNo <= " + ToInvoiceNo;
                    condition = true;
                }
                if (FromInvoiceDate.Date > BlankDate && ToInvoiceDate.Date > BlankDate)
                {
                    if (condition)
                    {
                        query += " And ";
                    }
                    query += "InvoiceDate >= '" + FromInvoiceDate.Date + "' And InvoiceDate <= '" + ToInvoiceDate.Date + "'";
                    condition = true;
                }
                if (!string.IsNullOrEmpty(ProductID))
                {
                    if (condition)
                    {
                        query += " And ";
                    }
                    query += "ProductID = '" + ProductID + "'";
                }
                SqlConnection sqlCon = new SqlConnection(GetConnectionString());
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ProductDetails invoiceProduct = new ProductDetails
                        {
                            InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                            InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                            OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                            OrderID = Converter.ToString(dr["OrderID"]),
                            ProductID = Converter.ToString(dr["ProductID"]),
                            Quantity = Converter.ToInt(dr["Quantity"]),
                            Rate = Converter.ToDecimal(dr["Rate"]),
                            Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),
                            CGST = Converter.ToDecimal(dr["CGST"]),
                            SGST = Converter.ToDecimal(dr["SGST"]),
                            IGST = Converter.ToDecimal(dr["IGST"]),
                            TaxGroup = "CSGST"
                        };
                        if (invoiceProduct.IGST > 0.00M)
                        {
                            invoiceProduct.TaxGroup = "IGST";
                        }
                        invoiceProduct.StringStatus = Converter.ToString(dr["Status"]);
                        lstInvoiceProduct.Add(invoiceProduct);
                    }
                }
                dr.Close();
                cmd.Dispose();
                sqlCon.Close();
                return lstInvoiceProduct;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<SettlementDetails> InvoiceSettlementPatch(int FromInvoiceNo, int ToInvoiceNo, DateTime FromInvoiceDate, DateTime ToInvoiceDate, string ProductID, DateTime FromSettlementDate, DateTime ToSettlementDate, int UserID)
        {
            try
            {
                List<SettlementDetails> lstInvoiceSettlement = new List<SettlementDetails>();
                string query = "Select InvoiceDate,InvoiceNo,OrderDate,OrderID,ProductID,SettlementDate,SrNo,SettlementAmount,(Select SubLookupName From SubLookupMaster Where LookupID = 14 And SubLookupID=ISM.Status) As Status From InvoiceSettlementMaster ISM Where ";
                bool condition = false;
                if (FromInvoiceNo > 0 && ToInvoiceNo > 0)
                {
                    query += "InvoiceNo >= " + FromInvoiceNo + " And InvoiceNo <= " + ToInvoiceNo;
                    condition = true;
                }
                if (FromInvoiceDate.Date > BlankDate && ToInvoiceDate.Date > BlankDate)
                {
                    if (condition)
                    {
                        query += " And ";
                    }
                    query += "InvoiceDate >= '" + FromInvoiceDate.Date + "' And InvoiceDate <= '" + ToInvoiceDate.Date + "'";
                    condition = true;
                }
                if (!string.IsNullOrEmpty(ProductID))
                {
                    if (condition)
                    {
                        query += " And ";
                    }
                    query += "ProductID = '" + ProductID + "'";
                }
                if (FromSettlementDate.Date > BlankDate && ToSettlementDate.Date > BlankDate)
                {
                    if (condition)
                    {
                        query += " And ";
                    }
                    query += "SettlementDate >= '" + FromSettlementDate.Date + "' And SettlementDate <= '" + ToSettlementDate.Date + "'";
                }
                ////return GenerateExcel(query, "InvoiceSettlementDataset", "InvoiceSettlementPatch", UserID);
                SqlConnection sqlCon = new SqlConnection(GetConnectionString());
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SettlementDetails invoiceSettlement = new SettlementDetails();
                        invoiceSettlement.InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]);
                        invoiceSettlement.InvoiceNo = Converter.ToInt(dr["InvoiceNo"]);
                        invoiceSettlement.OrderDate = Converter.ToDateTime(dr["OrderDate"]);
                        invoiceSettlement.OrderID = Converter.ToString(dr["OrderID"]);
                        invoiceSettlement.ProductID = Converter.ToString(dr["ProductID"]);
                        invoiceSettlement.SettlementDate = Converter.ToDateTime(dr["SettlementDate"]);
                        invoiceSettlement.SrNo = Converter.ToInt(dr["SrNo"]);
                        invoiceSettlement.SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]);
                        invoiceSettlement.StringStatus = Converter.ToString(dr["Status"]);
                        lstInvoiceSettlement.Add(invoiceSettlement);
                    }
                }
                dr.Close();
                cmd.Dispose();
                sqlCon.Close();
                return lstInvoiceSettlement;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Patches==

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
        private Tuple<string, string, bool> GenerateExcel(string Query, string DatasetName, string ReportName, int UserID)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(Query);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = new SqlConnection(GetConnectionString());
            da.SelectCommand = cmd;
            da.Fill(ds, DatasetName);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(DatasetName, ds.Tables[0]));
            reportViewer.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\Reports\\" + ReportName + ".rdlc";
            reportViewer.LocalReport.Refresh();

            string dt = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            string FolderPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\Patches\\Generated\\" + dt + "\\" + UserID;
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            byte[] Bytes = reportViewer.LocalReport.Render(format: "Excel", deviceInfo: "");
            string FileName = Guid.NewGuid().ToString() + ReportName + ".xls";
            using (FileStream stream = new FileStream(FolderPath + "\\" + FileName, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }
            return new Tuple<string, string, bool>("Patches\\Generated\\" + dt + "\\" + UserID, FileName, true);
        }

        public decimal FindGstRate(string ProductID, string TaxType)
        {
            try
            {
                decimal Rate = 0M;
                int gstType = 0;
                if (TaxType == "CGST")
                {
                    gstType = 1;
                }
                else if (TaxType == "SGST")
                {
                    gstType = 2;
                }
                else if (TaxType == "IGST")
                {
                    gstType = 3;
                }
                decimal result = 0M;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                sqlParam[0] = new SqlParameter("@nvarcharProductID", ProductID);
                sqlParam[1] = new SqlParameter("@intGstType", gstType);
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_FindGstRate", sqlParam);
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    result = Converter.ToDecimal(dr["Rate"]);
                }
                if (result > 0)
                {
                    return result;
                }
                return Rate;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public Tuple<string, string, bool> UploadPatchSheet(byte[] sheetData, string FileName, int UserID, int SheetType)
        {
            try
            {
                string ErrorCsvFileName = string.Empty, ErrorFolderPath = string.Empty, MainFolderPath = string.Empty, MainFileName = string.Empty;
                string dt = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

                if (SheetType == Const.InvoiceDetails) { ErrorFolderPath = "Patches\\Upload\\InvoiceDetails\\" + dt + "\\" + UserID; }
                if (SheetType == Const.InvoiceProduct) { ErrorFolderPath = "Patches\\Upload\\InvoiceProduct\\" + dt + "\\" + UserID; }
                if (SheetType == Const.InvoiceSettlement) { ErrorFolderPath = "Patches\\Upload\\InvoiceSettlement\\" + dt + "\\" + UserID; }

                string destFilePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + ErrorFolderPath;
                MainFolderPath = destFilePath;
                if (!Directory.Exists(MainFolderPath))
                {
                    Directory.CreateDirectory(MainFolderPath);
                }
                MainFileName = ErrorCsvFileName = Guid.NewGuid().ToString() + FileName;
                string FilePath = destFilePath + "\\" + MainFileName;
                System.IO.File.WriteAllBytes(FilePath, sheetData);

                string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
                using (OleDbConnection OleDbCon = new OleDbConnection(con))
                {
                    OleDbCon.Open();
                    string query = string.Empty;
                    if (SheetType == Const.InvoiceDetails)
                    {
                        query = "Select * From [InvoiceDetails$] Where ModeType <> ''";
                    }
                    else if (SheetType == Const.InvoiceProduct)
                    {
                        query = "Select * From [InvoiceProduct$] Where ModeType <> ''";
                    }
                    else if (SheetType == Const.InvoiceSettlement)
                    {
                        query = "Select * From [InvoiceSettlement$] Where ModeType <> ''";
                    }
                    using (OleDbCommand oleDbCommand = new OleDbCommand(query, OleDbCon))
                    {
                        OleDbDataReader dr = oleDbCommand.ExecuteReader();
                        ErrorCsvFileName = ErrorCsvFileName.Replace(".xlsx", ".csv");
                        ErrorCsvFileName = ErrorCsvFileName.Replace(".xls", ".csv");
                        ErrorFolderPath += "\\Error\\";
                        if (!Directory.Exists(MainFolderPath + "\\Error\\"))
                        {
                            Directory.CreateDirectory(MainFolderPath + "\\Error\\");
                        }
                        StreamWriter sw = new StreamWriter(MainFolderPath + "\\Error\\" + ErrorCsvFileName);
                        if (SheetType == Const.InvoiceDetails) sw.WriteLine("InvoiceDate,InvoiceNo,OrderID,OrderDate,PartysName,Address,EmailID,MobileNo,Vendor,FinalStatus,ModeType,ErrorMessage");
                        if (SheetType == Const.InvoiceProduct) sw.WriteLine("InvoiceDate,InvoiceNo,OrderID,OrderDate,ProductID,Quantity,Rate,ShippingAndHandling,TaxGroup,Status,ModeType,ErrorMessage");
                        if (SheetType == Const.InvoiceSettlement) sw.WriteLine("InvoiceDate,InvoiceNo,OrderID,OrderDate,ProductID,SettlementDate,SettlementAmount,Status,ModeType,ErrorMessage");

                        while (dr.Read())
                        {
                            int invoiceNo = 0;
                            string orderID = string.Empty;
                            DateTime invoiceDate;
                            DateTime orderDate;
                            string ProductID = string.Empty;
                            string ErrorMsg = string.Empty;
                            string modType = string.Empty;
                            int Result = 0;

                            invoiceNo = Converter.ToInt(dr["InvoiceNo"]);
                            orderID = Converter.ToString(dr["OrderID"]);
                            orderDate = Converter.ToDateTime(dr["OrderDate"]);
                            invoiceDate = Converter.ToDateTime(dr["InvoiceDate"]);
                            modType = Converter.ToString(dr["ModeType"]).ToUpper();

                            if (invoiceNo <= 0 || string.IsNullOrEmpty(orderID) || invoiceDate <= new DateTime(1900, 01, 01) || orderDate <= new DateTime(1900, 01, 01)) continue;

                            try
                            {
                                if (SheetType == Const.InvoiceDetails)
                                {
                                    string PartyName, Address, EmailID, MobileNo, Vendor, FinalStatus;

                                    PartyName = Converter.ToString(dr["PartysName"]);
                                    Address = Converter.ToString(dr["Address"]);
                                    EmailID = Converter.ToString(dr["EmailID"]);
                                    MobileNo = Converter.ToString(dr["MobileNo"]);
                                    Vendor = Converter.ToString(dr["Vendor"]);
                                    FinalStatus = Converter.ToStatusString(dr["FinalStatus"]);

                                    if (modType == "UPDATE")
                                    {
                                        SqlConnection sqlCon = new SqlConnection(GetConnectionString());
                                        SqlCommand cmd;
                                        string Query = "Update InvoiceDetailsMaster Set ";
                                        if (!string.IsNullOrEmpty(PartyName)) Query += "PartysName='" + PartyName + "',";
                                        if (!string.IsNullOrEmpty(Address)) Query += "Address='" + Address + "',";
                                        if (!string.IsNullOrEmpty(EmailID)) Query += "EmailID='" + EmailID + "',";
                                        if (!string.IsNullOrEmpty(MobileNo)) Query += "MobileNo='" + MobileNo + "',";
                                        if (!string.IsNullOrEmpty(Vendor))
                                        {
                                            int VendorID = GetVendorID(Vendor);
                                            if (VendorID != 0)
                                            {
                                                Query += "VendorID=" + VendorID + ",";
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(FinalStatus))
                                        {
                                            int StatusID = GetStatusID(FinalStatus);
                                            if (StatusID != 0)
                                            {
                                                Query += "Status=" + StatusID + ",";
                                            }
                                        }
                                        Query += "ModifiedBy = " + UserID + ", ModifiedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
                                        Query += "Where InvoiceDate = '" + invoiceDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' And OrderDate = '" + orderDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'  And ";
                                        Query += "InvoiceNo = " + invoiceNo + " And OrderID = '" + orderID + "'";
                                        cmd = new SqlCommand(Query, sqlCon);
                                        sqlCon.Open();
                                        Result = cmd.ExecuteNonQuery();
                                        sqlCon.Close();
                                    }
                                    else if (modType == "DELETE")
                                    {
                                        Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                                        sqlParam[0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate);
                                        sqlParam[1] = new SqlParameter("@intInvoiceNo", invoiceNo);
                                        sqlParam[2] = new SqlParameter("@nvarcharOrderID", orderID);
                                        sqlParam[3] = new SqlParameter("@datetimeOrderDate", orderDate);
                                        Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceDetails", sqlParam);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    if (Result >= Const.Success) ErrorMsg = string.Empty;
                                    if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice Details Not Found";
                                    if (Result == Const.Error) ErrorMsg = "Error";
                                    if (Result == Const.VendorIDNotFound) ErrorMsg = "Vendor ID Not Found";
                                    if (Result == Const.InvoiceStatusNotFound) ErrorMsg = "Invoice Status Not Found";
                                    if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                    sw.WriteLine(invoiceDate.ToString("dd-MM-yyyy") + "," + invoiceNo + "," + orderID + "," + orderDate.ToString("dd-MM-yyyy") + "," + PartyName + "," + Address + "," + EmailID + "," + MobileNo + "," + Vendor + "," + FinalStatus + "," + modType + "," + ErrorMsg);
                                    continue;
                                }
                                if (SheetType == Const.InvoiceProduct)
                                {
                                    string TaxGroup, Status;
                                    int Quantity;
                                    decimal Rate;
                                    decimal UnitPrice, NetSales, Total;
                                    decimal CGST = 0.00M, SGST = 0.00M, IGST = 0.00M;

                                    decimal shipping_UnitPrice = 0.00M, shipping_NetSales = 0.00M, shipping_Total;
                                    decimal shipping_CGST = 0.00M, shipping_SGST = 0.00M, shipping_IGST = 0.00M;

                                    ProductID = Converter.ToString(dr["ProductID"]);
                                    Quantity = Converter.ToInt(dr["Quantity"]);
                                    Rate = Converter.ToDecimal(dr["Rate"]);
                                    shipping_Total = Converter.ToDecimal(dr["Shipping & Handling"]);
                                    TaxGroup = Converter.ToString(dr["TaxGroup"]);
                                    Status = Converter.ToString(dr["Status"]);

                                    #region ==SGST, CGST & IGST Calculation==

                                    string TaxType = string.Empty;
                                    decimal SGST_Perc = 0.00M, CGST_Perc = 0.00M, IGST_Perc = 0.00M;
                                    string[] splitTax = null;
                                    if (!string.IsNullOrEmpty(TaxGroup))
                                    {
                                        splitTax = TaxGroup.Split('-');
                                        if (splitTax.Count() > 1)
                                        {
                                            TaxType = splitTax[0];
                                        }
                                        else
                                        {
                                            TaxType = TaxGroup;
                                        }
                                    }

                                    if (TaxType == "CSGST")
                                    {
                                        if (splitTax.Count() > 1)
                                        {
                                            CGST_Perc = Converter.ToDecimal(splitTax[1]);
                                        }
                                        if (CGST_Perc <= 0.00M)
                                        {
                                            CGST_Perc = FindGstRate(ProductID, "CGST");
                                        }

                                        if (splitTax.Count() > 2)
                                        {
                                            SGST_Perc = Converter.ToDecimal(splitTax[2]);
                                        }
                                        if (SGST_Perc <= 0.00M)
                                        {
                                            SGST_Perc = FindGstRate(ProductID, "SGST");
                                        }

                                        UnitPrice = Math.Round(((Rate / (CGST_Perc + 100)) * 100), 2);
                                        CGST = (Rate - UnitPrice) * Quantity;

                                        UnitPrice = Math.Round(((Rate / (SGST_Perc + 100)) * 100), 2);
                                        SGST = (Rate - UnitPrice) * Quantity;

                                        UnitPrice = (Rate * Quantity) - CGST - SGST;

                                        if (shipping_Total > 0.00M)
                                        {
                                            shipping_UnitPrice = Math.Round(((shipping_Total / (CGST_Perc + 100)) * 100), 2);
                                            shipping_CGST = shipping_Total - shipping_UnitPrice;

                                            shipping_UnitPrice = Math.Round(((shipping_Total / (SGST_Perc + 100)) * 100), 2);
                                            shipping_SGST = shipping_Total - shipping_UnitPrice;

                                            shipping_UnitPrice = (shipping_Total - shipping_CGST - shipping_SGST) / Quantity;
                                            shipping_NetSales = shipping_UnitPrice * Quantity;
                                        }
                                    }
                                    else
                                    {
                                        if (splitTax.Count() > 1)
                                        {
                                            IGST_Perc = Converter.ToDecimal(splitTax[1]);
                                        }
                                        if (IGST_Perc <= 0.00M)
                                        {
                                            IGST_Perc = FindGstRate(ProductID, "IGST");
                                        }

                                        UnitPrice = Math.Round(((Rate / (IGST_Perc + 100)) * 100), 2);
                                        IGST = (Rate - UnitPrice) * Quantity;

                                        if (shipping_Total > 0.00M)
                                        {
                                            shipping_UnitPrice = Math.Round(((shipping_Total / (IGST_Perc + 100)) * 100), 2);
                                            shipping_IGST = shipping_Total - shipping_UnitPrice;

                                            shipping_UnitPrice = (shipping_Total - shipping_IGST) / Quantity;
                                            shipping_NetSales = shipping_UnitPrice * Quantity;
                                        }
                                    }

                                    #endregion ==SGST, CGST & IGST Calculation==

                                    NetSales = UnitPrice * Quantity;
                                    Total = (Rate * Quantity) + Math.Round(shipping_Total, 2);

                                    if (modType == "UPDATE")
                                    {
                                        if (!string.IsNullOrEmpty(Status))
                                        {
                                            Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                                            {
                                                [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate),
                                                [1] = new SqlParameter("@intInvoiceNo", invoiceNo),
                                                [2] = new SqlParameter("@nvarcharOrderID", orderID),
                                                [3] = new SqlParameter("@datetimeOrderDate", orderDate),
                                                [4] = new SqlParameter("@nvarcharProductID", ProductID),

                                                [5] = new SqlParameter("@decimalRate", Rate),
                                                [6] = new SqlParameter("@intQuantity", Quantity),
                                                [7] = new SqlParameter("@decimalTotal", Total),
                                                [8] = new SqlParameter("@decimalNetSale", NetSales),
                                                [9] = new SqlParameter("@decimalCGST", CGST),
                                                [10] = new SqlParameter("@decimalSGST", SGST),
                                                [11] = new SqlParameter("@decimalIGST", IGST),
                                                [12] = new SqlParameter("@decimalUnitPrice", UnitPrice),

                                                [13] = new SqlParameter("@decimalCGSTPerc", CGST_Perc),
                                                [14] = new SqlParameter("@decimalSGSTPerc", SGST_Perc),
                                                [15] = new SqlParameter("@decimalIGSTPerc", IGST_Perc),

                                                [16] = new SqlParameter("@decimalShipping_Total", shipping_Total),
                                                [17] = new SqlParameter("@decimalShipping_CGST", shipping_CGST),
                                                [18] = new SqlParameter("@decimalShipping_SGST", shipping_SGST),
                                                [19] = new SqlParameter("@decimalShipping_IGST", shipping_IGST),
                                                [20] = new SqlParameter("@decimalShipping_UnitPrice", shipping_UnitPrice),
                                                [21] = new SqlParameter("@decimalShipping_NetSale", shipping_NetSales),

                                                [22] = new SqlParameter("@nvarcharStatus", Status),
                                                [23] = new SqlParameter("@intCreatedModifiedBy", UserID)
                                            };
                                            Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceProduct", sqlParam);
                                        }
                                        else
                                        {
                                            string strQuery = "Update InvoiceProductMaster Set Rate =" + Rate + ", UnitPrice =" + UnitPrice + ", Quantity =" + Quantity + ", Total =" + Total + ", NetSale=" + NetSales + ",CGST=" + CGST + ",SGST=" + SGST + ",IGST=" + IGST + ", Shipping_Total=" + shipping_Total + ", Shipping_UnitPrice=" + shipping_UnitPrice + ", Shipping_NetSale=" + shipping_NetSales + ", Shipping_CGST=" + shipping_CGST + ", Shipping_SGST=" + shipping_SGST + ", Shipping_IGST=" + shipping_IGST + ", CGSTPerc=" + CGST_Perc + ", SGSTPerc=" + SGST_Perc + ", IGSTPerc=" + IGST_Perc + ", ";
                                            strQuery += "ModifiedBy = " + UserID + ", ModifiedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
                                            strQuery += "Where InvoiceDate = '" + invoiceDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' And OrderDate = '" + orderDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'  And ";
                                            strQuery += "InvoiceNo = " + invoiceNo + " And OrderID = '" + orderID + "' And ProductID = '" + ProductID + "'";

                                            Result = ExecuteNonQuery(strQuery);
                                        }
                                    }
                                    else if (modType == "DELETE")
                                    {
                                        Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                                        {
                                            [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate),
                                            [1] = new SqlParameter("@intInvoiceNo", invoiceNo),
                                            [2] = new SqlParameter("@nvarcharOrderID", orderID),
                                            [3] = new SqlParameter("@datetimeOrderDate", orderDate),
                                            [4] = new SqlParameter("@nvarcharProductID", ProductID)
                                        };

                                        Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceProduct", sqlParam);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    if (Result >= Const.Success) ErrorMsg = string.Empty;
                                    if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice Details Not Found";
                                    if (Result == Const.Error) ErrorMsg = "Error";
                                    if (Result == Const.VendorIDNotFound) ErrorMsg = "Vendor ID Not Found";
                                    if (Result == Const.InvoiceStatusNotFound) ErrorMsg = "Invoice Status Not Found";
                                    if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                    sw.WriteLine(invoiceDate.ToString("dd-MM-yyyy") + "," + invoiceNo + "," + orderID + "," + orderDate.ToString("dd-MM-yyyy") + "," + ProductID + "," + Quantity + "," + Rate + "," + shipping_Total + "," + TaxGroup + "," + Status + "," + modType + "," + ErrorMsg);
                                    continue;
                                }
                                if (SheetType == Const.InvoiceSettlement)
                                {
                                    string Status;
                                    DateTime SettlementDate;
                                    decimal SettlementAmount;

                                    ProductID = Converter.ToString(dr["ProductID"]);
                                    SettlementDate = Converter.ToDateTime(dr["SettlementDate"]);
                                    SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]);
                                    Status = Converter.ToString(dr["Status"]);

                                    if (modType == "UPDATE")
                                    {
                                        if (!string.IsNullOrEmpty(Status))
                                        {
                                            Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                                            {
                                                [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate),
                                                [1] = new SqlParameter("@intInvoiceNo", invoiceNo),
                                                [2] = new SqlParameter("@nvarcharOrderID", orderID),
                                                [3] = new SqlParameter("@datetimeOrderDate", orderDate),
                                                [4] = new SqlParameter("@nvarcharProductID", ProductID),
                                                [5] = new SqlParameter("@datetimeSettlementDate", SettlementDate),

                                                [6] = new SqlParameter("@decimalSettlementAmount", SettlementAmount),
                                                [7] = new SqlParameter("@nvarcharStatus", Status),
                                                [8] = new SqlParameter("@intCreatedModifiedBy", UserID)
                                            };
                                            Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceSettlement", sqlParam);
                                        }
                                        else
                                        {
                                            string strQuery = "Update InvoiceSettlementMaster Set SettlementAmount =" + SettlementAmount + ",";
                                            strQuery += "ModifiedBy = " + UserID + ", ModifiedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
                                            strQuery += "Where InvoiceDate = '" + invoiceDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' And OrderDate = '" + orderDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'  And ";
                                            strQuery += "InvoiceNo = " + invoiceNo + " And OrderID = '" + orderID + "' And ProductID = '" + ProductID + "' And SettlementDate = '" + SettlementDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

                                            Result = ExecuteNonQuery(strQuery);
                                        }
                                    }
                                    else if (modType == "DELETE")
                                    {
                                        Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                                        {
                                            [0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate),
                                            [1] = new SqlParameter("@intInvoiceNo", invoiceNo),
                                            [2] = new SqlParameter("@nvarcharOrderID", orderID),
                                            [3] = new SqlParameter("@datetimeOrderDate", orderDate),
                                            [4] = new SqlParameter("@nvarcharProductID", ProductID),
                                            [5] = new SqlParameter("@datetimeSettlementDate", SettlementDate)
                                        };

                                        Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoiceSettlement", sqlParam);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    if (Result >= Const.Success) ErrorMsg = string.Empty;
                                    if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice Details Not Found";
                                    if (Result == Const.Error) ErrorMsg = "Error";
                                    if (Result == Const.VendorIDNotFound) ErrorMsg = "Vendor ID Not Found";
                                    if (Result == Const.InvoiceStatusNotFound) ErrorMsg = "Invoice Status Not Found";
                                    if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                    sw.WriteLine(invoiceDate.ToString("dd-MM-yyyy") + "," + invoiceNo + "," + orderID + "," + orderDate.ToString("dd-MM-yyyy") + "," + ProductID + "," + SettlementDate.ToString("dd-MM-yyyy") + "," + SettlementAmount + "," + Status + "," + modType + "," + ErrorMsg);
                                    continue;
                                }

                                dr.Close();
                                oleDbCommand.Dispose();
                                OleDbCon.Close();
                                if (sw != null)
                                {
                                    sw.Flush();
                                    sw.Close();
                                }
                                return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, false);
                            }
                            catch (Exception ex)
                            {
                                sw.WriteLine("DO NOT UPLOAD THIS DOCUMENT");
                                sw.WriteLine(invoiceNo + "," + orderID + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + ProductID + "," + ex.Message);
                            }
                        }
                        if (sw != null)
                        {
                            sw.Flush();
                            sw.Close();
                        }
                    }

                    return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, true);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        private int ExecuteNonQuery(string query)
        {
            using (SqlConnection sqlCon = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                {
                    sqlCon.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetVendorID(string Vendor)
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand("Select VendorID From VendorMaster Where VendorName = '" + Vendor + "'", sqlCon);
            sqlCon.Open();
            int VendorID = Converter.ToInt(cmd.ExecuteScalar());
            sqlCon.Close();
            cmd.Dispose();
            if (VendorID == 0)
            {
                VendorID = Const.VendorIDNotFound;
            }
            return VendorID;
        }

        private int GetStatusID(string Status)
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand("Select SubLookupID From SubLookupMaster Where LookupID = 14 And SubLookupName = '" + Status + "'", sqlCon);
            sqlCon.Open();
            int StatusID = Converter.ToInt(cmd.ExecuteScalar());
            sqlCon.Close();
            cmd.Dispose();
            if (StatusID == 0)
            {
                StatusID = Const.InvoiceStatusNotFound;
            }
            return StatusID;
        }

        public Tuple<string, string, bool> UploadSheetUpdateDelete(byte[] sheetData, string FileName, int UserID, int SheetType)
        {
            try
            {
                string ErrorCsvFileName = string.Empty, ErrorFolderPath = string.Empty, MainFolderPath = string.Empty, MainFileName = string.Empty;
                string dt = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                if (SheetType == Const.Invoicing) { ErrorFolderPath = "Patches\\Sheets\\Invoicing\\" + dt + "\\" + UserID; }
                if (SheetType == Const.Settlement) { ErrorFolderPath = "Patches\\Sheets\\Settlement\\" + dt + "\\" + UserID; }

                string destFilePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + ErrorFolderPath;
                MainFolderPath = destFilePath;
                if (!Directory.Exists(MainFolderPath))
                {
                    Directory.CreateDirectory(MainFolderPath);
                }
                MainFileName = ErrorCsvFileName = Guid.NewGuid().ToString() + FileName;
                string FilePath = destFilePath + "\\" + MainFileName;
                System.IO.File.WriteAllBytes(FilePath, sheetData);

                string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
                using (OleDbConnection OleDbCon = new OleDbConnection(con))
                {
                    OleDbCon.Open();
                    OleDbCommand oleDbCommand = new OleDbCommand();
                    if (SheetType == Const.Invoicing)
                    {
                        oleDbCommand = new OleDbCommand("Select * From [Invoicing$] Where ModeType <> ''", OleDbCon);
                    }
                    else if (SheetType == Const.Settlement)
                    {
                        oleDbCommand = new OleDbCommand("Select * From [Settlement$] Where ModeType <> ''", OleDbCon);
                    }
                    OleDbDataReader dr = oleDbCommand.ExecuteReader();
                    ErrorCsvFileName = ErrorCsvFileName.Replace(".xlsx", ".csv");
                    ErrorFolderPath += "\\Error\\";
                    if (!Directory.Exists(MainFolderPath + "\\Error\\"))
                    {
                        Directory.CreateDirectory(MainFolderPath + "\\Error\\");
                    }
                    StreamWriter sw = new StreamWriter(MainFolderPath + "\\Error\\" + ErrorCsvFileName);
                    if (SheetType == Const.Invoicing) sw.WriteLine("InvoiceNo,Invoice Date,Party's Name, Order Date,Order ID,SKU,Rate,Quantity,Shipping & Handling,Sales Group,Tax Group,Address,Email ID,Mobile No,ModeType,ErrorMessage");
                    if (SheetType == Const.Settlement) sw.WriteLine("Invoice No,Order ID,Invoice Date,SKU,Settlement Date,Settlement Amount,Status,New Status,ModeType,ErrorMessage");

                    while (dr.Read())
                    {
                        int invoiceNo = 0;
                        string orderID = string.Empty;
                        DateTime invoiceDate = new DateTime();
                        string productID = "";
                        string ErrorMsg = string.Empty;
                        int Result = 0;
                        try
                        {
                            string ModeType = Converter.ToString(dr["ModeType"]).ToUpper();
                            if (SheetType == Const.Invoicing)
                            {
                                int quantity = 0;
                                decimal rate;
                                decimal ShippingHandling;
                                DateTime orderDate;
                                string TaxGroup, PartyName, Address, EmailID, MobNo, Vendor;

                                orderID = Converter.ToString(dr["Order ID"]);
                                if (string.IsNullOrEmpty(orderID)) continue;
                                orderDate = Converter.ToDateTime(dr["Order Date"]);
                                if (orderDate <= new DateTime(1900, 01, 01)) orderDate = DateTime.Now.Date;
                                invoiceNo = 0;
                                invoiceDate = Converter.ToDateTime(dr["Invoice Date"]);
                                if (invoiceDate <= new DateTime(1900, 01, 01)) invoiceDate = DateTime.Now.Date;
                                productID = Converter.ToString(dr["SKU"]).ToUpper();

                                quantity = Converter.ToInt(dr["Quantity"]);
                                rate = Converter.ToDecimal(dr["Rate"]);
                                TaxGroup = Converter.ToString(dr["Tax Group"]);
                                PartyName = Converter.ToString(dr["Party's Name"]);
                                Address = Converter.ToString(dr["Address"]);
                                EmailID = Converter.ToString(dr["Email ID"]);
                                MobNo = Converter.ToString(dr["Mobile No"]);
                                ShippingHandling = Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);
                                Vendor = Converter.ToString(dr["Sales Group"]);

                                if (ModeType == "UPDATE")
                                {
                                    Result = UpdateInvoicing(orderID, productID, PartyName, Address, EmailID, MobNo, Vendor, TaxGroup, rate, quantity, ShippingHandling, UserID);
                                }
                                else if (ModeType == "DELETE")
                                {
                                    Result = DeleteInvoicing(orderID);
                                }
                                else
                                {
                                    continue;
                                }

                                if (Result >= Const.Success) ErrorMsg = string.Empty;
                                if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice details not found";
                                if (Result == Const.InvoiceProductNotFound) ErrorMsg = "Invoice product not found";
                                if (Result == Const.ProductNotExists) ErrorMsg = "Product not found";
                                if (Result == Const.Duplicate) ErrorMsg = "Duplicate record";
                                if (Result == Const.Error) ErrorMsg = "Error";
                                if (Result == Const.VendorIDNotFound) ErrorMsg = "Vendor ID not found";
                                if (Result == Const.NetworkError) ErrorMsg = "Network error";
                                Address = Address.Replace(',', ' ');
                                if (invoiceDate.Date == DateTime.Now.Date && orderDate.Date == DateTime.Now.Date) sw.WriteLine(Result + ",," + PartyName + ",," + orderID + "," + productID + "," + rate + "," + quantity + "," + ShippingHandling + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ModeType + "," + ErrorMsg);
                                if (invoiceDate.Date == DateTime.Now.Date && orderDate.Date != DateTime.Now.Date) sw.WriteLine(Result + ",," + PartyName + "," + orderDate.ToString("dd-MM-yyyy") + "," + orderID + "," + productID + "," + rate + "," + quantity + "," + ShippingHandling + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ModeType + "," + ErrorMsg);
                                if (invoiceDate.Date != DateTime.Now.Date && orderDate.Date == DateTime.Now.Date) sw.WriteLine(Result + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + PartyName + ",," + orderID + "," + productID + "," + rate + "," + quantity + "," + ShippingHandling + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ModeType + "," + ErrorMsg);
                                if (invoiceDate.Date != DateTime.Now.Date && orderDate.Date != DateTime.Now.Date) sw.WriteLine(Result + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + PartyName + "," + orderDate.ToString("dd-MM-yyyy") + "," + orderID + "," + productID + "," + rate + "," + quantity + "," + ShippingHandling + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ModeType + "," + ErrorMsg);
                                continue;
                            }
                            if (SheetType == Const.Settlement)
                            {
                                DateTime SettlementDate;
                                decimal SettlementAmt;
                                string PrevStatus = string.Empty, NewStatus = string.Empty;

                                invoiceNo = Converter.ToInt(dr["Invoice No"]);
                                orderID = Converter.ToString(dr["Order ID"]);
                                invoiceDate = Converter.ToDateTime(dr["Invoice Date"]);
                                productID = Converter.ToString(dr["SKU"]).ToUpper();

                                SettlementDate = Converter.ToDateTime(dr["Settlement Date"]);
                                SettlementAmt = Converter.ToDecimal(dr["Settlement Amount"]);
                                PrevStatus = Converter.ToStatusString(dr["Status"]);
                                NewStatus = Converter.ToStatusString(dr["New Status"]);

                                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                                {
                                    [0] = new SqlParameter("@nvarcharOrderID", orderID),
                                    [1] = new SqlParameter("@nvarcharProductID", productID),
                                    [2] = new SqlParameter("@datetimeSettlementDate", SettlementDate),
                                    [3] = new SqlParameter("@decimalSettlementAmount", SettlementAmt),
                                    [4] = new SqlParameter("@nvarcharPrevStatus", PrevStatus),
                                    [5] = new SqlParameter("@nvarcharNewStatus", NewStatus),
                                    [6] = new SqlParameter("@intCreatedModifiedBy", UserID)
                                };

                                if (ModeType == "UPDATE")
                                {
                                    Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateInvoiceSettlementStatus", sqlParam);
                                }
                                else
                                {
                                    Result = Const.Error;
                                }

                                if (Result == Const.Success) continue;
                                if (Result == Const.InvoiceReceived) ErrorMsg = "Already Received";
                                if (Result == Const.InvoiceReturned) ErrorMsg = "Already Returned";
                                if (Result == Const.InvoiceCancelled) ErrorMsg = "Already Cancelled";
                                if (Result == Const.InvoiceStatusNotFound) ErrorMsg = "Invoice Status Not Found";
                                if (Result == Const.InvalidPendingInvoiceStatus) ErrorMsg = "Invalid Status Pending";
                                if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice Details Not Found";
                                if (Result == Const.InvoiceSettlementNotFound) ErrorMsg = "Invoice Settlement Not Found";
                                if (Result == Const.CreditNoteDetailsNotFound) ErrorMsg = "Credit Note Details Not Found";
                                if (Result == Const.SalesDetailsNotFound) ErrorMsg = "Sales Details Not Found";
                                if (Result == Const.Error) ErrorMsg = "Error";
                                if (Result == Const.NetworkError) ErrorMsg = "Network Error";

                                sw.WriteLine(invoiceNo + "," + orderID + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + productID + "," + SettlementDate.ToString("dd-MM-yyyy") + "," + SettlementAmt + "," + PrevStatus + "," + NewStatus + "," + ModeType + "," + ErrorMsg);
                                continue;
                            }
                            dr.Close();
                            oleDbCommand.Dispose();
                            OleDbCon.Close();
                            if (sw != null)
                            {
                                sw.Flush();
                                sw.Close();
                            }
                            return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, false);
                        }
                        catch (Exception ex)
                        {
                            sw.WriteLine("DO NOT UPLOAD THIS DOCUMENT");
                            sw.WriteLine(invoiceNo + "," + orderID + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + productID + "," + ex.Message);
                        }
                    }
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
                return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, true);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1854:Dead stores should be removed", Justification = "<Pending>")]
        private int UpdateInvoicing(string orderID, string productID, string PartyName, string Address, string EmailID, string MobNo, string Vendor, string TaxGroup, decimal rate, int quantity, decimal shipping_Total, int UserID)
        {
            int Result;
            string Query;

            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction;
            sqlCon.Open();
            transaction = sqlCon.BeginTransaction();

            try
            {
                #region ==InvoiceDetailsMaster==

                Query = "Update InvoiceDetailsMaster Set ";
                if (!string.IsNullOrEmpty(PartyName)) Query += "PartysName='" + PartyName + "',";
                if (!string.IsNullOrEmpty(Address)) Query += "Address='" + Address + "',";
                if (!string.IsNullOrEmpty(EmailID)) Query += "EmailID='" + EmailID + "',";
                if (!string.IsNullOrEmpty(MobNo)) Query += "MobileNo='" + MobNo + "',";
                if (!string.IsNullOrEmpty(Vendor))
                {
                    int VendorID = GetVendorID(Vendor);
                    if (VendorID != 0)
                    {
                        Query += "VendorID=" + VendorID + ",";
                    }
                }

                Query += "ModifiedBy = " + UserID + ", ModifiedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
                Query += "Where OrderID = '" + orderID + "' And FinalStatus!=9";
                cmd = new SqlCommand(Query, sqlCon, transaction);
                Result = cmd.ExecuteNonQuery();

                #endregion ==InvoiceDetailsMaster==

                #region ==InvoiceProductMaster==

                if (Result > 0)
                {
                    decimal PreviousProductAmount;

                    cmd = new SqlCommand("Select Total From  InvoiceProductMaster Where OrderID = '" + orderID + "' And ProductID='" + productID + "' And Status != 9", sqlCon, transaction);
                    PreviousProductAmount = Converter.ToDecimal(cmd.ExecuteScalar());
                    if (PreviousProductAmount <= 0.00M)
                    {
                        transaction.Rollback();
                        Result = Const.Error;
                    }
                    else
                    {
                        decimal unitPrice, netSales, total;
                        decimal SGST_Perc = 0.00M, CGST_Perc = 0.00M, IGST_Perc = 0.00M;
                        decimal CGST = 0.00M, SGST = 0.00M, IGST = 0.00M;

                        decimal shipping_UnitPrice = 0.00M, shipping_NetSales = 0.00M;
                        decimal shipping_CGST = 0.00M, shipping_SGST = 0.00M, shipping_IGST = 0.00M;

                        Query = string.Empty;

                        if (TaxGroup == "CSGST")
                        {
                            CGST_Perc = FindGstRate(productID, "CGST");
                            unitPrice = Math.Round(((rate / (CGST_Perc + 100)) * 100), 2);
                            CGST = (rate - unitPrice) * quantity;

                            SGST_Perc = FindGstRate(productID, "SGST");
                            unitPrice = Math.Round(((rate / (SGST_Perc + 100)) * 100), 2);
                            SGST = (rate - unitPrice) * quantity;

                            unitPrice = (rate * quantity) - CGST - SGST;

                            if (shipping_Total > 0.00M)
                            {
                                shipping_UnitPrice = Math.Round(((shipping_Total / (CGST_Perc + 100)) * 100), 2);
                                shipping_CGST = shipping_Total - shipping_UnitPrice;

                                shipping_UnitPrice = Math.Round(((shipping_Total / (SGST_Perc + 100)) * 100), 2);
                                shipping_SGST = shipping_Total - shipping_UnitPrice;

                                shipping_UnitPrice = (shipping_Total - shipping_CGST - shipping_SGST) / quantity;
                                shipping_NetSales = shipping_UnitPrice * quantity;
                            }
                        }
                        else
                        {
                            IGST_Perc = FindGstRate(productID, "IGST");
                            unitPrice = Math.Round(((rate / (IGST_Perc + 100)) * 100), 2);
                            IGST = (rate - unitPrice) * quantity;

                            if (shipping_Total > 0.00M)
                            {
                                shipping_UnitPrice = Math.Round(((shipping_Total / (IGST_Perc + 100)) * 100), 2);
                                shipping_IGST = shipping_Total - shipping_UnitPrice;

                                shipping_UnitPrice = (shipping_Total - shipping_IGST) / quantity;
                                shipping_NetSales = shipping_UnitPrice * quantity;
                            }
                        }

                        netSales = unitPrice * quantity;
                        total = (rate * quantity) + shipping_Total;

                        Query = "Update InvoiceProductMaster Set Rate =" + rate + ", UnitPrice =" + unitPrice + ", Quantity =" + quantity + ", Total =" + total + ", NetSale=" + netSales + ",";
                        Query += "CGST=" + CGST + ", SGST=" + SGST + ", IGST=" + IGST + ",";
                        Query += "CGSTPerc=" + CGST_Perc + ", SGSTPerc=" + SGST_Perc + ", IGSTPerc=" + IGST_Perc + ",";
                        Query += "Shipping_UnitPrice=" + shipping_UnitPrice + ", Shipping_NetSale=" + shipping_NetSales + ", Shipping_Total=" + shipping_Total + ",";
                        Query += "Shipping_CGST=" + shipping_CGST + ", Shipping_SGST=" + shipping_SGST + ", Shipping_IGST=" + shipping_IGST + ",";
                        Query += "ModifiedBy = " + UserID + ", ModifiedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
                        Query += "Where OrderID = '" + orderID + "' And ProductID = '" + productID + "' And Status != 9";
                        cmd = new SqlCommand(Query, sqlCon, transaction);
                        Result = cmd.ExecuteNonQuery();
                        if (Result <= 0)
                        {
                            transaction.Rollback();
                            Result = Const.InvoiceProductNotFound;
                        }
                        else
                        {
                            Query = string.Empty;
                            Query = "Update InvoiceDetailsMaster Set InvoiceAmount+=" + (total - PreviousProductAmount) + " ";
                            Query += "Where OrderID = '" + orderID + "' And FinalStatus != 9";
                            cmd = new SqlCommand(Query, sqlCon, transaction);
                            Result = cmd.ExecuteNonQuery();
                            if (Result <= 0)
                            {
                                transaction.Rollback();
                                Result = Const.InvoiceDetailsNotFound;
                            }
                            else
                            {
                                transaction.Commit();
                            }
                        }
                    }
                }
                else
                {
                    transaction.Rollback();
                    Result = Const.InvoiceDetailsNotFound;
                }

                #endregion ==InvoiceProductMaster==
            }
            catch
            {
                transaction.Rollback();
                Result = Const.Error;
            }
            sqlCon.Close();
            return Result;
        }

        private int DeleteInvoicing(string orderID)
        {
            int Result = 0;
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlTransaction transaction;
            sqlCon.Open();
            transaction = sqlCon.BeginTransaction();

            try
            {
                using (SqlCommand cmd = new SqlCommand("Delete From InvoiceDetailsMaster Where OrderID = '" + orderID + "' And FinalStatus != 9", sqlCon, transaction))
                {
                    Result = cmd.ExecuteNonQuery();
                    if (Result > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        Result = Const.InvoiceDetailsNotFound;
                    }
                }
            }
            catch
            {
                transaction.Rollback();
                Result = Const.Error;
            }
            sqlCon.Close();
            return Result;
        }
    }
}