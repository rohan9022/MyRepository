using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ICreditNoteRepository
    {
        void Add(CreditNote creditNote);

        void Update(CreditNote creditNote);

        void Delete(CreditNote creditNote);

        CreditNote Get(CreditNote creditNote);

        IList<CreditNote> GetAll();
    }
}