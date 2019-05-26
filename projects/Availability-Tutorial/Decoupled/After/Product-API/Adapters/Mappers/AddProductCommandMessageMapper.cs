using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using paramore.brighter.commandprocessor;
using Products_Core.Ports.Commands;

namespace Product_API.Adapters.Mappers
{
    internal class AddProductCommandMessageMapper : IAmAMessageMapper<AddProductCommand>
    {
        public Message MapToMessage(AddProductCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Product.Add-Product.Command", messageType: MessageType.MT_COMMAND);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        public AddProductCommand MapToRequest(Message message)
        {
            var data = JObject.Parse(message.Body.Value);

            var productName = (string) data.SelectToken("ProductName");
            var productDescription = (string)data.SelectToken("ProductDescription");
            var productPrice= (double)data.SelectToken("ProductPrice");

            return new AddProductCommand(productName, productDescription, productPrice);
        }
    }
}
