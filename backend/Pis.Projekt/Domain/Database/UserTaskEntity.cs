using System;
using System.Collections.Generic;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class UserTaskEntity: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public UserTaskType Type { get; set; }
        public IEnumerable<SalesAggregateEntity> Sales { get; set; }
    }

    public enum UserTaskType
    {
        TODO_A,
        TODO_B
    }
}