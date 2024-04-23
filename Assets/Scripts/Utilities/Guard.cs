using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.QuickMath
{
    public static class Guard
    {
        public static Against Against;
    }

    public interface Against { }

    public static class UnityObject
    {
        public static void UnityObjectNull(this Against against, UnityEngine.Object obj, string objName)
        {
            Debug.Assert(obj != null, $"Unity Object {objName} is null");
        }
    }
}
