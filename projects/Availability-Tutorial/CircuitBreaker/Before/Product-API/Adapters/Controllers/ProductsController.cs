using System.Net;
using System.Net.Http;
using System.Web.Http;
using paramore.brighter.commandprocessor;
using Products_Core.Adapters.DataAccess;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Resources;
using Products_Core.Ports.ViewModelRetrievers;
using Product_Service;

namespace Product_API.Adapters.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductsDAO _productsDao;
        private readonly IAmACommandProcessor _commandProcessor;

        public ProductsController(ProductsDAO productsDao, IAmACommandProcessor commandProcessor)
        {
            _productsDao = productsDao;
            _commandProcessor = commandProcessor;
        }

        [HttpGet]
        public ProductListModel Get()
        {
            var retriever = new ProductListModelRetriever(Globals.HostName, _productsDao);
            return retriever.RetrieveProducts();
        }

        [HttpGet]
        public ProductModel Get(int productId)
        {
            var retriever = new ProductModelRetriever(Globals.HostName, _productsDao);
            return retriever.RetrieveProduct(productId);
        }

        [HttpPost]
        public ProductModel CreateProduct(AddProductModel newProduct)
        {
            if (newProduct == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Product in entity body missing or badly-formed and could not be parsed"),
                    ReasonPhrase = "Missing Product"
                };
                throw new HttpResponseException(message);
            }
                

            var addProductCommand = new AddProductCommand(
                productName: newProduct.ProductName,
                productDescription: newProduct.ProductDescription,
                productPrice: newProduct.ProductPrice
                );

            _commandProcessor.Send(addProductCommand);

            return Get(addProductCommand.ProductId);
        }
    }
}
