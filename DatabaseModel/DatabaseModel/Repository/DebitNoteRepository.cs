using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class DebitNoteRepository : IDebitNoteRepository
    {
        private readonly TestContext testContext;

        public DebitNoteRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(DebitNote debitNote)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DebitNote Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<DebitNote> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(DebitNote debitNote)
        {
            throw new NotImplementedException();
        }
    }
}