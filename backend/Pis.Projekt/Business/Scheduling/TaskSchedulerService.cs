using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling
{
    public class TaskSchedulerService : IHostedService
    {
        public TaskSchedulerService(PriceCalculatorService priceCalculator)
        {
            _priceCalculator = priceCalculator;
        }

        public async Task<IEnumerable<PricedProduct>> RegisterDecreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _task = new ProductSalesDecreasedTask("products decreased", products);
            await StartAsync(default).ConfigureAwait(false);
            return _task.Result;
        }

        public async Task<IEnumerable<PricedProduct>> RegisterIncreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _task = new ProductSalesIncreasedTask("products increased", products);
            await StartAsync(default).ConfigureAwait(false);
            return _task.Result;
        }

        public async Task ProcessAsync(IScheduledTask<IEnumerable<PricedProduct>> task, Type type)
        {
            if (type == typeof(ProductSalesIncreasedTask))
            {
                task.Result =
                    await HandleProductSalesIncreasedTask((ProductSalesIncreasedTask) task)
                        .ConfigureAwait(false);
            }
            else if (type == typeof(ProductSalesIncreasedTask))
            {
                task.Result =
                    await HandleProductSalesDecreasedTask((ProductSalesDecreasedTask) task)
                        .ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException($"Unsupported type of Scheduled type of: {type}");
            }
        }

        public async Task<IEnumerable<PricedProduct>> HandleProductSalesIncreasedTask(
            ProductSalesIncreasedTask task)
        {
            foreach (var product in task.Products)
            {
                product.SalesWeek++;
                product.Price = _priceCalculator.CalculatePrice(product);
            }

            await Task.CompletedTask;
            return task.Products;
        }

        public Task<IEnumerable<PricedProduct>> HandleProductSalesDecreasedTask(
            ProductSalesDecreasedTask task)
        {
            return null;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_task != null)
            {
                await ProcessAsync(_task, _task.GetType()).ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => _task = null, cancellationToken);
        }

        private readonly PriceCalculatorService _priceCalculator;
        private IScheduledTask<IEnumerable<PricedProduct>> _task;
    }
}