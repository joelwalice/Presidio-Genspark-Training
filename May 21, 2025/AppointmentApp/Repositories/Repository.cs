using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentApp.Exceptions;
using AppointmentApp.Interfaces;

namespace AppointmentApp.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected List<T> _items = new List<T>();

        public abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);
        public T Add(T item)
        {
            var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
            //if (_items.Contains(item))
            //{
            //    throw new DuplicateEntityException("Appointment already exists");
            //}
            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            _items.Remove(item);
            return item;
        }

        public T Update(T item)
        {
            //var myItem = GetById((K)item.GetType().GetProperty("Id").GetValue(item));
            var myItem = _items.FirstOrDefault(i => i.Equals(item));
            if (myItem != null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            int index = _items.IndexOf(item);
            _items[index] = item;
            return item;
        }
    }
}