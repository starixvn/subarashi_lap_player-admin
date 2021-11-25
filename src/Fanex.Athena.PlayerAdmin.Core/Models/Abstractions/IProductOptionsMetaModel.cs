
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public interface IProductOptionsMetaModel
    {
        IEnumerable<ProductOption> ProductOptions { get; set; }
        IEnumerable<ProductOption> OriginalProductOptions { get; set; }
    }
}