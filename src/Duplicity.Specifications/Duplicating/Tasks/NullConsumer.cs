namespace Duplicity.Specifications.Duplicating.Tasks
{
    public sealed class NullConsumer : IConsumeFileSystemChanges
    {
        public bool HasStatedConsuming { get; private set; }

        public void Consume(IProduceFileSystemChanges producer)
        {
            HasStatedConsuming = true;

            // But does not process changes
        }
    }
}
