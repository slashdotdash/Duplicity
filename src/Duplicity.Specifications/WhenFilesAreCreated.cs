using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Machine.Specifications;

namespace Duplicity.Specifications
{
    [Subject(typeof(DirectoryObservable))]
    public sealed class WhenFilesAreCreated
    {
        private static string _sourceDirectory;
        private static DirectoryObservable _observable;
        private static IDisposable _subscription;
        private static IList<FileSystemChange> _changes = new List<FileSystemChange>();

        private Establish context = () =>
                                        {
                                            _sourceDirectory = TempPath.GetTempDirectoryName();

                                            _observable = new DirectoryObservable(_sourceDirectory);
                                            _subscription = _observable.Subscribe(change => _changes.Add(change));

                                            Thread.Sleep(1000);
                                        };

        private Because of = () => File.Create(Path.Combine(_sourceDirectory, "New File.txt")).Close();

        private It should_notify_new_file_created = () => _changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => _changes.Single().Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => _changes.Single().Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_changed_file_path = () => _changes.Single().FileOrDirectoryName.ShouldEqual(Path.Combine(_sourceDirectory, "New File.txt"));

        private Cleanup dispose = () =>
                                      {
                                          _subscription.Dispose();
                                          _observable.Dispose();
                                          Directory.Delete(_sourceDirectory, true);
                                      };
    }
}