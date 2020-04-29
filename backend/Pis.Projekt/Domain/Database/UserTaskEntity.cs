using System;
using System.Collections.Generic;

namespace Pis.Projekt.Domain.Database
{
    public class UserTaskEntity
    {
        public Guid Guid { get; set; }
        public UserTaskType Type { get; set; }
        public IEnumerable<SalesAggregateEntity> Sales { get; set; }
    }

    public enum UserTaskType
    {
        TODO_A,
        TODO_B
    }
}