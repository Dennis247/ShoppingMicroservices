using Shopping.Common;
using System.Security.Principal;

namespace Shopping.Catalog.Entities
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
