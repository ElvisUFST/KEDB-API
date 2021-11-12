using System;

namespace KEDB.Audit
{
    public class UserAction
    {
        public UserAction(string user, UserActionType actionType, EntityType entityType, string entityId, object entity)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            ActionType = actionType;
            EntityType = entityType;
            EntityId = entityId;
            Entity = entity;
            EventTime = DateTime.UtcNow;
        }

        public string User { get; }
        public UserActionType ActionType { get; }
        public EntityType EntityType { get; }
        public string EntityId { get; }
        public object Entity { get; }
        public DateTime EventTime { get; }
    }
}
