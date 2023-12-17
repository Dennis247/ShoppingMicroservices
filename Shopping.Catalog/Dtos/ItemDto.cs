using System.ComponentModel.DataAnnotations;

namespace Shopping.Catalog.Dtos
{
    public record ItemDTO(Guid Id, string Name, string Description, int Price, DateTimeOffset CreatedTime);
    public record CreateItemDTO([Required] string Name, string Desc, [Range(0, 100)] int Price, string Description);
    public record UpdateItemDTO([Required] string Name, string Desc, [Range(0, 100)] int Price);
}
