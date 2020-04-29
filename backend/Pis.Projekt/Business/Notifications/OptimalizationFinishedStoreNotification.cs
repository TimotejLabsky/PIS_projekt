using System;
using System.Collections.Generic;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications
{
    public class OptimalizationFinishedStoreNotification: INotification<IEnumerable<PricedProduct>>
    {
        public Type Type { get; set; }
        public IEnumerable<PricedProduct> Content { get; set; }
        
        public static OptimalizationFinishedStoreNotification Create(IEnumerable<PricedProduct> content)
        {
            return new OptimalizationFinishedStoreNotification
            {
                Type = typeof(OptimalizationFinishedStoreNotification),
                Content = content
            };
        }
    }
}