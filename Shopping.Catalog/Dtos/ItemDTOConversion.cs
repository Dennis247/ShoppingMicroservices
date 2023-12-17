using Shopping.Catalog.Entities;

namespace Shopping.Catalog.Dtos
{
    public static class ItemDTOConversion
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO(Id: item.Id, Name: item.Name, Description: item.Description, Price: item.Price, CreatedTime: item.CreatedDate);
        }
    }
}
