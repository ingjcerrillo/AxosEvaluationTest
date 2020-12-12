using com.ToysAndGames.Server.WebAPI.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.ToysAndGames.Server.WebAPI.DAL.Repositories.Generic
{
    //Generic unit of work implementation.

    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Commit();
    }

    public class GenericUnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }

        public GenericUnitOfWork(ApiContext apiContext)
        {
            Context = apiContext;
        }

        public void Commit()
        {
            Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
