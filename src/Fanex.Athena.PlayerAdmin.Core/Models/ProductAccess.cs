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
            ProductList = new ReadOnlyCollection<Product>(productList);
        }

        public readonly ReadOnlyCollection<Product> ProductList;

        public IEnumerable<string> ListProductCode => ProductList.Select(_ => _.ProductCode);

        public bool IsAllowBA => ProductList.Any(p => p.ProductCode == ProductCode.BA);

        public bool IsAllow(string productCode) => ProductList.Any(p => p.ProductCode == productCode);
    }
}