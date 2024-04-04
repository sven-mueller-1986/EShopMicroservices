﻿namespace EShopMicroservices.Services.Catalog.API.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid id) : base($"Product with id {id} was not found.")
    { }
}

