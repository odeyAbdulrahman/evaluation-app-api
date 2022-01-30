using Microsoft.EntityFrameworkCore.Storage;
using OA.Data;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA.Service.Services
{
    class Transaction : ITransaction
    {
        readonly AppDbContext Db;
        public Transaction(AppDbContext db)
        {
            Db = db;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Db.Database.BeginTransaction();
        }
    }
}
