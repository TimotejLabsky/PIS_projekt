using System;
using System.Collections.Generic;
using System.Net.Mail;
using Newtonsoft.Json;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.DTOs
{
    public class UserTask: IEmail
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
        [JsonProperty("task_type")]
        public UserTaskType Type { get; set; }
        [JsonProperty("sales")]
        public IEnumerable<SalesAggregate> Sales { get; set; }
        
        [JsonIgnore]
        public MailAddress ToMailAddress { get; }
        [JsonIgnore]
        public string Subject { get; }
        [JsonIgnore]
        public string Message => JsonConvert.SerializeObject(this);
    }
}