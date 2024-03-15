using BookingApp.Server.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace BookingApp.Server.Controllers.WebApi
{
    public class ProductController : ControllerBase
    {
		private readonly DatabaseContext _context;
        public ProductController(DatabaseContext context)
		{
			_context = context;
		}
        public IResult Get(int page)
        {
			try
			{
				const int pageSize = 10;
                var count = _context.Product.Count();
                var products = _context.Product.Skip(page * pageSize).Take(10).ToList();
				var maxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);
				dynamic data = new ExpandoObject();
				data.products = products;
				data.maxPage = maxPage;
				return Results.Ok(data);
			}
			catch (Exception ex)
			{
				return Results.BadRequest(ex.Message.ToString());
			}
        }
    }
}
