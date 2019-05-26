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
            //There is a timeout here as well, for connections. We talk about that later, for now it is also set of infinite wait
            _db = Database.OpenConnection("Data Source=.;Initial Catalog=Products;Application Name=Availability_Tutorial;Connect Timeout=0;Trusted_Connection=True");
        }

        public dynamic BeginTransaction()
        {
            var tx = _db.BeginTransaction();
            //The command will timeout at 30s (too long!!) by default, unless we tell it not too
            //So let's set an infinite timeout, to show you how an application performs when it does not timeout
            //N.B. Setting it to 30 or 60s is just as valid to show you tying up a resource.
            //The minumum you can set is 1s, which is still TOO LONG.
            tx.WithOptions(new AdoOptions(commandTimeout: 0));
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
