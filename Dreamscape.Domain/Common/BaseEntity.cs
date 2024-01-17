namespace Dreamscape.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        //public DateTimeOffset DataCreated { get; set; }
        //public DateTimeOffset DataUpdated { get; set; }
        //public DateTimeOffset DataDeleted { get; set; }
    }
}
