using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NorthwindDemo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        /// 取得資料
        /// </summary>
        /// <param name="predicate">查詢條件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="include">包含關聯資料</param>
        /// <param name="disableTracking">是否關閉 tracking</param>
        /// <returns></returns>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
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