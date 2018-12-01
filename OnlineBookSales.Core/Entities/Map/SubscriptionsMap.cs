using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineBookSales.Core.Entities.Map
{
    public class SubscriptionsMap
    {
        public SubscriptionsMap(EntityTypeBuilder<Subscriptions> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserId).IsRequired().HasMaxLength(50);
            entityBuilder.Property(t => t.BookId).IsRequired().HasMaxLength(50);
        }
    }
}
