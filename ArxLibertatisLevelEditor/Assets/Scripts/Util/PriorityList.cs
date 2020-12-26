using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Util
{
    public class PriorityList<T>
    {
        readonly Dictionary<int, List<T>> items = new Dictionary<int, List<T>>();
        readonly List<int> priorities = new List<int>();

        public void Add(T item, int priority)
        {
            if (!items.TryGetValue(priority, out List<T> lst))
            {
                lst = new List<T>();
                items[priority] = lst;
                priorities.Add(priority);
                priorities.Sort((a, b) => b.CompareTo(a)); //descending order list
            }

            lst.Add(item);
        }

        public bool Remove(T item, int priority)
        {
            if (items.TryGetValue(priority, out List<T> lst))
            {
                return lst.Remove(item);
            }
            return false;
        }

        public bool Remove(T item)
        {
            bool retval = false;
            foreach (var prio in priorities)
            {
                retval = Remove(item, prio) || retval; //written in this order cause second parameter in OR is not executed if first param is true
            }
            return retval;
        }

        public IEnumerable<int> GetPriorities()
        {
            return priorities;
        }

        public IEnumerable<T> GetPriorityItems(int priority)
        {
            if (items.TryGetValue(priority, out List<T> lst))
            {
                return lst;
            }
            return Enumerable.Empty<T>();
        }

        public void Clear()
        {
            priorities.Clear();
            items.Clear();
        }
    }
}
