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
            ChildrenDataNodeMap = new Dictionary<T, dynamic>();
        }


        public virtual Tree<T> AddChild(T value)
        {
            var newChild = new Tree<T>(value, this);
            AddValueToChildrenDataNodeMap(value, newChild);
            Children.Add(newChild);
            return newChild;
        }


        //IEnumerable<T> SearchInChildren(T value)
        //{
        //    using the ChildrenDataNodeMap to boost children search
        //}


        private void AddValueToChildrenDataNodeMap(T value, Tree<T> node)
        {
            if (ChildrenDataNodeMap.ContainsKey(value))
            {
                if (ChildrenDataNodeMap[value].GetType() == typeof(ICollection<T>)) // in case we already created a bucket
                {
                    (ChildrenDataNodeMap[value] as ICollection<Tree<T>>).Add(node);
                }
                else // lets create a bucket in case of an hash collision
                {
                    Tree<T> temp = ChildrenDataNodeMap[value];
                    ChildrenDataNodeMap[value] = new LinkedList<Tree<T>>();
                    (ChildrenDataNodeMap[value] as LinkedList<Tree<T>>).Append(temp);
                }
            }
            ChildrenDataNodeMap[value] = node;
        }


        public virtual void RemoveChild(Tree<T> node)
        {
            Children.Remove(node);
            ChildrenDataNodeMap.Remove(node.Data);
        }


        public virtual void RemoveChild(T data)
        {
            var node = ChildrenDataNodeMap[data];
            RemoveChild(node);
        }


        public virtual Tree<T> SearchAncestors(Tree<T> node) 
            => GetAncestors().SingleOrDefault(ancestor => ancestor == node);


        public virtual Tree<T> SearchAncestors(T data) 
            => GetAncestors().SingleOrDefault(ancestor => ancestor.Data.Equals(data));


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


        public virtual Tree<T> SearchInDescendants(Tree<T> nodeToSearch = null)
            => SearchInDescendants<Tree<T>>(CompareWithNode, nodeToSearch);


        public virtual Tree<T> SearchInDescendants(T data)
            => SearchInDescendants<T>(CompareWithData, data);


        private Tree<T> CompareWithNode(Tree<T> nodeToSearch, Tree<T> currentNode) 
            => currentNode.Equals(nodeToSearch) ? currentNode : null;


        private Tree<T> CompareWithData(T dataToSearch, Tree<T> currentNode) 
            => dataToSearch.Equals(currentNode.Data) ? currentNode : null;


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


        public virtual T Data { get; private set; }
        public virtual Tree<T> Parent { get; private set; }
        public virtual ICollection<Tree<T>> Children { get; set; }
        public IDictionary<T, dynamic> ChildrenDataNodeMap { get; set; }

        public virtual bool IsRoot => Parent == null;
        public virtual bool IsLeaf => !Children.Any();
        public virtual int Level => IsRoot ? 0 : Parent.Level + 1;
    }
}
