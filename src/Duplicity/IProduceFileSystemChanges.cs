namespace Duplicity
{
    public interface IProduceFileSystemChanges
    {
        bool IsEmpty { get; }

        bool Add(FileSystemChange change);
        bool TryTake(out FileSystemChange change);
    }
}
