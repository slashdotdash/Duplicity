﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    public abstract class WithAnObservableDirectory
    {
        protected static string SourceDirectory;
        protected static IList<FileSystemChange> Changes;

        protected static FileSystemChange Change
        {
            get { return Changes.Single(); }
        }

        protected static FileSystemChange ChangeAt(int index)
        {
            return Changes.ElementAt(index);
        }

        private static FileSystemObservable _observable;
        private static IDisposable _subscription;

        protected Establish Context = () =>
        {
            SourceDirectory = TempPath.GetTempDirectoryName();
            Changes = new List<FileSystemChange>();
        };

        protected static void CreateFileSystemObservable()
        {
            _observable = new FileSystemObservable(SourceDirectory);
            _subscription = _observable.Subscribe(change => Changes.Add(change));
            
            Wait.Until(() => _observable.IsAlive);
        }

        protected Cleanup Dispose = () =>
        {
            _subscription.Dispose();
            _observable.Dispose();

            Directory.Delete(SourceDirectory, true);
        };
    }
}