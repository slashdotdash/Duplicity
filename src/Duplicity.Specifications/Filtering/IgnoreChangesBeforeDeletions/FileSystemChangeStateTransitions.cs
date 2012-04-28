using Duplicity.Filtering;
using Duplicity.Filtering.Aggregation;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class FileSystemChangeStateTransitions
    {
        private It should_create_from_nothing = () => Transition(FileSystemChangeType.None, FileSystemChangeType.Created).ShouldEqual(FileSystemChangeType.Created);
        private It should_be_nothing_for_create_then_delete = () => Transition(FileSystemChangeType.Created, FileSystemChangeType.Deleted).ShouldEqual(FileSystemChangeType.None);
        private It should_be_created_after_modifying = () => Transition(FileSystemChangeType.Created, FileSystemChangeType.Changed).ShouldEqual(FileSystemChangeType.Created);
        private It should_be_deleted_when_deleting_after_modification = () => Transition(FileSystemChangeType.Changed, FileSystemChangeType.Deleted).ShouldEqual(FileSystemChangeType.Deleted);

        private static FileSystemChangeType Transition(FileSystemChangeType initial, FileSystemChangeType action)
        {
            return FileSystemChangeStateMachine.Get(initial, action);
        }
    }
}
