using DatabaseModel.Models;
using DatabaseModel.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace DatabaseModel
{
    internal class Program
    {
        public class Credit
        {
            public string CrNoteNo { get; set; }
        }

        private static void Main(string[] args)
        {
            List<Credit> lst = new List<Credit>();
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000001" });
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000002" });
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000006" });
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000005" });
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000004" });
            lst.Add(new Credit { CrNoteNo = "SR-C-0000000003" });

            string strID = "SR-C-" + (System.Convert.ToInt32(lst.Max(x => x.CrNoteNo.Substring(5, 10))) + 1).ToString().PadLeft(10, '0');

            //IUnityContainer container = new UnityContainer();
            //RegisterComponents();
            //var lstCategory1 = container.Resolve<ICategoryRepository>().GetAll();

            using (var context = new TestContext())
            {
                var list = context.Set<CategoryMaster>().FromSql("sp_cm_GetCategoryList");

                //int nCatID = 1;
                //List<Product> lst = dataContext.Products
                //                   .FromSql("usp_GetProductsByCategory @p0", nCatID)
                //                   .ToList();

                //int nCatID = 1;
                //var catName = "Clothing";
                //lstProducts = dataContext.Products
                //             .FromSql("usp_GetProductsByCategoryIDAndName @p0, @p1",
                //                    parameters: new[] { nCatID.ToString(), catName })
                //              .ToList();

                //int nCatID = 1;
                //List<Product> lst = dataContext.Products
                //                   .FromSql("usp_GetProductsByCategory {0}", nCatID)
                //                   .ToList();

                //               Select IM.InvoiceNo As InvoiceNo, IM.PartysName As PartyName, IM.Address As Address,IM.InvoiceDate As InvoiceDate,IM.OrderID As OrderNo,IM.OrderDate As OrderDate,VM.GSTNo As GSTNo, VM.PanNo As PanNo, VM.CommissionTab As CommissionTab
                //   From InvoiceDetailsMaster IM Join VendorMaster VM On IM.VendorID = VM.VendorID

                //   Where IM.InvoiceNo = @intInvoiceNo And IM.FinalStatus != 9

                //   Select IM.InvoiceNo, PDM.ProductID + ' - ' + PDM.Title + ' ' + PDM.PosterDimensions + ' [' + SLM.SubLookupName + ']' As Description, IM.Quantity As Quantity,
                //IM.UnitPrice As UnitPrice, IM.NetSale As NetSale,
                //IM.CGST As CGST, IM.SGST AS SGST, IM.IGST As IGST,
                //IM.Shipping_UnitPrice As Shipping_UnitPrice, IM.Shipping_NetSale As Shipping_NetSale,
                //IM.Shipping_CGST As Shipping_CGST, IM.Shipping_SGST AS Shipping_SGST, IM.Shipping_IGST As Shipping_IGST, IM.Shipping_Total As Shipping,
                //IM.Rate As Rate, IM.SettlementAmount As SettlementAmount, (IM.Total - IM.SettlementAmount) As DifferenceAmount, IM.Status As Status,
                //IM.CGSTPerc As CGSTPerc, IM.SGSTPerc As SGSTPerc, IM.IGSTPerc As IGSTPerc
                //   From InvoiceProductMaster IM Join ProductDescriptionMaster PDM On IM.ProductID = PDM.ProductID

                //   Join SubLookupMaster SLM On IM.Status = SLM.SubLookupID

                //   Where IM.InvoiceNo = @intInvoiceNo And SLM.LookupID = 14 And IM.Status != 9

                // max query
                int id = context.CategoryMaster.Max(x => x.CategoryId) + 1;

                // find all
                var lstCategory = context.CategoryMaster.ToList();

                var item = context.CategoryMaster.Where(x => x.Name == "Posters");

                var pk = context.CategoryMaster.Find(1);
            }
        }

        private static void RegisterComponents()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDbContextRepository, DbContextRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<ICreditNoteRepository, CreditNoteRepository>();
            container.RegisterType<IDamagedGoodsRepository, DamagedGoodsRepository>();
            container.RegisterType<IDebitNoteRepository, DebitNoteRepository>();
            container.RegisterType<IInventoryItemRepository, InventoryItemRepository>();
            container.RegisterType<IInvoiceDetailsRepository, InvoiceDetailsRepository>();
            container.RegisterType<IInvoiceProductRepository, InvoiceProductRepository>();
            container.RegisterType<IInvoiceRepository, InvoiceRepository>();
            container.RegisterType<IInvoiceSettlementRepository, InvoiceSettlementRepository>();
            container.RegisterType<ILookupRepository, LookupRepository>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IProductDescriptionRepository, ProductDescriptionRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IPurchaseRepository, PurchaseRepository>();
            container.RegisterType<ISalesRepository, SalesRepository>();
            container.RegisterType<ISubCategoryRepository, SubCategoryRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IVendorRepository, VendorRepository>();

            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            container.Resolve<IDbContextRepository>();
            container.Resolve<ICategoryRepository>();
            container.Resolve<ICompanyRepository>();
            container.Resolve<ICreditNoteRepository>();
            container.Resolve<IDamagedGoodsRepository>();
            container.Resolve<IDebitNoteRepository>();
            container.Resolve<IInventoryItemRepository>();
            container.Resolve<IInvoiceDetailsRepository>();
            container.Resolve<IInvoiceProductRepository>();
            container.Resolve<IInvoiceRepository>();
            container.Resolve<IInvoiceSettlementRepository>();
            container.Resolve<ILookupRepository>();
            container.Resolve<IMenuRepository>();
            container.Resolve<IProductDescriptionRepository>();
            container.Resolve<IProductRepository>();
            container.Resolve<IPurchaseRepository>();
            container.Resolve<ISalesRepository>();
            container.Resolve<ISubCategoryRepository>();
            container.Resolve<IUserRepository>();
            container.Resolve<IVendorRepository>();
        }
    }
}