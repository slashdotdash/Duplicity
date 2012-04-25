namespace Duplicity.Collections
{
    public interface ISimpleNodeVisitor<T>
    {
        void Visit(SimpleTreeNode<T> node);
    }
}
