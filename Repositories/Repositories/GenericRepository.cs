using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{

    public interface IGenericRepository<T> where T : class
    {
        public List<T> GetAll(Expression<Func<T, bool>> filter = null,
                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                        string includeProperties = "");

        public void Add(T item);

        public T GetById(string id);

        public void Update(T item);

        public void Delete(T item);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        ViroCureFal2024dbContext context;
        DbSet<T> dbSet;
        public GenericRepository()
        {
            context = new ViroCureFal2024dbContext();
            dbSet = context.Set<T>();
        }
        public List<T> GetAll(Expression<Func<T, bool>> filter = null,
                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public void Add(T item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            dbSet.Update(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public T GetById(string id)
        {
            return dbSet.Find(id);
        }
    }
}
