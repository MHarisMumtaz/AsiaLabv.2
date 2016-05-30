using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Repositories
{
    public class Repository<T> where T : class
    {

        protected static AsiaLabDbEntities Context;
        public DbSet<T> Table { get; set; }

        public Repository()
        {
            if (Context == null)
            {
                Context = StaticDbContext.context;
            }
            Table = Context.Set<T>();
        }

        public List<T> GetAll()
        {
            return Table.ToList();
        }
        public T GetById(int Id)
        {
            return Table.Find(Id);
        }

        public T Insert(T entity)
        {
            try
            {
                Table.Add(entity);
                Context.SaveChanges();
                return entity;

            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void Update(T entity,int id)
        {
            var ent = GetById(id);
            Table.Remove(ent);
            Context.SaveChanges();
            Table.Add(entity);
            Context.SaveChanges();
        }
        public void UpdateGeneric(T entity)
        {
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
            Context.SaveChanges();
        }
        public void DeleteById(int Id)
        {
            var entity = GetById(Id);
            Table.Remove(entity);
            Context.SaveChanges();
        }

        public void DeleteAll()
        {
            var list = GetAll();

            foreach (var item in list)
            {
                Delete(item);
                Context.SaveChanges();
            }

        }
    }
}