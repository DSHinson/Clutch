using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public class BPlusTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int Order { get; }

        public BPlusTreeNode<TKey> Root { get; private set; }

        public BPlusTree(int order)
        {
            if (order < 3)
            {
                throw new ArgumentException("Order must be >= 3");
            }

            Order = order;
            Root = new BPlusTreeLeaf<TKey, TValue>();
        }
    }

}
