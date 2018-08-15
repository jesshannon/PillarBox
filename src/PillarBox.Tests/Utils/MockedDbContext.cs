using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PillarBox.Tests.Utils
{
    public class MockedDbContext<T> : Mock<T> where T : DbContext
    {
        public MockedDbContext()
        {
            Type contextType = typeof(T);

            // find all DbSet properties
            var dbSetProperties = contextType.GetProperties().Where(prop =>
                (prop.PropertyType.IsGenericType)
                && (prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                );
            
            // Create a mock table for each property
            foreach (var prop in dbSetProperties)
            {
                // create mock table as List<T>
                var entityType = prop.PropertyType.GetGenericArguments()[0];
                Type listType = typeof(List<>).MakeGenericType(entityType);
                var mockedTableList = Activator.CreateInstance(listType);

                // get setup method
                string methodname = "MockDbSet";
                var method = this.GetType().GetMethod(methodname).MakeGenericMethod(entityType);

                // input param for setup method
                var parameter = Expression.Parameter(contextType);

                // setup named property, eg. Context.Entities
                var body = Expression.PropertyOrField(parameter, prop.Name);

                var dbSetType = typeof(DbSet<>).MakeGenericType(entityType);
                var funcType = typeof(Func<,>).MakeGenericType(typeof(T), dbSetType);
                var createLambda = typeof(Expression).GetMethods()
                    .Where(m=>m.Name == "Lambda" && m.IsGenericMethod && m.GetParameters().Count()==2)
                    .First().MakeGenericMethod(funcType);
                var propertyExpression = createLambda.Invoke(null, new object[] { body, new ParameterExpression[] { parameter } });


                var iSetupMethod = GetType().GetMethods()
                    .Where(m => m.Name == "Setup" && m.IsGenericMethod)
                    .First().MakeGenericMethod(dbSetType);
                var returnMethod = typeof(IReturns<,>).MakeGenericType(typeof(T), dbSetType).GetMethods()
                    .Where(m => m.Name == "Returns" && !m.IsGenericMethod).First();

                var iSetup = iSetupMethod.Invoke(this, new object[] { propertyExpression });
                returnMethod.Invoke(iSetup, new object[] { method.Invoke(null, new[] { mockedTableList }) });
                
                var methodBody = Expression.Call(parameter, contextType.GetMethods().Single(m => m.Name == "Set" && m.IsGenericMethod).MakeGenericMethod(entityType));

                var setExpression = createLambda.Invoke(null, new object[] { methodBody, new ParameterExpression[] { parameter } });

                var iSetupSet = iSetupMethod.Invoke(this, new object[] { setExpression });
                returnMethod.Invoke(iSetupSet, new object[] { method.Invoke(null, new[] { mockedTableList }) });

            }

        }

        public static DbSet<T> MockDbSet<T>(List<T> table) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>(table.Add);
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(table.AddRange);
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>(t => table.Remove(t));
            dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
            {
                foreach (var t in ts) { table.Remove(t); }
            });
            return dbSet.Object;
        }
        
    }

}
