using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shopbridge_base.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shopbridge_base.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly Shopbridge_Context dbcontext;
        private readonly IConfiguration _configuration;

        public Repository(Shopbridge_Context _dbcontext,IConfiguration configuration)
        {
            this.dbcontext = _dbcontext;
            this._configuration = configuration;
        }
       
        public async Task<IEnumerable<Product>> Get<Product>()
        {
            try
            {
                var AllData= (IEnumerable<Product>)await dbcontext.Product.ToListAsync();
                return AllData;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task <Product> GetProductbyId(int id)
        {
            try
            {
                return await dbcontext.Product.FindAsync(id);
            }
            catch(Exception EX)
            {
                return null;
            }
            
        }

        public async Task <bool> DeleteProductbyId(int id)
        {
            var NeedToDelete =await dbcontext.Product.FindAsync(id);

            if (NeedToDelete == null)
                return false;
             dbcontext.Remove(NeedToDelete);
            await dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<string> AddProduct(Product product)
        {
            try
            {
                var result = await dbcontext.Product.AddAsync(product);
                 await dbcontext.SaveChangesAsync();

                return "Success";
            }
            catch(Exception ex)
            {
                return ex.InnerException.Message;
            }
            
        }

        public async Task<string> InsertProduct(int Id, Product product)
        {
            try
            {
                var m_product = await dbcontext.Product.FindAsync(product.Product_Id);
                if (m_product == null)
                    return "id not matched from DB Values";

                m_product.Product_Id = product.Product_Id;
                m_product.ProductName = product.ProductName;
                m_product.ProductDescription = product.ProductDescription;
                m_product.ProductPrice = product.ProductPrice;
                m_product.ProductRating = product.ProductRating;

                await dbcontext.SaveChangesAsync();
                return "Success";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        
        public async Task<string> Login(SignInModel _signInModel)
        {
            try
            {
                if (_signInModel.Email == "Raees@gmail.com" && _signInModel.Password == "123")
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,_signInModel.Email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

                    var authSinInkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                    var Token = new JwtSecurityToken(

                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(2),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSinInkey, SecurityAlgorithms.HmacSha256Signature)


                        );

                    return new JwtSecurityTokenHandler().WriteToken(Token);
                }
                else
                    return "error";
            }
            catch(Exception ex)
            {
                return null;
            }
          
        }
    }
}
