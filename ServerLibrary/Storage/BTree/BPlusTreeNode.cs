using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public abstract class BPlusTreeNode<TKey> where TKey : IComparable<TKey>
    {
        public List<TKey> Keys { get; } = new();

        public abstract bool IsLeaf { get; }
    }
}
