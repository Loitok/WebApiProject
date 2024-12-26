using BLL.Models;

namespace BLL.HashTable
{
    public class HashUsage
    {
        public void Usage()
        {
            LocationModel model1 = new LocationModel { Id = 1, Address1 = "Address1", City = "City1" };
            LocationModel model2 = new LocationModel { Id = 2, Address1 = "Address2", City = "City2" };
            LocationModel model3 = new LocationModel { Id = 3, Address1 = "Address3", City = "City3" };

            HashTable hashTable = new HashTable();

            hashTable.Add(model1.Id, model1);
            hashTable.Add(model2.Id, model2);
            hashTable.Add(model3.Id, model3);

            var getModel1 = hashTable.Get(model1.Id);

            hashTable.Remove(model1.Id);
            hashTable.Remove(model2.Id);
            hashTable.Remove(model3.Id);
        }
    }

    public class HashTable
    {
        private const int Size = 100;
        private LinkedList<KeyValuePair<int, LocationModel>>[] buckets;

        public HashTable()
        {
            buckets = new LinkedList<KeyValuePair<int, LocationModel>>[Size];
        }

        private int GetHash(int key)
        {
            return key.GetHashCode() % Size;
        }

        public void Add(int key, LocationModel value)
        {
            var index = GetHash(key);

            if (buckets[index] == null)
            {
                buckets[index] = new LinkedList<KeyValuePair<int, LocationModel>>();
            }

            foreach (var pair in buckets[index])
            {
                if (pair.Key.Equals(key))
                {
                    throw new ArgumentException("Key already exists");
                }
            }

            buckets[index].AddLast(new KeyValuePair<int, LocationModel>(key, value));
        }

        public LocationModel Get(int key)
        {
            int index = GetHash(key);

            if (buckets[index] != null)
            {
                foreach (var pair in buckets[index])
                {
                    if (pair.Key.Equals(key))
                    {
                        return pair.Value;
                    }
                }
            }
            throw new KeyNotFoundException("Key not found");
        }

        public bool Remove(int key)
        {
            int index = GetHash(key);

            if (buckets[index] != null)
            {
                var current = buckets[index].First;
                while (current != null)
                {
                    if (current.Value.Key.Equals(key))
                    {
                        buckets[index].Remove(current);
                        return true;
                    }
                    current = current.Next;
                }
            }
            return false;
        }
    }
}
