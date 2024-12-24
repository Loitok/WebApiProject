using BLL.Models;

namespace BLL.HashTable
{
    public class LocationHashTable
    {
        private class HashNode
        {
            public int Key { get; set; }
            public LocationModel Value { get; set; }
            public HashNode Next { get; set; }

            public HashNode(int key, LocationModel value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        private readonly HashNode[] _buckets;
        private const int DefaultCapacity = 16;

        public LocationHashTable(int capacity = DefaultCapacity)
        {
            _buckets = new HashNode[capacity];
        }

        private int GetBucketIndex(int key)
        {
            return key.GetHashCode() % _buckets.Length;
        }

        public void Add(int key, LocationModel value)
        {
            int index = GetBucketIndex(key);
            HashNode node = _buckets[index];

            // Якщо в кошику вже є елементи, додаємо в ланцюжок
            while (node != null)
            {
                if (node.Key == key)
                    throw new ArgumentException($"Key {key} already exists.");
                node = node.Next;
            }

            // Додаємо новий елемент
            HashNode newNode = new HashNode(key, value)
            {
                Next = _buckets[index]
            };
            _buckets[index] = newNode;
        }

        public LocationModel Get(int key)
        {
            int index = GetBucketIndex(key);
            HashNode node = _buckets[index];

            while (node != null)
            {
                if (node.Key == key)
                    return node.Value;
                node = node.Next;
            }

            throw new KeyNotFoundException($"Key {key} not found.");
        }

        public bool Remove(int key)
        {
            int index = GetBucketIndex(key);
            HashNode node = _buckets[index];
            HashNode prev = null;

            while (node != null)
            {
                if (node.Key == key)
                {
                    if (prev == null)
                    {
                        _buckets[index] = node.Next; // Видаляємо перший елемент
                    }
                    else
                    {
                        prev.Next = node.Next; // Обходимо видалений вузол
                    }
                    return true;
                }
                prev = node;
                node = node.Next;
            }

            return false;
        }

        public bool ContainsKey(int key)
        {
            int index = GetBucketIndex(key);
            HashNode node = _buckets[index];

            while (node != null)
            {
                if (node.Key == key)
                    return true;
                node = node.Next;
            }

            return false;
        }
    }
}
