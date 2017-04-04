using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeDataStructure
{
    public class Tree<T> where T: IComparable<T>
    {

        public Tree(T data, Tree<T> parent = null)
        {
            Data = data;
            Parent = parent;

            Children = new LinkedList<Tree<T>>();
            ChildrenDataNodeMap = new Dictionary<T, Tree<T>>();
            
        }

        public Tree<T> AddChild(T value)
        {
            var newChild = new Tree<T>(value, this);
            Children.Add(newChild);
            return newChild;
        }

        public void RemoveChild(Tree<T> node)
        {
            Children.Remove(node);
        }

        public void RemoveChild(T data)
        {
            var node = ChildrenDataNodeMap[data];
            Children.Remove(node);
        }

        public IDictionary<T, Tree<T>> ChildrenDataNodeMap { get; set; }


        public Tree<T> SearchAncestors(Tree<T> node)
        {
            return GetAncestors().SingleOrDefault(ancestor => ancestor == node);
        }

        public Tree<T> SearchAncestors(T data)
        {
            return GetAncestors().SingleOrDefault(ancestor => ancestor.Data.Equals(data));
        }

        public IEnumerable<Tree<T>> GetAncestors()
        {
            var ancestor = Parent;
            while (ancestor != null)
            {
                yield return ancestor;
                ancestor = ancestor.Parent;
            }
        }

        // Breadth First Search
        public IEnumerable<Tree<T>> GetDescendants(Tree<T> nodeToSearch = null)
        {
            HashSet<Tree<T>> visitedNodes = new HashSet<Tree<T>>();

            // populate queue
            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            // traversing queue until no more new childs are enqueued
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                foreach (var child in node.Children)
                {
                    if (!visitedNodes.Contains(child))
                    {
                        yield return child;
                        queue.Enqueue(child);
                    }
                }
            }
            
        }

        public Tree<T> SearchDescendants(Tree<T> nodeToSearch = null)
        {
            HashSet<Tree<T>> visitedNodes = new HashSet<Tree<T>>();

            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node == nodeToSearch)
                    return node;

                foreach (var child in node.Children)
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


        public Tree<T> SearchDescendants(T data)
        {
            HashSet<Tree<T>> visitedNodes = new HashSet<Tree<T>>();

            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                // we cant use == operator, as they are not implemented for generic type comparisons
                if (node.Data.Equals(data)) 
                    return node;

                foreach (var child in node.Children)
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



        //public IEnumerable<Tree<T>> GetAncestors()
        //{

        //}

        //public IEnumerable<Tree<T>> GetDescendants()
        //{

        //}

        //public bool Compare<T>(T x, T y)
        //{
        //    return EqualityComparer<T>.Default.Equals(x, y);
        //}


        public T Data { get; private set; }
        public Tree<T> Parent { get; private set; }
        public ICollection<Tree<T>> Children { get; set; }
        
        public bool IsRoot => Parent == null;
        public bool IsLeaf => !Children.Any();
        public int Level => IsRoot ? 0 : Parent.Level + 1;
    }
}
