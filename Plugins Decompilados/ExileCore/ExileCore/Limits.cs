// Decompiled with JetBrains decompiler
// Type: ExileCore.Limits
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExileCore
{
  internal static class Limits
  {
    public static readonly int ElementChildCount = 1000;
    public static readonly int UnicodeStringLength = 5120;
    public static readonly int ReadStructsArrayCount = 100000;
    public static readonly int ReadMemoryTimeLimit = 2000;

    static Limits()
    {
      try
      {
        string path = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config", "limits.json");
        if (!File.Exists(path))
          return;
        Limits.LimitsInstance limitsInstance = JsonConvert.DeserializeObject<Limits.LimitsInstance>(File.ReadAllText(path));
        int? elementChildCount = limitsInstance.ElementChildCount;
        if (elementChildCount.HasValue)
          Limits.ElementChildCount = elementChildCount.GetValueOrDefault();
        int? nullable = limitsInstance.UnicodeStringLength;
        if (nullable.HasValue)
          Limits.UnicodeStringLength = nullable.GetValueOrDefault();
        nullable = limitsInstance.ReadStructsArrayCount;
        if (nullable.HasValue)
          Limits.ReadStructsArrayCount = nullable.GetValueOrDefault();
        nullable = limitsInstance.ReadMemoryTimeLimit;
        if (!nullable.HasValue)
          return;
        Limits.ReadMemoryTimeLimit = nullable.GetValueOrDefault();
      }
      catch (Exception ex)
      {
        try
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
          interpolatedStringHandler.AppendLiteral("Unable to load the limits file: ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          string stringAndClear = interpolatedStringHandler.ToStringAndClear();
          Logger.Log.Error(stringAndClear);
          DebugWindow.LogError(stringAndClear);
        }
        catch
        {
        }
      }
    }

    private class LimitsInstance
    {
      public int? ElementChildCount;
      public int? UnicodeStringLength;
      public int? ReadStructsArrayCount;
      public int? ReadMemoryTimeLimit;
    }
  }
}
