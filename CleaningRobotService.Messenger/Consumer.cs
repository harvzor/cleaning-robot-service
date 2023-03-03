using Confluent.Kafka;

namespace CleaningRobotService.Messenger;

public class Consumer<TMessage, TMessageHandler>
    where TMessage : IMessage
    where TMessageHandler : IMessageHandler<TMessage>
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly TMessageHandler _messageHandler;
    
    public Consumer()
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092,localhost:9092",
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        _messageHandler = (TMessageHandler)Activator.CreateInstance(typeof(TMessageHandler))!;
    }

    public async void Consume(TMessage message, CancellationToken cancellationToken)
    {
        using IConsumer<Guid, TMessage>? consumer = new ConsumerBuilder<Guid, TMessage>(_consumerConfig).Build();
        
        consumer.Subscribe("weblog");

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<Guid, TMessage>? consumeResult = consumer.Consume(cancellationToken);

            _messageHandler.Handle(consumeResult.Message.Value);
        }

        consumer.Close();
    }
}
