// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.DictionaryExtensions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;
using System.Linq;

namespace ExileCore.Shared.Helpers
{
  public static class DictionaryExtensions
  {
    public static T MergeLeft<T, TK, TV>(this T me, params IDictionary<TK, TV>[] others) where T : IDictionary<TK, TV>, new()
    {
      T obj1 = new T();
      foreach (IEnumerable<KeyValuePair<TK, TV>> keyValuePairs in new List<IDictionary<TK, TV>>()
      {
        (IDictionary<TK, TV>) me
      }.Concat<IDictionary<TK, TV>>((IEnumerable<IDictionary<TK, TV>>) others))
      {
        foreach (KeyValuePair<TK, TV> keyValuePair in keyValuePairs)
        {
          ref T local = ref obj1;
          if ((object) default (T) == null)
          {
            T obj2 = local;
            local = ref obj2;
          }
          TK key = keyValuePair.Key;
          TV v = keyValuePair.Value;
          local[key] = v;
        }
      }
      return obj1;
    }
  }
}
