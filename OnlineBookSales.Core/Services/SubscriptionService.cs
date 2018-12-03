using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineBookSales.Core.Interfaces;
using OnlineBookSales.Core.Entities;

namespace OnlineBookSales.Core.Services
{
    public class SubscriptionService : ISubscriptionsService
    {
        private ISubscriptionsRepository _subscriptionsRepo;
        public SubscriptionService(ISubscriptionsRepository subscriptionsRepo)
        {
            _subscriptionsRepo = subscriptionsRepo;
        }

        public bool Subscribe(Subscriptions subscription)
        {
            _subscriptionsRepo.Subscribe(subscription);
            return true;
        }

        public bool UnSubscribe(Subscriptions subscription)
        {
            _subscriptionsRepo.UnSubscribe(subscription);

            return true;
        }
    }
}
