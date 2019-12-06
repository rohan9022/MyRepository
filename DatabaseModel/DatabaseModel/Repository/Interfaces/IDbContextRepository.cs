using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IDbContextRepository
    {
        TestContext TestContext { get; }
    }
}