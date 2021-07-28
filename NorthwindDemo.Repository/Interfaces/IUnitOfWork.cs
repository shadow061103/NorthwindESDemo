using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDemo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 取得泛型 Repository
        /// </summary>
        /// <typeparam name="TEntity">實體</typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;
    }
}
