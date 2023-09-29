using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterCloneBackend.DDD;

namespace TwitterCloneBackend.Tests
{
    public static class DbSetExtensions
    {
        public static DbSet<TEntity> ReturnsDbSet<TEntity>(this Mock<DataContext> mockContext, List<TEntity> data) where TEntity : class
        {
            var queryable = data.AsQueryable();

            var dbSet = new Mock<DbSet<TEntity>>();
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mockContext.Setup(c => c.Set<TEntity>()).Returns(dbSet.Object);
            return dbSet.Object;
        }
    }
}
