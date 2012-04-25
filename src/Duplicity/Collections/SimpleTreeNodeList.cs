// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System.Collections.Generic;

namespace Duplicity.Collections
{
    /// <summary>
    /// Contains a list of SimpleTreeNode (or SimpleTreeNode-derived) objects, with the capability of linking parents and children in both directions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleTreeNodeList<T> : List<SimpleTreeNode<T>>
    {
        public SimpleTreeNode<T> Parent;

        public SimpleTreeNodeList(SimpleTreeNode<T> parent)
        {
            Parent = parent;
        }

        public new SimpleTreeNode<T> Add(SimpleTreeNode<T> node)
        {
            base.Add(node);
            node.Parent = Parent;
            return node;
        }

        public SimpleTreeNode<T> Add(T value)
        {
            return new SimpleTreeNode<T>(Parent) { Value = value };
        }
    }
}