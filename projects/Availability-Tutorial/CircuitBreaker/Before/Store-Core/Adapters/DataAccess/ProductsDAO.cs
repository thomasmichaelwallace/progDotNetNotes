using System.Collections.Generic;
using Simple.Data;
using Simple.Data.Ado;
using Store_Core.ReferenceData;

namespace Store_Core.Adapters.DataAccess
{
    public class ProductsDAO : IProductsDAO
    {
        private readonly dynamic _db;

        public ProductsDAO()
        {
            _db = Database.OpenConnection("Data Source=.;Initial Catalog=Store;Application Name=Availability_Tutorial;Connect Timeout=60;Trusted_Connection=True");
        }

        public dynamic BeginTransaction()
        {
            var tx = _db.BeginTransaction();
            tx.WithOptions(new AdoOptions(commandTimeout: 1));
            return tx;
        }

        public ProductReference Add(ProductReference newProductReference)
        {
            return _db.Products.Insert(newProductReference);
        }

        public void Clear()
        {
            _db.Products.DeleteAll();
        }

        public void Delete(int productId)
        {
            _db.Products.DeleteById(productId);
        }

        public IEnumerable<ProductReference> FindAll()
        {
            return _db.Products.All().ToList<ProductReference>();
        }

        public ProductReference FindById(int id)
        {
            return _db.Products.FindById(id);
        }

        public void Update(ProductReference productReference)
        {
            _db.Products.UpdateById(productReference);
        }

    }
}
