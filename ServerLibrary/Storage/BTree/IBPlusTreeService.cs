using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Storage.BTree
{
    public interface IBPlusTreeService<TKey, TValue> where TKey : IComparable<TKey>
    {
        void Insert(TKey key, TValue value);
        bool Delete(TKey key);
        bool TryFind(TKey key, out TValue value);
    }

}
