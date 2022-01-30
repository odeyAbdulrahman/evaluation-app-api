using Microsoft.EntityFrameworkCore.Storage;
using OA.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA.Service.Interfaces
{
    public interface ITransaction
    {
        IDbContextTransaction BeginTransaction();
    }
}
