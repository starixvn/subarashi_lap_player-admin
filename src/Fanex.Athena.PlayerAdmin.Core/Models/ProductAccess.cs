using Fanex.Athena.PlayerAdmin.Core.Constants;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class ProductAccess
    {
        public ProductAccess(IList<Product> productList)
        {
            _productList = new ReadOnlyCollection<Product>(productList);
        }

        private readonly ReadOnlyCollection<Product> _productList;

        public ReadOnlyCollection<Product> ProductList => _productList;

        public IEnumerable<string> ProductCodeList => _productList.Select(_ => _.ProductCode);

        public string ProductCodeListString => string.Join(",", ProductCodeList);

        public IEnumerable<ProductOption> ProductOptionList => ProductList.Select(p => new ProductOption { Code = p.ProductCode, Name = p.ProductName, Status = true });

        public bool IsAllow(string productCode) => _productList.Any(p => p.ProductCode == productCode);

        public string GetProductName(string productCode)
        {
            var product = _productList.FirstOrDefault(p => p.ProductCode == productCode);
            return product?.ProductName ?? string.Empty;
        }
    }
}