using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{

		private readonly IConfiguration _configuration;

		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var coupon = await connection.QueryFirstAsync<Coupon>
				("select * from Coupon where ProductName = @productName",
				new
				{
					ProductName = productName,
				});
			if (coupon == null)
			{
				return new Coupon { Amount = 0, Description = "No Discount Available", ProductName = "no Product" };
			}
			return coupon;
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));


			var effected = await connection.ExecuteAsync
			("INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)",
			new
			{
				ProductName = coupon.ProductName,
				Amount = coupon.Amount,
				Description = coupon.Description,
			});
			if (effected == 0)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var effected = await connection.ExecuteAsync
			("DELETE FROM Coupon where ProductName = @productName",
			new
			{
				ProductName = productName,
			});
			if (effected == 0)
			{
				return false;
			}
			return true;
		}



		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var effected = await connection.ExecuteAsync
			("UPDATE  Coupon set ProductName = @ProductName,Description = @Description,Amount = @Amount where Id = @id",
			new
			{
				ProductName = coupon.ProductName,
				Amount = coupon.Amount,
				Description = coupon.Description,
				Id = coupon.Id,

			});
			if (effected == 0)
			{
				return false;
			}
			return true;
		}
	}
}
