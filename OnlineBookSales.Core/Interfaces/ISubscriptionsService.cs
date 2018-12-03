using OnlineBookSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBookSales.Core.Interfaces
{
    public interface ISubscriptionsService
    {
        bool Subscribe(Subscriptions subscription);
        bool UnSubscribe(Subscriptions subscription);
    }
}
