using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Pis.Projekt.Framework.Repositories
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Checks, if given entity exists in database context.
        /// ref: https://stackoverflow.com/a/12573886/592212
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static bool Exists<TContext, TEntity>(this TContext context, TEntity entity)
            where TContext : DbContext
            where TEntity : class
        {
            return context.Set<TEntity>().Local.Any(e => e == entity);
        }
    }
}