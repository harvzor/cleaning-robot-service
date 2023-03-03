using Confluent.Kafka;

namespace CleaningRobotService.Messenger;

public class Producer<TMessage> where TMessage : IMessage
{
    private readonly ProducerConfig _producerConfig;
    
    public Producer()
    {
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };
    }

    public async void Produce(TMessage message)
    {
        using IProducer<Guid, TMessage> producer = new ProducerBuilder<Guid, TMessage>(_producerConfig).Build();
        
        DeliveryResult<Guid, TMessage>? result = await producer.ProduceAsync(
            "weblog", 
            new Message<Guid, TMessage> { Key = message.Id, Value = message, }
        );
    }
}
