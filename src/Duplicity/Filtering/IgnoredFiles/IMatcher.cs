namespace Duplicity.Filtering.IgnoredFiles
{
    interface IMatcher
    {
        bool IsMatch(FileSystemChange change);
    }
}
