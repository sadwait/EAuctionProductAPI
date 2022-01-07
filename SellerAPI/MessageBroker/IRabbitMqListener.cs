using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerAPI.MessageBroker
{
    public interface IRabbitMqListener
    {
        void Receive();
    }
}
