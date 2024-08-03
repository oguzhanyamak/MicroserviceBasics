using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    static public class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
        public const string Order_StockNotReservedEventQueue = "order-stock-not-reserved-event-queue";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";
        public const string Order_PaymentSuccessEventQueue = "payment-success-event-queue";
        public const string Order_PaymentFailedEventQueue = "payment-failed-event-queue";
    }
}
