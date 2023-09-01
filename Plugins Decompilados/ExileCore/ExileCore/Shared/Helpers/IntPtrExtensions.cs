// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.IntPtrExtensions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.Helpers
{
  public static class IntPtrExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr Add(this IntPtr left, IntPtr right) => new IntPtr((long) left + (long) right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr Divide(this IntPtr left, IntPtr right) => new IntPtr((long) left / (long) right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong GetValue(this IntPtr ptr) => (ulong) (long) ptr;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAligned(this IntPtr ptr)
    {
      ulong num = ptr.GetValue();
      return num == 1UL || num % 2UL == 0UL;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotZero(this IntPtr ptr) => ptr != IntPtr.Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid(this IntPtr ptr)
    {
      ulong num = (ulong) (long) ptr;
      return IntPtr.Size == 4 ? num > 65536UL && num < 4293918720UL : num > 65536UL && num < 4222124650659840UL;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this IntPtr ptr) => ptr == IntPtr.Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr Multiply(this IntPtr left, IntPtr right) => new IntPtr((long) left * (long) right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr Subtract(this IntPtr left, IntPtr right) => new IntPtr((long) left - (long) right);
  }
}
