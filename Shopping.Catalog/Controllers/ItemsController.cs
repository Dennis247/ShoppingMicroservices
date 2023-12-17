using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shopping.Catalog.Contracts;
using Shopping.Catalog.Dtos;
using Shopping.Catalog.Entities;
using Shopping.Common;

namespace Shopping.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> repo,IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = repo ?? throw new ArgumentNullException(nameof(repo));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException( nameof(publishEndpoint));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDTO());
            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDTO createItemDTO)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(item);
            await _publishEndpoint.Publish(new CatalogItemCreated(ItemId: item.Id, Name: item.Name, Description: item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDTO>> PutAsync(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = await _itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Desc;
            existingItem.Price = updateItemDTO.Price;

            await _itemsRepository.UpdateAsync(existingItem);
            await _publishEndpoint.Publish(new CatalogItemUpdated(ItemId: existingItem.Id, Name: existingItem.Name, Description: existingItem.Description));

            return Ok(existingItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDTO>> DeleteAsync(Guid id)
        {
            var existingItem = await _itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            await _itemsRepository.RemoveAsync(id);
            await _publishEndpoint.Publish(new CatalogItemDeleted(ItemId: id));

            return Ok(existingItem);
        }

    }
}
