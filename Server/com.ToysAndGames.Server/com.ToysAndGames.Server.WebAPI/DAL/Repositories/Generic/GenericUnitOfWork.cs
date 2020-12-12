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

            //A bit tricky here, as this is not a migration we have to force the context to seed the data.
            if(Context.Database.IsInMemory())
                Context.Database.EnsureCreated();
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
