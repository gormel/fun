using System;
using System.Collections.Generic;
using System.Linq;

namespace FunMath.MultiTree
{
    public class MultiTree<T> where T : IMultiTreeElement
    {
        private readonly int mDimentions;
        private Node mRoot = null;

        class Node
        {
            private readonly int mDimentions;
            public List<T> Elements { get; } = new List<T>();
            public Node[] Children { get; private set; } = null;
            public MultiBox NodeBox { get; set; }

            public Node(int dimentions)
            {
                mDimentions = dimentions;
            }

            public void Split()
            {
                Children = new Node[1 << mDimentions];
                DeepEnumerate(new int[mDimentions], 0, coords =>
                {
                    var child = new Node(mDimentions);
                    child.NodeBox = new MultiBox(new MultiVector(coords.Select((c, i) => NodeBox.Position[i] + c * NodeBox.Size[i] / 2).ToArray()), NodeBox.Size / 2);
                    var childIndex = coords.Select((c, i) => c * (1 << i)).Sum();
                    Children[childIndex] = child;
                });
            }

            public Node CreateParent()
            {
                var parent = new Node(mDimentions);
                parent.NodeBox = new MultiBox(NodeBox.Position, NodeBox.Size * 2);
                parent.Split();
                parent.Children[0] = this;
                return parent;
            }

            private static void DeepEnumerate(int[] coords, int level, Action<int[]> body)
            {
                if (level >= coords.Length)
                {
                    body(coords);
                    return;
                }
                coords[level] = 0;
                DeepEnumerate(coords, level + 1, body);
                coords[level] = 1;
                DeepEnumerate(coords, level + 1, body);
            }

            public void Add(T value)
            {
                if (Elements.Count < 10)
                {
                    Elements.Add(value);
                    return;
                }

                if (Children == null)
                    Split();

                foreach (var child in Children)
                {
                    if (value.Box.Intersection(child.NodeBox) != null)
                        child.Add(value);
                } 
            }

            public void Query(List<T> result, MultiBox box)
            {
                foreach (var element in Elements)
                {
                    if (box.Intersection(element.Box) != null)
                        result.Add(element);
                }

                if (Children == null)
                    return;

                foreach (var child in Children)
                {
                    if (box.Intersection(child.NodeBox) != null)
                        child.Query(result, box);
                }
            }

            private static bool Intersects(MultiVector pos, double radius, MultiBox box)
            {
                bool topInSphere = false;
                DeepEnumerate(new int[pos.Dimentions], 0, coords =>
                {
                    var top = box.Position + new MultiVector(coords.Select((c, i) => box.Size[i] * c).ToArray(), false);
                    topInSphere = topInSphere || (top - pos).LenghtSq < radius * radius;
                });
                return box.Contains(pos) || topInSphere;
            }
        }

        public MultiTree(int dimentions)
        {
            mDimentions = dimentions;
        }

        public void Add(T value)
        {
            if (mRoot == null)
            {
                mRoot = new Node(mDimentions);
                mRoot.NodeBox = value.Box;
                mRoot.Elements.Add(value);
                return;
            }

            while (value.Box.Intersection(mRoot.NodeBox) == null)
                mRoot = mRoot.CreateParent();
            mRoot.Add(value);
        }

        public List<T> Query(MultiBox box)
        {
            var result = new List<T>();
            if (mRoot == null)
                return result;

            mRoot.Query(result, box);
            return result;
        }
    }
}
