using MediatR;
using InventoryManagement.Data.Repositories;
using InventoryManagement.Data.Entities;
using InventoryManagement.Features.Commands;
using InventoryManagement.Features.Queries;
using InventoryManagement.Exceptions;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;

namespace InventoryManagement.Features.Handlers
{
    public class ProductHandler : 
        IRequestHandler<CreateProductCommand, int>,
        IRequestHandler<UpdateProductCommand, bool>,
        IRequestHandler<DeleteProductCommand, bool>,
        IRequestHandler<GetProductByIdQuery, Product>,
        IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;
        private readonly InventoryDbContext _context;

        public ProductHandler(IProductRepository repository, InventoryDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                PartNumber = request.PartNumber,
                Name = request.Name,
                CostPrice = request.CostPrice,
                StockQuantity = request.StockQuantity
            };

            return await _repository.CreateProductAsync(product);
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);
            if (product == null)
                return false;

            product.PartNumber = request.PartNumber;
            product.Name = request.Name;
            product.CostPrice = request.CostPrice;
            product.StockQuantity = request.StockQuantity;

            return await _repository.UpdateProductAsync(product);
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);
            if (product == null)
                return false;

            return await _repository.DeleteProductAsync(request.Id);
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {request.Id} not found");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }
    }
}
