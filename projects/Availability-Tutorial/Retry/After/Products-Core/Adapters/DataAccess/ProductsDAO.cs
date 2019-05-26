using System.Collections.Generic;
using Products_Core.Model;
using Simple.Data;
using Simple.Data.Ado;

namespace Products_Core.Adapters.DataAccess
{
    public class ProductsDAO : IProductsDAO
    {
        private readonly dynamic _db;

        public ProductsDAO()
        {
            _db = Database.OpenConnection("Data Source=.;Initial Catalog=Products;Application Name=Availability_Tutorial;Connect Timeout=60;Trusted_Connection=True");
        }

        public dynamic BeginTransaction()
        {
            var tx = _db.BeginTransaction();
            tx.WithOptions(new AdoOptions(commandTimeout: 1));
            return tx;
        }

        public Product Add(Product newProduct)
        {
            return _db.Products.Insert(newProduct);
        }

        public void Clear()
        {
            _db.Products.DeleteAll();
        }

        public void Delete(int productId)
        {
            _db.Products.DeleteById(productId);
        }

        public IEnumerable<Product> FindAll()
        {
            return _db.Products.All().ToList<Product>();
        }

        public Product FindById(int id)
        {
            return _db.Products.FindById(id);
        }

        public void Update(Product product)
        {
            _db.Products.UpdateById(product);
        }

    }
}
