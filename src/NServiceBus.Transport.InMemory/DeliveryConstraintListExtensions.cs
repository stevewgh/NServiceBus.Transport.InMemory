namespace NServiceBus.Transport.InMemory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DelayedDelivery;
    using DeliveryConstraints;

    public static class DeliveryConstraintListExtensions
    {
        public static DateTime? DelayUntil(this List<DeliveryConstraint> constraints)
        {
            var delay = constraints.OfType<DelayDeliveryWith>().FirstOrDefault()?.Delay;
            if (delay == null)
            {
                return constraints.OfType<DoNotDeliverBefore>().FirstOrDefault()?.At;
            }

            return DateTime.UtcNow + delay;
        }
    }
}
