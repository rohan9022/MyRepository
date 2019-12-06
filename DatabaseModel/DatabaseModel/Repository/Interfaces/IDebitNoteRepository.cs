using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IDebitNoteRepository
    {
        void Add(DebitNote debitNote);

        void Update(DebitNote debitNote);

        void Delete(int id);

        DebitNote Get(int id);

        IList<DebitNote> GetAll();
    }
}