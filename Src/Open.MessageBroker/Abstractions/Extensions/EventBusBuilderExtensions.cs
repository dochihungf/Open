namespace Open.MessageBroker.Abstractions;

public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder AddMessageBroker(this IEventBusBuilder builder, Action<JsonSerializerOptions> configure)
    {
        builder.Services.Configure<EventBusSubscriptionInfo>(info =>  configure(info.JsonSerializerOptions));
        return builder;
    }

    public static IEventBusBuilder 
        AddSubscription<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(this IEventBusBuilder builder)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        // Sử dụng dịch vụ theo khóa để đăng ký nhiều trình xử lý (TH) cho cùng 1 loại sự kiện (T)
        // .AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
        // => T = OrderStatusChangedToStockConfirmedIntegrationEvent, TH = OrderStatusChangedToStockConfirmedIntegrationEventHandler implement IIntegrationEventHandler
        
        // Sử dụng GetKeyedServices<IIntegrationEventHandler>(typeof(OrderStatusChangedToStockConfirmedIntegrationEvent = T)) 
        // để lấy tất cả các trình xử lý cho loại sự kiện đó
        builder.Services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

        builder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            // Keep track of all registered event types and their name mapping. We send these event types over the message bus
            // and we don't want to do Type.GetType, so we keep track of the name mapping here.

            // This list will also be used to subscribe to events from the underlying message broker implementation.
            o.EventTypes[typeof(T).Name] = typeof(T);
        });

        return builder;
    }
    
}
