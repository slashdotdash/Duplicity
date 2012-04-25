// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System.Collections.Generic;

namespace Duplicity.Collections
{
    /// <summary>
    /// Represents a node in a SimpleTree structure, with a parent node and zero or more child nodes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleTreeNode<T>
    {
        private SimpleTreeNode<T> _parent;

        public SimpleTreeNode<T> Parent
        {
            get { return _parent; }
            set
            {
                if (value == _parent)
                    return;

                if (_parent != null)
                    _parent.Children.Remove(this);

                if (value != null && !value.Children.Contains(this))
                    value.Children.Add(this);

                _parent = value;
            }
        }

        public SimpleTreeNode<T> Root
        {
            get { return (Parent == null) ? this : Parent.Root; }
        }

        public SimpleTreeNodeList<T> Children { get; private set; }

        public T Value { get; set; }

        public SimpleTreeNode()
        {
            Parent = null;
            Children = new SimpleTreeNodeList<T>(this);
        }

        public SimpleTreeNode(T value)
        {
            Value = value;
            Children = new SimpleTreeNodeList<T>(this);
        }

        public SimpleTreeNode(SimpleTreeNode<T> parent)
        {
            Parent = parent;
            Children = new SimpleTreeNodeList<T>(this);
        }

        public void Accept(ISimpleNodeVisitor<T> visitor)
        {
            visitor.Visit(this);
            Children.Each(child => child.Accept(visitor));
        }

        /// <summary>
        /// Reports a depth of nesting in the tree, starting at 0 for the root.
        /// </summary>
        public int Depth
        {
            get { return (Parent == null ? -1 : Parent.Depth) + 1; }
        }

        public IEnumerable<SimpleTreeNode<T>> GetEnumerable(TreeTraversalType traversalType, TreeTraversalDirection traversalDirection)
        {
            switch (traversalType)
            {
                case TreeTraversalType.DepthFirst: return GetDepthFirstEnumerable(traversalDirection);
                case TreeTraversalType.BreadthFirst: return GetBreadthFirstEnumerable(traversalDirection);
                default: return null;
            }
        }

        private IEnumerable<SimpleTreeNode<T>> GetDepthFirstEnumerable(TreeTraversalDirection traversalDirection)
        {
            if (traversalDirection == TreeTraversalDirection.TopDown)
                yield return this;

            foreach (SimpleTreeNode<T> child in Children)
            {
                var e = child.GetDepthFirstEnumerable(traversalDirection).GetEnumerator();
                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }

            if (traversalDirection == TreeTraversalDirection.BottomUp)
                yield return this;
        }

        // TODO: adjust for traversal direction
        private IEnumerable<SimpleTreeNode<T>> GetBreadthFirstEnumerable(TreeTraversalDirection traversalDirection)
        {
            if (traversalDirection == TreeTraversalDirection.BottomUp)
            {
                var stack = new Stack<SimpleTreeNode<T>>();
                foreach (var item in GetBreadthFirstEnumerable(TreeTraversalDirection.TopDown))
                {
                    stack.Push(item);
                }
                while (stack.Count > 0)
                {
                    yield return stack.Pop();
                }
                yield break;
            }

            var queue = new Queue<SimpleTreeNode<T>>();
            queue.Enqueue(this);

            while (0 < queue.Count)
            {
                SimpleTreeNode<T> node = queue.Dequeue();

                foreach (SimpleTreeNode<T> child in node.Children)
                {
                    queue.Enqueue(child);
                }

                yield return node;
            }
        }
    }
}
