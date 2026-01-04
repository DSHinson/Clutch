using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public sealed class BPlusTreeService<TKey, TValue> : IBPlusTreeService<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly int _order;
        private BPlusTreeNode<TKey> _root;

        public BPlusTreeService(int order)
        {
            if (order < 3)
            {
                throw new ArgumentException("Order must be >= 3");
            }

            _order = order;
            _root = new BPlusTreeLeaf<TKey, TValue>();
        }

        public void Insert(TKey key, TValue value)
        {
            var split = InsertInternal(_root, key, value);

            if (split != null)
            {
                // Root split
                var newRoot = new BPlusTreeInternal<TKey>();
                newRoot.Keys.Add(split.PromotedKey);
                newRoot.Children.Add(_root);
                newRoot.Children.Add(split.RightNode);
                _root = newRoot;
            }
        }

        public bool TryFind(TKey key, out TValue value)
        {
            var leaf = FindLeaf(_root, key);
            int index = leaf.Keys.BinarySearch(key);

            if (index >= 0)
            {
                value = leaf.Values[index];
                return true;
            }

            value = default!;
            return false;
        }

        public bool Delete(TKey key)
        {
            return DeleteInternal(_root, key);
        }
    }

}
