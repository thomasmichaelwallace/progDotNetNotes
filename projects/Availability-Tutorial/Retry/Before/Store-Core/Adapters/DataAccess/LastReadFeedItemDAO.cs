using System;
using System.Collections.Generic;
using Simple.Data;
using Store_Core.Adapters.Atom;

namespace Store_Core.Adapters.DataAccess
{
    public class LastReadFeedItemDAO : ILastReadFeedItemDAO
    {
        private readonly dynamic _db;

        public LastReadFeedItemDAO()
        {
            _db = Database.OpenConnection("Data Source=.;Initial Catalog=Store;Application Name=Availability_Tutorial;Connect Timeout=60;Trusted_Connection=True");
        }

        public dynamic BeginTransaction()
        {
            return _db.BeginTransaction();
        }

        public LastReadFeedItem Add(LastReadFeedItem lastReadFeedItem)
        {
            return _db.LastReadFeedItem.Insert(lastReadFeedItem);
        }

        public void Clear()
        {
            _db.LastReadFeedItem.DeleteAll();
        }

        public void Delete(int lastFeedItemId)
        {
            _db.LastReadFeedItem.DeleteById(lastFeedItemId);
        }

        public IEnumerable<LastReadFeedItem > FindAll()
        {
            return _db.LastReadFeedItem.All().ToList<LastReadFeedItem >();
        }

        public LastReadFeedItem FindByFeedId(Guid id)
        {
            return _db.LastReadFeedItem.FindByFeedId(id);
        }

        public void Update(LastReadFeedItem productReference)
        {
            _db.LastReadFeedItem.UpdateById(productReference);
        }

    }
}
