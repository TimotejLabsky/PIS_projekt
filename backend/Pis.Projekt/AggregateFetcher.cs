using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt
{
    public class AggregateFetcher
    {
        public AggregateFetcher(ILogger<AggregateFetcher> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// System nacita vsetky produkty z uloziska za minuly tyzden aj zo vsetkymi aggregovanymi udajmi
        /// </summary>
        /// <remarks>
        /// Aggregovany udaj je cena, predajnost etc...
        /// </remarks>
        /// <returns></returns>
        public async Task<IEnumerable<SalesAggregate>> FetchSalesAggregatesAsync(
            DateTime currentDate,
            ISalesAggregateRepository repository)
        {
            _logger.LogBusinessCase(BusinessTasks.FetchLastAggregates);
            _logger.LogInput(BusinessTasks.FetchLastAggregates, "Dnešný dátum", currentDate);
            _logger.LogInput(BusinessTasks.FetchLastAggregates, "Úložisko", repository, false);
            var week = GetWeekNumberOfMonth(currentDate);
            var sales = await repository
                .FetchFromLastWeekAsync(week)
                .ConfigureAwait(false);
            _logger.LogTrace($"Fetched sales: {sales}");
            var aggregates = _mapper.Map<IEnumerable<SalesAggregate>>(sales);
            _logger.LogOutput(BusinessTasks.FetchLastAggregates, "Aggregovane predaje podla typu produktu", aggregates);
            return aggregates;
        }

        static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
        public async Task<Season> FetchSeasonAsync(DateTime currentDate, ISeasonRepository repository)
        {
            _logger.LogBusinessCase(BusinessTasks.FetchLastSeason);
            _logger.LogInput(BusinessTasks.FetchLastSeason, "Dnešný dátum", currentDate);
            var season = await repository.FetchSeason(currentDate).ConfigureAwait(false);
            _logger.LogOutput(BusinessTasks.FetchLastSeason, "Aktualna sezona", season);
            return season;
        }

        private readonly ILogger<AggregateFetcher> _logger;
        private readonly IMapper _mapper;
    }
}