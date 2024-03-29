﻿#region Licence
/* The MIT License (MIT)
Copyright © 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using Orders_Core.Adapters.MailGateway;
using Orders_Core.Model;
using Orders_Core.Ports.Commands;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.logging.Attributes;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.policy.Attributes;

namespace Orders_Core.Ports.Handlers
{
    public class OrderUpdateCommandHandler : RequestHandler<OrderUpdateCommand>
    {
        private readonly IAmAMailGateway _mailGateway;

        public OrderUpdateCommandHandler(IAmAMailGateway mailGateway, ILog logger) : base(logger)
        {
            _mailGateway = mailGateway;
        }

        [RequestLogging(step: 1, timing: HandlerTiming.Before)]
        [UsePolicy(CommandProcessor.CIRCUITBREAKER, step: 2)]
        [UsePolicy(CommandProcessor.RETRYPOLICY, step: 3)]
        public override OrderUpdateCommand Handle(OrderUpdateCommand orderUpdateCommand)
        {
            _mailGateway.Send(new OrderUpdate(
                orderReference: new OrderReference(orderUpdateCommand.OrderName),
                dueDate: orderUpdateCommand.DueDate,
                reminderTo: new EmailAddress(orderUpdateCommand.Recipient),
                copyReminderTo: new EmailAddress(orderUpdateCommand.CopyTo)
                ));

            return base.Handle(orderUpdateCommand);
        }
    }
}
