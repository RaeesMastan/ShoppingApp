using Microsoft.AspNetCore.Mvc;
using Shopbridge_base.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shopbridge_base.Data.Repository
{
    public interface IRepository
    {
        //IQueryable<T> AsQueryable<T>() where T : class;
        //IQueryable<T> Get<T>(params Expression<Func<T, object>>[] navigationProperties) where T : class;
        //IQueryable<T> Get<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class;
        //IEnumerable<T> Get<T>() where T : class;

        Task<IEnumerable<Product>> Get<Product>();
        Task <Product> GetProductbyId(int id);
        public Task <bool> DeleteProductbyId(int id);
        public Task<string> AddProduct(Product product);

        Task<string> InsertProduct(int Id, Product product);

        public Task<string> Login(SignInModel _signInModel);
    }
}
