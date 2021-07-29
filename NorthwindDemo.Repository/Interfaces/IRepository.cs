using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NorthwindDemo.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 加入實體
        /// </summary>
        /// <param name="entity">實體</param>
        void Add(TEntity entity);

        /// <summary>
        /// 新增多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 取得多筆資料
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="predicate">查詢條件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="include">包含關聯資料</param>
        /// <param name="disableTracking">是否關閉 tracking</param>
        /// <returns></returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        /// 刪除實體
        /// </summary>
        /// <param name="entity">實體</param>
        void Remove(TEntity entity);

        /// <summary>
        /// 刪除多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新實體
        /// </summary>
        /// <param name="entity">實體</param>
        void Update(TEntity entity);

        /// <summary>
        /// 更新多筆實體
        /// </summary>
        /// <param name="entities">實體集合</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 儲存異動
        /// </summary>
        void SaveChanges();
    }
}