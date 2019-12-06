using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class CreditNoteRepository : ICreditNoteRepository
    {
        //[sp_Cr_CreditNoteReport]

        private readonly TestContext testContext;

        public CreditNoteRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(CreditNote creditNote)
        {
            creditNote.CrNoteNo = GenearteId();
            testContext.Add(creditNote);
            testContext.SaveChanges();
        }

        public void Delete(CreditNote creditNote)
        {
            var creditNoteItem = testContext.CreditNote.Find(creditNote.CrNoteNo, creditNote.CrNoteDate, creditNote.ProductId, creditNote.OrderId, creditNote.OrderDate);
            testContext.CreditNote.Remove(creditNoteItem);
            testContext.SaveChanges();
        }

        public CreditNote Get(CreditNote creditNote)
        {
            return testContext.CreditNote.Find(creditNote.CrNoteNo, creditNote.CrNoteDate, creditNote.ProductId, creditNote.OrderId, creditNote.OrderDate);
        }

        public IList<CreditNote> GetAll()
        {
            return testContext.CreditNote.ToList();
        }

        public void Update(CreditNote creditNote)
        {
            var creditNoteItem = testContext.CreditNote.Find(creditNote.CrNoteNo, creditNote.CrNoteDate, creditNote.ProductId, creditNote.OrderId, creditNote.OrderDate);
            creditNoteItem.ModifiedBy = creditNote.ModifiedBy;
            creditNoteItem.IsReturned = creditNote.IsReturned;
            testContext.SaveChanges();
        }

        //[sp_Cr_InsertUpdateCreditNote]
        public void InsertUpdateCreditNote(CreditNote creditNote)
        {
            if (string.IsNullOrEmpty(creditNote.CrNoteNo))
            {
                creditNote.CrNoteNo = GenearteId();
                Add(creditNote);
            }
            else
            {
                Delete(creditNote);
            }
        }

        private string GenearteId()
        {
            return "SR-C-" + (System.Convert.ToInt32(testContext.CreditNote.Max(x => x.CrNoteNo.Substring(5, 10))) + 1).ToString().PadLeft(10, '0');
        }
    }
}