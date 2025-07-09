using System.Collections.Generic;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

namespace PersonNameSorter.Strategies.Sort
{
    public class MergeSortStrategy : ISortStrategy
    {
        public List<PersonName> Sort(List<PersonName> names)
        {
            if (names.Count <= 1) return names;

            int mid = names.Count / 2;
            var left = Sort(names.GetRange(0, mid));
            var right = Sort(names.GetRange(mid, names.Count - mid));

            return Merge(left, right);
        }

        private List<PersonName> Merge(List<PersonName> left, List<PersonName> right)
        {
            List<PersonName> result = new();
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                var l = left[i];
                var r = right[j];

                int cmp = l.LastName.CompareTo(r.LastName);
                if (cmp == 0)
                    cmp = string.Join(" ", l.GivenNames).CompareTo(string.Join(" ", r.GivenNames));

                if (cmp <= 0)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }

            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));
            return result;
        }
    }
}