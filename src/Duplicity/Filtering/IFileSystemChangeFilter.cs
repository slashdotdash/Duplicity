namespace Duplicity.Filtering
{
    public interface IFileSystemChangeFilter
    {
        /// <summary>
        /// Should the given change be filtered out?
        /// </summary>
        /// <param name="change">File system change to asses.</param>
        /// <returns>true to exclude/ignore the change, otherwise false.</returns>
        bool Filter(FileSystemChange change);
    }
}