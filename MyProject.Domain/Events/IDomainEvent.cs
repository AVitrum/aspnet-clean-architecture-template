using MediatR;

namespace MyProject.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}

