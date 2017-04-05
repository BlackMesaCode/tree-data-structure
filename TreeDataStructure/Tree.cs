using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeDataStructure
{
    public class Tree<T>
    {

        public Tree(T data, Tree<T> parent = null)
        {
            Data = data;
            Parent = parent;

            Children = new LinkedList<Tree<T>>();
            ChildrenDataNodeMap = new Dictionary<T, Tree<T>>();
        }

        public virtual Tree<T> AddChild(T value)
        {
            var newChild = new Tree<T>(value, this);
            Children.Add(newChild);
            return newChild;
        }

        public virtual void RemoveChild(Tree<T> node)
        {
            Children.Remove(node);
        }

        public virtual void RemoveChild(T data)
        {
            var node = ChildrenDataNodeMap[data];
            Children.Remove(node);
        }

        public IDictionary<T, Tree<T>> ChildrenDataNodeMap { get; set; }


        public virtual Tree<T> SearchAncestors(Tree<T> node)
        {
            return GetAncestors().SingleOrDefault(ancestor => ancestor == node);
        }

        public virtual Tree<T> SearchAncestors(T data)
        {
            return GetAncestors().SingleOrDefault(ancestor => ancestor.Data.Equals(data));
        }

        public virtual IEnumerable<Tree<T>> GetAncestors()
        {
            var ancestor = Parent;
            while (ancestor != null)
            {
                yield return ancestor;
                ancestor = ancestor.Parent;
            }
        }

        
        public virtual IEnumerable<Tree<T>> GetDescendants(Tree<T> nodeToSearch = null)
        {
            HashSet<Tree<T>> visitedNodes = new HashSet<Tree<T>>();

            // populate queue
            var queue = new Queue<Tree<T>>();  // Breadth First Search
            queue.Enqueue(this);

            // traversing queue until no more new childs are enqueued
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                foreach (var child in node.Children)
                {
                    if (!visitedNodes.Contains(child))
                    {
                        yield return child;  // lazy implementation
                        queue.Enqueue(child);
                    }
                }
            }
            
        }


        private Tree<T> CompareWithNode(Tree<T> nodeToSearch, Tree<T> currentNode) => currentNode.Equals(nodeToSearch) ? currentNode : null;
        private Tree<T> CompareWithData(T dataToSearch, Tree<T> currentNode) => dataToSearch.Equals(currentNode.Data) ? currentNode : null;


        private Tree<T> SearchInDescendants<TSearchTarget>(
            Func<TSearchTarget, Tree<T>, Tree<T>> compareWithSearchTarget, 
            TSearchTarget searchTarget)
        {
            HashSet<Tree<T>> visitedNodes = new HashSet<Tree<T>>();

            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                var comparisonResult = compareWithSearchTarget(searchTarget, currentNode);

                if (comparisonResult != null)
                    return comparisonResult;

                foreach (var child in currentNode.Children)
                {
                    if (!visitedNodes.Contains(child))
                    {
                        queue.Enqueue(child);
                        visitedNodes.Add(child);
                    }

                }
            }

            return null;
        }


        public virtual Tree<T> SearchInDescendants(Tree<T> nodeToSearch = null) 
            => SearchInDescendants<Tree<T>>(CompareWithNode, nodeToSearch);


        public virtual Tree<T> SearchInDescendants(T data) 
            => SearchInDescendants<T>(CompareWithData, data);



        public virtual T Data { get; private set; }
        public virtual Tree<T> Parent { get; private set; }
        public virtual ICollection<Tree<T>> Children { get; set; }
        
        public virtual bool IsRoot => Parent == null;
        public virtual bool IsLeaf => !Children.Any();
        public virtual int Level => IsRoot ? 0 : Parent.Level + 1;
    }
}
