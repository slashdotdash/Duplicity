// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

namespace Duplicity.Collections
{
    /// <summary>
    /// Represents a node in a Tree structure, with a parent node and zero or more child nodes.
    /// </summary>
    public class ComplexTreeNode<T> where T : ComplexTreeNode<T>
    {
        private T _parent;

        public T Value
        {
            get { return (T)this; }
        }

        public T Parent
        {
            get { return _parent; }
            set
            {
                if (value == _parent)
                {
                    return;
                }

                if (_parent != null)
                {
                    _parent.Children.Remove(this);
                }

                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }

                _parent = value;
            }
        }

        public T Root
        {
            get { return (T) ((Parent == null) ? this : Parent.Root); }
        }

        public virtual ComplexTreeNodeList<T> Children { get; private set; }

        public ComplexTreeNode()
        {
            Parent = null;
            Children = new ComplexTreeNodeList<T>(this);
        }

        public ComplexTreeNode(T parent)
        {
            Parent = parent;
            Children = new ComplexTreeNodeList<T>(this);
        }

        public ComplexTreeNode(ComplexTreeNodeList<T> children)
        {
            Parent = null;
            Children = children;
            children.Parent = (T)this;
        }

        public ComplexTreeNode(T parent, ComplexTreeNodeList<T> children)
        {
            Parent = parent;
            Children = children;
            children.Parent = (T)this;
        }

        public void Accept(IComplexNodeVisitor<T> visitor)
        {
            visitor.Visit((T)this);
            Children.Each(child => child.Accept(visitor));
        }

        /// <summary>
        /// Reports a depth of nesting in the tree, starting at 0 for the root.
        /// </summary>
        public int Depth
        {
            get {  return (Parent == null ? -1 : Parent.Depth) + 1; }
        }
    }
}