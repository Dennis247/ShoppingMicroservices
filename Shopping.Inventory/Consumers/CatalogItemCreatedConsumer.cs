﻿using Shopping.Catalog.Contracts;
using Shopping.Common;
using Shopping.Inventory.Entities;
using MassTransit;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = await repository.GetByIdAsync(message.ItemId);

            if (item != null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };

            await repository.CreateAsync(item);
        }
    }
}