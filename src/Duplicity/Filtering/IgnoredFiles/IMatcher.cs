namespace Duplicity.Filtering.IgnoredFiles
{
    interface IMatcher
    {
        bool IsMatch(string path);
    }
}
