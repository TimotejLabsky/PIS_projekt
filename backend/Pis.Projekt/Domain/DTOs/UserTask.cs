using System;
using System.Collections.Generic;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.DTOs
{
    public class UserTask
    {
        public Guid Guid { get; set; }
        public UserTaskType Type { get; set; }
        public IEnumerable<SalesAggregateEntity> Sales { get; set; }
    }
}