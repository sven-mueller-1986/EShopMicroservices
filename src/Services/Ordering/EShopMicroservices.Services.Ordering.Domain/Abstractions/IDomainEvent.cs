﻿using MediatR;

namespace EShopMicroservices.Services.Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
}

