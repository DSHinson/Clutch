using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public sealed class BPlusTreeInternal<TKey> : BPlusTreeNode<TKey> where TKey : IComparable<TKey>
    {
        public override bool IsLeaf => false;

        // Children count = Keys.Count + 1
        public List<BPlusTreeNode<TKey>> Children { get; } = new();
    }

}
