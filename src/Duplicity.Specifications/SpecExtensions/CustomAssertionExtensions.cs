using Machine.Specifications;

namespace Duplicity.Specifications.SpecExtensions
{
    internal static class CustomAssertionExtensions
    {
        public static FileSystemChange ShouldEqual(this FileSystemChange actual, FileSystemChange expected)
        {
            actual.Change.ShouldEqual(expected.Change);
            actual.Source.ShouldEqual(expected.Source);
            actual.FileOrDirectoryPath.ShouldEqual(expected.FileOrDirectoryPath);
            return actual;
        }
    }
}
