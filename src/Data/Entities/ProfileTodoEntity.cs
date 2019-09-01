using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TIKSN.Data;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities
{
    public class ProfileTodoEntity : IEntity<Guid>
    {
        public List<ChecklistItemModel> Checklist { get; set; }

        public bool? CollapseChecklist { get; set; }

        public bool? Completed { get; set; }

        public long? CounterDown { get; set; }

        public long? CounterUp { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? Date { get; set; }

        public Guid DatumId { get; set; }

        public List<long> DaysOfMonth { get; set; }

        public bool? Down { get; set; }

        public long? EveryX { get; set; }

        public string Frequency { get; set; }

        [BsonId]
        public Guid ID { get; set; }

        public bool? IsDue { get; set; }

        public string Notes { get; set; }

        public double Priority { get; set; }

        public Guid ProviderProfileID { get; set; }

        public Guid ProviderUniformID { get; set; }

        public Guid ProviderUserID { get; set; }

        public RepeatModel Repeat { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public long? Streak { get; set; }

        public List<Guid> Tags { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public bool? Up { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public Guid UserId { get; set; }

        public long Value { get; set; }

        public List<long> WeeksOfMonth { get; set; }

        public bool? YesterDaily { get; set; }

        public class ChecklistItemModel
        {
            public bool Completed { get; set; }
            public Guid Id { get; set; }
            public string Text { get; set; }
        }

        public class RepeatModel
        {
            public bool F { get; set; }
            public bool M { get; set; }
            public bool S { get; set; }
            public bool Su { get; set; }
            public bool T { get; set; }
            public bool Th { get; set; }
            public bool W { get; set; }
        }
    }
}