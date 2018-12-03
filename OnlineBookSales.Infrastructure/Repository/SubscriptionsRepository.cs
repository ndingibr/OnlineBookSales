using OnlineBookSales.Core.Entities;
using OnlineBookSales.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineBookSales.Infrastructure.Repository
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private IRepository<Subscriptions> _subscriptionsRepo;

        public SubscriptionsRepository(IRepository<Subscriptions> subscriptionsRepo)
        {
            _subscriptionsRepo = subscriptionsRepo;
        }

        public bool UnSubscribe(Subscriptions subscription)
        {
            var subscriptionToDelete = _subscriptionsRepo.GetAll().FirstOrDefault(x => x.BookId == subscription.BookId &&
                                 x.UserId == subscription.UserId);

            _subscriptionsRepo.Delete(subscriptionToDelete);

            return true;
        }

        public bool Subscribe(Subscriptions subscription)
        {
            _subscriptionsRepo.Insert(subscription);
            return true;
        }
    }
}
