// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.MiscHelpers
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExileCore.Shared.Helpers
{
  public static class MiscHelpers
  {
    public static string InsertBeforeUpperCase(this string str, string append)
    {
      StringBuilder stringBuilder = new StringBuilder();
      char ch = char.MinValue;
      foreach (char c in str ?? "")
      {
        if (char.IsUpper(c) && stringBuilder.Length != 0 && ch != ' ')
          stringBuilder.Append(append);
        stringBuilder.Append(c);
        ch = c;
      }
      return stringBuilder.ToString();
    }

    public static string GetTimeString(TimeSpan timeSpent)
    {
      int totalSeconds = (int) timeSpent.TotalSeconds;
      int num1 = totalSeconds % 60;
      int num2 = totalSeconds / 60;
      int num3 = num2 / 60;
      int num4 = num2 % 60;
      return string.Format(num3 > 0 ? "{0}:{1:00}:{2:00}" : "{1}:{2:00}", (object) num3, (object) num4, (object) num1);
    }

    public static string ToString(this NativeStringU str, IMemory mem) => ((NativeUtf16Text) str).ToString(mem);

    public static string ToString(this NativeUtf16Text str, IMemory mem) => str.ToString(mem, 256);

    public static unsafe string ToString(this NativeUtf16Text str, IMemory mem, int maxLength)
    {
      if (str.Length <= 0L)
        return string.Empty;
      if (str.LengthWithNullTerminator >= 8L)
        return mem.ReadStringU(str.Buffer, (int) (Math.Min((long) maxLength, str.Length) * 2L));
      if (str.Length > 8L)
        return "";
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(16)), 16);
      BitConverter.TryWriteBytes(destination, str.Buffer);
      ref Span<byte> local = ref destination;
      BitConverter.TryWriteBytes(local.Slice(8, local.Length - 8), str.Reserved8Bytes);
      return Encoding.Unicode.GetString((ReadOnlySpan<byte>) destination.Slice(0, (int) str.ByteLength));
    }

    public static unsafe string ToString(this NativeUtf8Text str, IMemory m)
    {
      if (str.Length <= 0)
        return string.Empty;
      if (str.LengthWithNullTerminator > 15)
        return m.ReadString(str.Buffer, Math.Min(str.Length, 256));
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(16)), 16);
      BitConverter.TryWriteBytes(destination, str.Buffer);
      ref Span<byte> local = ref destination;
      BitConverter.TryWriteBytes(local.Slice(8, local.Length - 8), str.Reserved8Bytes);
      return Encoding.UTF8.GetString((ReadOnlySpan<byte>) destination.Slice(0, str.Length));
    }

    public static string ToString(this PathEntityOffsets str, IMemory mem) => mem.ReadStringU(str.Path.Ptr, (int) str.Length * 2);

    public static T ToEnum<T>(this string value) => (T) Enum.Parse(typeof (T), value, true);

    public static System.Numerics.Vector2 ClickRandomNum(this SharpDX.RectangleF clientRect, int x = 3, int y = 3) => new System.Numerics.Vector2((float) Random.Shared.Next((int) clientRect.TopLeft.X + x, (int) clientRect.TopRight.X - x), (float) Random.Shared.Next((int) clientRect.TopLeft.Y + y, (int) clientRect.BottomLeft.Y - x));

    public static System.Numerics.Vector2 ClickRandom(this System.Drawing.RectangleF clientRect, int x = 3, int y = 3) => new System.Numerics.Vector2((float) Random.Shared.Next((int) clientRect.Left + x, (int) clientRect.Right - x), (float) Random.Shared.Next((int) clientRect.Top + y, (int) clientRect.Bottom - x));

    [Obsolete]
    public static SharpDX.Vector2 ClickRandom(this SharpDX.RectangleF clientRect, int x = 3, int y = 3) => new SharpDX.Vector2((float) Random.Shared.Next((int) clientRect.TopLeft.X + x, (int) clientRect.TopRight.X - x), (float) Random.Shared.Next((int) clientRect.TopLeft.Y + y, (int) clientRect.BottomLeft.Y - x));

    public static void PerfTimerLogMsg(Action act, string msg, float time = 3f, bool log = false)
    {
      using (new PerformanceTimer(msg, callback: ((Action<string, TimeSpan>) ((s, span) =>
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
        interpolatedStringHandler.AppendFormatted(s);
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<double>(span.TotalMilliseconds);
        interpolatedStringHandler.AppendLiteral(" ms.");
        DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), time, SharpDX.Color.Zero.GetRandomColor());
      })), log: false))
      {
        if (act == null)
          return;
        act();
      }
    }

    public static IEnumerable<string[]> LoadConfigBase(string path, int columnsCount = 2) => ((IEnumerable<string>) File.ReadAllLines(path)).Where<string>((Func<string, bool>) (line => !string.IsNullOrWhiteSpace(line) && line.IndexOf(';') >= 0 && !line.StartsWith("#"))).Select<string, string[]>((Func<string, string[]>) (line => ((IEnumerable<string>) line.Split(new char[1]
    {
      ';'
    }, columnsCount)).Select<string, string>((Func<string, string>) (parts => parts.Trim())).ToArray<string>()));
  }
}
