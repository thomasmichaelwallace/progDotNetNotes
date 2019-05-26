using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using paramore.brighter.commandprocessor;
using Polly.CircuitBreaker;
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
        public HttpResponseMessage CreateProduct(AddProductModel newProduct)
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

            //We switch here from .Send to .Post; which is easy, although we have to ensure
            //that the service is configured with information on the broker to push to
            //and we have to have a service to consumer from the queue
            try
            {
                _commandProcessor.Post(addProductCommand);
            }
            catch (Exception e)
            {
                var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("There was an internal error, please wait and try again later")
                };
                throw new HttpResponseException(message);
                
            }

            //We no longer return the created entity in the body, as we can't, but an indication that we have accepted the request
            //and you can obtain it later.    
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
