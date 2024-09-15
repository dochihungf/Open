namespace Open.MessageBroker.IntegrationEventLogs;

public enum EventStateEnum : int
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFailed = 3
}

