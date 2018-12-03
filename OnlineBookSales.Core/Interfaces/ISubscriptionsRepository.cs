using System;
using System.Collections.Generic;
using System.Text;
using OnlineBookSales.Core.Entities;

namespace OnlineBookSales.Core.Interfaces
{
    public interface ISubscriptionsRepository
    {

        bool UnSubscribe(Subscriptions subscription);
        bool Subscribe(Subscriptions subscription);
    }
}
