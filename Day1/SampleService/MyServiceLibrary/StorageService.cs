using System;
using System.Collections.Generic;
using System.Linq;


namespace StorageServiceLibrary
{

    public class StorageService<T> where T : IUnique
    {
        #region Fields

        private IIdGenerator idGenerator;
        private ICollection<T> container;
        private IEqualityComparer<T> comparer;
        private IdComparer idComparer;

        #endregion

        #region Ctor

        public StorageService()
        {
            idGenerator = GetDefaultIdGenerator();
            container = new List<T>();
            idComparer = new IdComparer();

         
        }

        public StorageService(IIdGenerator idGenerator, ICollection<T> container, IEqualityComparer<T> comparer) : base()
        {
            if (idGenerator == null) throw new ArgumentNullException(nameof(idGenerator));
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            this.idGenerator = idGenerator;
            this.container = container;
            this.comparer = comparer;
        }

        #endregion

        #region Public methods

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (container.Contains(item, comparer)) throw new InvalidOperationException(nameof(item));

            while (item.Id == 0 || container.Contains(item, idComparer)) item.Id = idGenerator.GetId();

            container.Add(item);
        }

        public void Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!container.Remove(item)) throw new InvalidOperationException(nameof(item));
        }


        public void Remove(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (!container.Remove(container.First(item => item.Id == id))) throw new InvalidOperationException();
        }

        public T Get(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            return container.First(item => item.Id == id);
        }


        public IEnumerable<T> Search(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return container.Where(predicate).AsEnumerable();
        }


        public IEnumerable<T> AsEnumerable()
        {
            return container.AsEnumerable();
        }

        public IList<T> ToList()
        {
            return container.ToList();
        }

        public T[] ToArray()
        {
            return container.ToArray();
        }



        static public IList<T> GetDefaultContainer()
        {
            return new List<T>();
        }

        static public IIdGenerator GetDefaultIdGenerator()
        {
            return new IncrementIdGenerator();
        }

        static public IEqualityComparer<T> GetDefaultComparer()
        {
            return new IdComparer();
        }

        #endregion

        #region Nested clases

        private class IncrementIdGenerator : IIdGenerator
        {
            int counter = 0;

            public int GetId()
            {
                return counter++;
            }
        }

        private class IdComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                if (x.Id == y.Id) return true;
                return false;
            }

            public int GetHashCode(T obj)
            {
                return obj.Id; //!!!
            }
        }

        #endregion

    }
}
