// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.ActionExtensions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Helpers
{
  public static class ActionExtensions
  {
    public static void SafeTryInvoke(this Action action)
    {
      if (action == null)
        return;
      try
      {
        action();
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public static void SafeTryInvoke<T>(this Action<T> action, T arg)
    {
      if (action == null)
        return;
      try
      {
        action(arg);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public static void SafeTryInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
      if (action == null)
        return;
      try
      {
        action(arg1, arg2);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public static void SafeTryInvoke<T1, T2, T3>(
      this Action<T1, T2, T3> action,
      T1 arg1,
      T2 arg2,
      T3 arg3)
    {
      if (action == null)
        return;
      try
      {
        action(arg1, arg2, arg3);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }
  }
}
