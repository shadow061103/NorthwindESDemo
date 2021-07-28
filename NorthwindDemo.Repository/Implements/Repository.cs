using Microsoft.EntityFrameworkCore;
using NorthwindDemo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NorthwindDemo.Repository.Implements
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 加入實體
        /// </summary>
        /// <param name="entity">實體</param>
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// 新增多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        /// <summary>
        /// 取得多筆資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Get()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 刪除實體
        /// </summary>
        /// <param name="entity">實體</param>
        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// 刪除多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        /// <summary>
        /// 儲存異動
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// 更新實體
        /// </summary>
        /// <param name="entity">實體</param>
        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        /// <summary>
        /// 更新多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }
    }
}
