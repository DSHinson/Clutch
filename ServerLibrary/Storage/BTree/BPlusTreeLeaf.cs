using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public sealed class BPlusTreeLeaf<TKey, TValue> : BPlusTreeNode<TKey> where TKey : IComparable<TKey>
    {
        public override bool IsLeaf => true;

        public List<TValue> Values { get; } = new();

        public BPlusTreeLeaf<TKey, TValue>? Next { get; set; }
        public BPlusTreeLeaf<TKey, TValue>? Previous { get; set; }
    }

}
