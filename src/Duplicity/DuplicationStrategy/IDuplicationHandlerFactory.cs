namespace Duplicity.DuplicationStrategy
{
    public interface IDuplicationHandlerFactory
    {
        IDuplicationHandler Create(FileSystemChange modification);
    }
}