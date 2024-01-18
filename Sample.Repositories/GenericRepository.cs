using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Sample.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(long id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<bool> Detach(T entity);
        IQueryable<T> GetAll();
    }

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected System.Data.Entity.DbSet<T> DbSet { get; set; }
        protected Microsoft.EntityFrameworkCore.DbContext Context { get; set; }

        public GenericRepository(Microsoft.EntityFrameworkCore.DbContext context)
		{
            if (context == null)
                throw new ArgumentException("An instance of DbContext is " +
                                            "required to use this repository.", nameof(context));
            Context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual async Task<bool> Add(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State != Microsoft.EntityFrameworkCore.EntityState.Detached)
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Added;
            else
                DbSet.Add(entity);

            await Context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Update(T entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Detached)
                DbSet.Attach(entity);

            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await Context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State != Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }

            await Context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);

            if (entity != null)
                await Delete(entity);

            return true;
        }

        public virtual async Task<bool> Detach(T entity)
        {
            var entry = Context.Entry(entity);

            entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            await Context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T> GetById(long id)
        {
            return await DbSet.FindAsync(id);
        }
    }
}

