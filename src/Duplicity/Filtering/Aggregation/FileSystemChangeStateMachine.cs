using System;
using System.Collections.Generic;

namespace Duplicity.Filtering.Aggregation
{
    public enum FileSystemChangeType
    {
        None,
        Created,
        Deleted,
        Changed,
    }

    public static class FileSystemChangeTypeExtensions
    {
        public static bool Is(this FileSystemChangeType change, FileSystemChangeType equal)
        {
            return change == equal;
        }
    }

    /// <summary>
    /// Simple state machine to aggregate a finite number of file system changes to a single resultant change
    /// </summary>
    /// <example>For example, a file created and then deleted results in no overall change. 
    /// A file modified a number of times then deleted is the same as just deleting the file.</example>
    internal static class FileSystemChangeStateMachine
    {        
        /// <summary>
        /// Get the resultant file system change from the given initial state and action to apply.
        /// </summary>
        public static FileSystemChangeType Get(FileSystemChangeType initial, FileSystemChangeType action)
        {
            return Matrix[initial][action];
        }

        private static readonly IDictionary<FileSystemChangeType, IDictionary<FileSystemChangeType, FileSystemChangeType>> Matrix =
                new Dictionary<FileSystemChangeType, IDictionary<FileSystemChangeType, FileSystemChangeType>>();

        /// <summary>        
        ///              |          Action             |
        /// Inital State | Created | Changed | Deleted |
        /// -------------|---------|---------|---------|
        /// None         | Created | Changed | Deleted |
        /// Created      |    -    | Created | None    |
        /// Changed      |    -    | Changed | Deleted |
        /// Deleted      | Changed |    -    |   -     |
        /// </summary>
        static FileSystemChangeStateMachine()
        {
            Configure(FileSystemChangeType.None)
                .Permit(FileSystemChangeType.Created, FileSystemChangeType.Created)
                .Permit(FileSystemChangeType.Changed, FileSystemChangeType.Changed)
                .Permit(FileSystemChangeType.Deleted, FileSystemChangeType.Deleted);

            Configure(FileSystemChangeType.Created)
                .Permit(FileSystemChangeType.Changed, FileSystemChangeType.Created)
                .Permit(FileSystemChangeType.Deleted, FileSystemChangeType.None);

            Configure(FileSystemChangeType.Changed)
                .Permit(FileSystemChangeType.Changed, FileSystemChangeType.Changed)
                .Permit(FileSystemChangeType.Deleted, FileSystemChangeType.Deleted);

            Configure(FileSystemChangeType.Deleted)
                .Permit(FileSystemChangeType.Created, FileSystemChangeType.Changed);
        }
        
        /// <summary>
        /// Configure the permitted actions and their result for a given initiat state.
        /// </summary>
        /// <param name="initialState"></param>
        /// <returns>A builder to allow fluent configuration.</returns>
        private static ConfigurationBuilder Configure(FileSystemChangeType initialState)
        {
            if (Matrix.ContainsKey(initialState)) 
                throw new InvalidOperationException(string.Format(@"Initiate state ""{0}"" has already been configured", initialState));

            return new ConfigurationBuilder(Matrix[initialState] = new Dictionary<FileSystemChangeType, FileSystemChangeType>());
        }

        internal sealed class ConfigurationBuilder
        {
            private readonly IDictionary<FileSystemChangeType, FileSystemChangeType> _config;

            public ConfigurationBuilder(IDictionary<FileSystemChangeType, FileSystemChangeType> config)
            {
                _config = config;
            }

            public ConfigurationBuilder Permit(FileSystemChangeType action, FileSystemChangeType result)
            {
                _config.Add(action, result);
                return this;
            }
        }
    }
}