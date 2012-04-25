namespace Duplicity.Collections
{
    public interface IComplexNodeVisitor<in T> where T : ComplexTreeNode<T>
    {
        void Visit(T node);
    }
}
