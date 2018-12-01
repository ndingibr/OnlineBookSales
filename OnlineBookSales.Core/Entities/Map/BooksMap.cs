using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineBookSales.Core.Entities.Map
{
    public class BooksMap
    {
        public BooksMap(EntityTypeBuilder<Books> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            entityBuilder.Property(t => t.Text).IsRequired().HasMaxLength(300);
            entityBuilder.Property(t => t.PurchasePrice).IsRequired();
        }
    }
}
