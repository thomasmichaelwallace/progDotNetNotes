#region Licence
/* The MIT License (MIT)
Copyright � 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the �Software�), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System.Collections.Generic;
using Orders_Core.Model;
using Simple.Data;
using Simple.Data.Ado;

namespace Orders_Core.Adapters.DataAccess
{
    public class OrdersDAO : IOrdersDAO
    {
        private readonly dynamic _db;

        public OrdersDAO()
        {
            _db = Database.OpenConnection("Data Source=.;Initial Catalog=Orders;Application Name=Availability_Tutorial;Connect Timeout=60;Trusted_Connection=True");
        }

        public Order Add(Order newOrder)
        {
            return _db.Orders.Insert(newOrder);
        }

        public IEnumerable<Order> FindAll()
        {
            return _db.Orders.All().ToList<Order>();
        }

        public dynamic BeginTransaction()
        {
            var tx = _db.BeginTransaction();
            tx.WithOptions(new AdoOptions(commandTimeout: 1));
            return tx;
        }

        public void Update(Order order)
        {
            _db.Orders.UpdateById(order);
        }

        public void Clear()
        {
            _db.Orders.DeleteAll();
        }

        public Order FindById(int orderId)
        {
            return _db.Orders.FindById(orderId);
        }

        public Order FindByName(string customerName)
        {
            return _db.Orders.FindBy(customerName: customerName);
        }
    }
}