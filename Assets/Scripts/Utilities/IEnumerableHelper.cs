using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HotPlay.Utilities
{
    public static class IEnumerableHelper 
    {
        public static T RandomPick<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        public static T RandomEnumValue<T>(this T @enum, int min)
        {
            var values = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(min, values.Length);
            return (T)values.GetValue(random);
        }

        public static bool Random(this bool boolean)
        {
            uint boolBits = 0;
            System.Random random = new System.Random();
            boolBits >>= 1;
            boolBits = (uint)~random.Next();
            return (boolBits & 1) == 0;
        }
        
        public static T RandomPick<T>(this IEnumerable<T> enumerable, int count)
        {
            var random = UnityEngine.Random.Range(0, count);
            using var enumerator = enumerable.GetEnumerator();

            for (int i = 0; i < random; i++)
            {
                enumerator.MoveNext();
            }
            
            return enumerator.Current;
        }

        public static T RandomPick<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RandomPick<T>(this List<T> list, out int index)
        {
            index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static bool TryGetAtIndex<T>(this T[] array, int index, out T obj)
        {
            if (index < 0 || index >= array.Length)
            {
                obj = default(T);
                return false;
            }

            obj = array[index];
            return true;
        }

        public static T GetAtIndexOrDefault<T>(this T[] array, int index)
        {
            array.TryGetAtIndex(index, out var obj);
            return obj;
        }
        
        public static string ToListString<T>(this IEnumerable<T> list)
        {
            return list.Aggregate("Count " + list.Count(), (a, x) => a += " | " + x.ToString());
        }
    }
}
