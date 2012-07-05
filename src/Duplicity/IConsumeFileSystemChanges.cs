namespace Duplicity
{
    public interface IConsumeFileSystemChanges
    {
        void Consume(IProduceFileSystemChanges producer);
    }
}
