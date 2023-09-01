// Decompiled with JetBrains decompiler
// Type: ExileCore.Memory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets.Native;
using ProcessMemoryUtilities.Managed;
using ProcessMemoryUtilities.Native;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore
{
  public class Memory : IMemory, IDisposable
  {
    private bool closed;
    private readonly Stopwatch sw = Stopwatch.StartNew();
    private IMemoryBackend _backend;

    public Memory((Process, Offsets) tuple)
    {
      this.Process = tuple.Item1;
      this.AddressOfProcess = this.Process.MainModule.BaseAddress.ToInt64();
      this.MainWindowHandle = this.Process.MainWindowHandle;
      this.OpenProcessHandle = NativeWrapper.OpenProcess(ProcessAccessFlags.Read, this.Process.Id);
      this._backend = (IMemoryBackend) new DefaultMemoryBackend(this.OpenProcessHandle);
      this.BaseOffsets = tuple.Item2.DoPatternScans((IMemory) this);
    }

    public MemoryBackendMode BackendMode
    {
      get
      {
        IMemoryBackend backend = this._backend;
        MemoryBackendMode backendMode;
        switch (backend)
        {
          case DefaultMemoryBackend _:
            backendMode = MemoryBackendMode.AlwaysRead;
            break;
          case PagedMemoryBackend _:
            backendMode = MemoryBackendMode.CacheAndPreload;
            break;
          default:
            \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) backend);
            break;
        }
        return backendMode;
      }
      set
      {
        if (this.BackendMode == value)
          return;
        IMemoryBackend memoryBackend1;
        switch (value)
        {
          case MemoryBackendMode.AlwaysRead:
            memoryBackend1 = (IMemoryBackend) new DefaultMemoryBackend(this.OpenProcessHandle);
            break;
          case MemoryBackendMode.CacheAndPreload:
            memoryBackend1 = (IMemoryBackend) new PagedMemoryBackend((IMemoryBackend) new DefaultMemoryBackend(this.OpenProcessHandle));
            break;
          default:
            \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) value);
            break;
        }
        IMemoryBackend memoryBackend2 = memoryBackend1;
        IMemoryBackend backend = this._backend;
        this._backend = memoryBackend2;
        backend?.Dispose();
      }
    }

    public IntPtr MainWindowHandle { get; }

    public IntPtr OpenProcessHandle { get; }

    public long AddressOfProcess { get; }

    public Dictionary<OffsetsName, long> BaseOffsets { get; }

    public Process Process { get; }

    public void NotifyFrame() => this._backend.NotifyFrame();

    public unsafe string ReadString(long addr, int length = 256, bool replaceNull = true)
    {
      if (addr <= 65536L && addr >= -1L)
        return string.Empty;
      if (length <= 0 || length > 1000000)
        return "TextTooLong";
      Span<byte> span1;
      if (length < 1024)
      {
        int length1 = length;
        // ISSUE: untyped stack allocation
        span1 = new Span<byte>((void*) __untypedstackalloc((IntPtr) (uint) length1), length1);
      }
      else
        span1 = (Span<byte>) new byte[length];
      Span<byte> span2 = span1;
      if (!this.ReadMem<byte>(new IntPtr(addr), span2))
        return string.Empty;
      string text = Encoding.UTF8.GetString((ReadOnlySpan<byte>) span2);
      return !replaceNull ? text : Memory.RTrimNull(text);
    }

    public string ReadNativeString(long address)
    {
      uint num = this.Read<uint>(address + 16L);
      if (num == 0U)
        return string.Empty;
      return 8U <= num ? this.ReadStringU(this.Read<long>(address), 256, true) : this.ReadStringU(address, 256, true);
    }

    public unsafe string ReadStringU(long addr, int lengthBytes = 256, bool replaceNull = true)
    {
      if (lengthBytes > Limits.UnicodeStringLength || lengthBytes < 0 || addr == 0L)
        return string.Empty;
      Span<byte> span1;
      if (lengthBytes < 1024)
      {
        int length = lengthBytes;
        // ISSUE: untyped stack allocation
        span1 = new Span<byte>((void*) __untypedstackalloc((IntPtr) (uint) length), length);
      }
      else
        span1 = (Span<byte>) new byte[lengthBytes];
      Span<byte> span2 = span1;
      if (!this.ReadMem<byte>(new IntPtr(addr), span2) || span2[0] == (byte) 0 && span2[1] == (byte) 0)
        return string.Empty;
      string text = Encoding.Unicode.GetString((ReadOnlySpan<byte>) span2);
      return !replaceNull ? text : Memory.RTrimNull(text);
    }

    public byte[] ReadMem(long addr, int size) => this.ReadMem(new IntPtr(addr), size);

    public byte[] ReadMem(IntPtr address, int size) => this.ReadMem<byte>(address, size);

    public T[] ReadMem<T>(long addr, int size) where T : unmanaged => this.ReadMem<T>(new IntPtr(addr), size);

    public T[] ReadMem<T>(IntPtr address, int size) where T : unmanaged
    {
      try
      {
        if (size <= 0 || address.ToInt64() <= 0L || size >= int.MaxValue / Unsafe.SizeOf<T>())
          return Array.Empty<T>();
        T[] objArray = new T[size];
        this._backend.TryReadMemory(address, MemoryMarshal.Cast<T, byte>((Span<T>) objArray));
        return objArray;
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
        interpolatedStringHandler.AppendLiteral("Readmem-> A: ");
        interpolatedStringHandler.AppendFormatted<IntPtr>(address);
        interpolatedStringHandler.AppendLiteral(" Size: ");
        interpolatedStringHandler.AppendFormatted<int>(size);
        interpolatedStringHandler.AppendLiteral(". ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        throw;
      }
    }

    public bool ReadMem<T>(IntPtr address, Span<T> target) where T : unmanaged
    {
      try
      {
        return target.Length > 0 && address.ToInt64() > 0L && target.Length < int.MaxValue / Unsafe.SizeOf<T>() && this._backend.TryReadMemory(address, MemoryMarshal.Cast<T, byte>(target));
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
        interpolatedStringHandler.AppendLiteral("Readmem-> A: ");
        interpolatedStringHandler.AppendFormatted<IntPtr>(address);
        interpolatedStringHandler.AppendLiteral(" Size: ");
        interpolatedStringHandler.AppendFormatted<int>(target.Length);
        interpolatedStringHandler.AppendLiteral(". ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        throw;
      }
    }

    public byte[] ReadBytes(long addr, int size) => this.ReadMem(addr, size);

    public byte[] ReadBytes(long addr, long size) => this.ReadMem(addr, (int) size);

    public List<T> ReadStructsArray<T>(
      long startAddress,
      long endAddress,
      int structSize,
      RemoteMemoryObject game)
      where T : RemoteMemoryObject, new()
    {
      List<T> objList = new List<T>();
      long num = (endAddress - startAddress) / (long) structSize;
      if (num < 0L || num > (long) Limits.ReadStructsArrayCount)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 5);
        interpolatedStringHandler.AppendLiteral("Maybe overflow memory in ");
        interpolatedStringHandler.AppendFormatted(nameof (ReadStructsArray));
        interpolatedStringHandler.AppendLiteral(" for reading structures of type: ");
        interpolatedStringHandler.AppendFormatted(typeof (T).Name);
        interpolatedStringHandler.AppendLiteral(", start is ");
        interpolatedStringHandler.AppendFormatted<long>(startAddress);
        interpolatedStringHandler.AppendLiteral(", end is ");
        interpolatedStringHandler.AppendFormatted<long>(endAddress);
        interpolatedStringHandler.AppendLiteral(", size is ");
        interpolatedStringHandler.AppendFormatted<int>(structSize);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 3f);
        return objList;
      }
      for (long address = startAddress; address < endAddress; address += (long) structSize)
        objList.Add(RemoteMemoryObject.GetObjectStatic<T>(address));
      return objList;
    }

    public List<T> ReadStructsArray<T>(long startAddress, long endAddress, int structSize) where T : unmanaged
    {
      List<T> objList = new List<T>();
      int num1 = 0;
      long num2 = (endAddress - startAddress) / (long) structSize;
      if (num2 < 0L || num2 > (long) Limits.ReadStructsArrayCount)
      {
        DebugWindow.LogError("Maybe overflow memory in ReadStructsArray for reading structures of type: " + typeof (T).Name, 3f);
        return objList;
      }
      for (long addr = startAddress; addr < endAddress; addr += (long) structSize)
      {
        objList.Add(this.Read<T>(addr));
        ++num1;
        if (num1 > Limits.ReadStructsArrayCount)
        {
          DebugWindow.LogError("Maybe overflow memory in ReadStructsArray for reading structures of type: " + typeof (T).Name, 3f);
          return objList;
        }
      }
      return objList;
    }

    public IList<T> ReadDoublePtrVectorClasses<T>(
      long address,
      RemoteMemoryObject game,
      bool noNullPointers = false)
      where T : RemoteMemoryObject, new()
    {
      long num = this.Read<long>(address);
      int size = (int) (this.Read<long>(address + 16L) - num);
      byte[] numArray = this.ReadMem(new IntPtr(num), size);
      List<T> objList = new List<T>();
      Stopwatch stopwatch = Stopwatch.StartNew();
      for (int startIndex = 0; startIndex < size; startIndex += 16)
      {
        if (stopwatch.ElapsedMilliseconds > (long) Limits.ReadMemoryTimeLimit)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
          interpolatedStringHandler.AppendLiteral("ReadDoublePtrVectorClasses error result count: ");
          interpolatedStringHandler.AppendFormatted<int>(objList.Count);
          DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
          return (IList<T>) new List<T>();
        }
        long int64 = BitConverter.ToInt64(numArray, startIndex);
        if (!(int64 == 0L & noNullPointers))
          objList.Add(game.GetObject<T>(int64));
      }
      return (IList<T>) objList;
    }

    public IList<long> ReadPointersArray(long startAddress, long endAddress, int offset = 8)
    {
      List<long> longList1 = new List<long>();
      long size = endAddress - startAddress;
      if (endAddress <= 0L || startAddress <= 0L || size <= 0L || size > 160000L || size % 8L != 0L)
        return (IList<long>) longList1;
      this.sw.Restart();
      List<long> longList2 = new List<long>((int) (size / (long) offset) + 1);
      byte[] numArray = this.ReadMem(startAddress, (int) size);
      for (int startIndex = 0; (long) startIndex < size; startIndex += offset)
      {
        if (this.sw.ElapsedMilliseconds > (long) Limits.ReadMemoryTimeLimit)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
          interpolatedStringHandler.AppendLiteral("ReadPointersArray error result count: ");
          interpolatedStringHandler.AppendFormatted<int>(longList2.Count);
          DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
          return (IList<long>) new List<long>();
        }
        longList2.Add(BitConverter.ToInt64(numArray, startIndex));
      }
      return (IList<long>) longList2;
    }

    public IList<long> ReadSecondPointerArray_Count(long startAddress, int count) => throw new NotImplementedException();

    public T Read<T>(Pointer addr, params int[] offsets) where T : unmanaged => throw new NotImplementedException();

    public T Read<T>(IntPtr addr, params int[] offsets) where T : unmanaged
    {
      if (addr == IntPtr.Zero)
        return default (T);
      long num1 = this.Read<long>(addr);
      for (int index = 0; index < offsets.Length - 1; ++index)
      {
        if (num1 == 0L)
          return default (T);
        int offset = offsets[index];
        num1 = this.Read<long>(num1 + (long) offset);
      }
      if (num1 == 0L)
        return default (T);
      long num2 = num1;
      int[] numArray = offsets;
      long num3 = (long) numArray[numArray.Length - 1];
      return this.Read<T>(num2 + num3);
    }

    public T Read<T>(long addr, params int[] offsets) where T : unmanaged => this.Read<T>(new IntPtr(addr), offsets);

    public T Read<T>(Pointer addr) where T : unmanaged => throw new NotImplementedException();

    public unsafe T Read<T>(IntPtr addr) where T : unmanaged
    {
      if (addr == IntPtr.Zero)
        return default (T);
      // ISSUE: untyped stack allocation
      Span<T> span = new Span<T>((void*) __untypedstackalloc(checked (new IntPtr(1) * sizeof (T))), 1);
      this._backend.TryReadMemory(addr, MemoryMarshal.Cast<T, byte>(span));
      return span[0];
    }

    public T Read<T>(long addr) where T : unmanaged => this.Read<T>(new IntPtr(addr));

    public IList<Tuple<long, int>> ReadDoublePointerIntList(long address)
    {
      List<Tuple<long, int>> tupleList = new List<Tuple<long, int>>();
      long addr = this.Read<long>(address);
      NativeListNode nativeListNode = this.Read<NativeListNode>(addr);
      tupleList.Add(new Tuple<long, int>(nativeListNode.Ptr2_Key, nativeListNode.Value));
      Stopwatch stopwatch = Stopwatch.StartNew();
      while (addr != nativeListNode.Next)
      {
        if (stopwatch.ElapsedMilliseconds > (long) Limits.ReadMemoryTimeLimit)
        {
          ILogger logger = Core.Logger;
          if (logger != null)
          {
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
            interpolatedStringHandler.AppendLiteral("ReadDoublePointerIntList error result count: ");
            interpolatedStringHandler.AppendFormatted<int>(tupleList.Count);
            logger.Error(interpolatedStringHandler.ToStringAndClear());
          }
          return (IList<Tuple<long, int>>) new List<Tuple<long, int>>();
        }
        nativeListNode = this.Read<NativeListNode>(nativeListNode.Next);
        tupleList.Add(new Tuple<long, int>(nativeListNode.Ptr2_Key, nativeListNode.Value));
      }
      if (tupleList.Count > 0)
        tupleList.RemoveAt(tupleList.Count - 1);
      return (IList<Tuple<long, int>>) tupleList;
    }

    public IList<T> ReadList<T>(IntPtr head) where T : unmanaged
    {
      List<T> objList = new List<T>();
      NativeListNode nativeListNode = this.Read<NativeListNode>(head);
      Stopwatch stopwatch = Stopwatch.StartNew();
      for (long int64 = head.ToInt64(); int64 != nativeListNode.Next; nativeListNode = this.Read<NativeListNode>(nativeListNode.Next))
      {
        if (stopwatch.ElapsedMilliseconds > (long) Limits.ReadMemoryTimeLimit)
        {
          ILogger logger = Core.Logger;
          if (logger != null)
          {
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
            interpolatedStringHandler.AppendLiteral("Readlist error result count: ");
            interpolatedStringHandler.AppendFormatted<int>(objList.Count);
            logger.Error(interpolatedStringHandler.ToStringAndClear());
          }
          return (IList<T>) new List<T>();
        }
        objList.Add(this.Read<T>(nativeListNode.Next));
      }
      return (IList<T>) objList;
    }

    public IList<T> ReadStdList<T>(IntPtr head) where T : unmanaged
    {
      List<T> objList = new List<T>();
      StdListNode<T> stdListNode;
      for (IntPtr next = this.Read<StdListNode>(head).Next; next != head; next = stdListNode.Next)
      {
        stdListNode = this.Read<StdListNode<T>>(next);
        if (next == IntPtr.Zero)
        {
          ILogger logger = Core.Logger;
          if (logger != null)
          {
            logger.Error("Terminating reading of list next nodes because of unexpected 0x00 found. This is normal if it happens after closing the game, otherwise report it.");
            break;
          }
          break;
        }
        objList.Add(stdListNode.Data);
      }
      return (IList<T>) objList;
    }

    public T[] ReadRMOStdVector<T>(StdVector nativeContainer, int objectSize) where T : RemoteMemoryObject, new()
    {
      long num = nativeContainer.Last - nativeContainer.First;
      long count = num / (long) objectSize;
      return num <= 0L || num % (long) objectSize != 0L || count > (long) Array.MaxLength ? Array.Empty<T>() : Enumerable.Range(0, (int) count).Select<int, T>((Func<int, T>) (x => RemoteMemoryObject.GetObjectStatic<T>(nativeContainer.First + (long) (x * objectSize)))).ToArray<T>();
    }

    public T[] ReadStdVector<T>(StdVector nativeContainer) where T : unmanaged
    {
      int num1 = Unsafe.SizeOf<T>();
      long num2 = nativeContainer.Last - nativeContainer.First;
      long size = num2 / (long) num1;
      return num2 <= 0L || num2 % (long) num1 != 0L || size > (long) Array.MaxLength ? Array.Empty<T>() : this.ReadMem<T>(nativeContainer.First, (int) size);
    }

    public T[] ReadStdVector<T>(NativePtrArray nativeContainer) where T : unmanaged
    {
      int elementSize = Unsafe.SizeOf<T>();
      long num = nativeContainer.Last - nativeContainer.First;
      long size = nativeContainer.ElementCount(elementSize);
      return num <= 0L || num % (long) elementSize != 0L || size > (long) Array.MaxLength ? Array.Empty<T>() : this.ReadMem<T>(nativeContainer.First, (int) size);
    }

    public T[] ReadStdVectorStride<T>(NativePtrArray nativeContainer, int stride) where T : unmanaged
    {
      int num1 = Unsafe.SizeOf<T>();
      if (stride < num1)
        return Array.Empty<T>();
      long num2 = nativeContainer.Last - nativeContainer.First;
      long length = nativeContainer.ElementCount(stride);
      if (num2 <= 0L || num2 % (long) stride != 0L || length > (long) Array.MaxLength)
        return Array.Empty<T>();
      byte[] numArray = this.ReadBytes(nativeContainer.First, length * (long) stride);
      T[] objArray = new T[length];
      for (int index = 0; (long) index < length; ++index)
      {
        ref byte local = ref numArray[stride * index];
        objArray[index] = Unsafe.As<byte, T>(ref local);
      }
      return objArray;
    }

    public T ReadStdVectorElement<T>(StdVector nativeContainer, int index) where T : unmanaged
    {
      int num1 = Unsafe.SizeOf<T>();
      int num2 = (int) (nativeContainer.Last - nativeContainer.First) / num1;
      return index < 0 && index >= num2 ? default (T) : this.Read<T>(nativeContainer.First + (long) (num1 * index));
    }

    public T ReadStdVectorElement<T>(NativePtrArray nativeContainer, int index) where T : unmanaged
    {
      int elementSize = Unsafe.SizeOf<T>();
      long num = nativeContainer.ElementCount(elementSize);
      return index < 0 && (long) index >= num ? default (T) : this.Read<T>(nativeContainer.First + (long) (elementSize * index));
    }

    public IList<long> ReadListPointer(IntPtr head)
    {
      List<long> longList = new List<long>();
      NativeListNode nativeListNode = this.Read<NativeListNode>(head);
      Stopwatch stopwatch = Stopwatch.StartNew();
      for (long int64 = head.ToInt64(); int64 != nativeListNode.Next; nativeListNode = this.Read<NativeListNode>(nativeListNode.Next))
      {
        if (stopwatch.ElapsedMilliseconds > (long) Limits.ReadMemoryTimeLimit)
        {
          ILogger logger = Core.Logger;
          if (logger != null)
          {
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
            interpolatedStringHandler.AppendLiteral("ReadListPointer error result count: ");
            interpolatedStringHandler.AppendFormatted<int>(longList.Count);
            logger.Error(interpolatedStringHandler.ToStringAndClear());
          }
          return (IList<long>) new List<long>();
        }
        longList.Add(nativeListNode.Next);
      }
      return (IList<long>) longList;
    }

    public long[] FindPatterns(params IPattern[] patterns)
    {
      byte[] exeImage = this.ReadMem(new IntPtr(this.AddressOfProcess), this.Process.MainModule.ModuleMemorySize);
      long[] address = new long[patterns.Length];
      Parallel.For(0L, (long) patterns.Length, new Action<long>(FindPattern));
      exeImage = (byte[]) null;
      return address;

      static bool CompareData(IPattern pattern, Span<byte> data)
      {
        byte[] bytes = pattern.Bytes;
        string mask = pattern.Mask;
        if ((int) bytes[0] == (int) data[0])
        {
          byte[] numArray = bytes;
          if ((int) numArray[numArray.Length - 1] == (int) data[bytes.Length - 1])
          {
            for (int index = 0; index < bytes.Length; ++index)
            {
              if (mask[index] == 'x' && (int) bytes[index] != (int) data[index])
                return false;
            }
            return true;
          }
        }
        return false;
      }

      void FindPattern(long iPattern)
      {
        IPattern pattern = patterns[iPattern];
        int length = pattern.Bytes.Length;
        bool flag = false;
        using (new PerformanceTimer("Pattern: " + pattern.Name + " -> ", callback: ((Action<string, TimeSpan>) ((s, span) =>
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 4);
          interpolatedStringHandler.AppendFormatted(s);
          interpolatedStringHandler.AppendLiteral(": Time: ");
          interpolatedStringHandler.AppendFormatted<double>(span.TotalMilliseconds);
          interpolatedStringHandler.AppendLiteral(" ms. Offset:[");
          interpolatedStringHandler.AppendFormatted<long>(address[iPattern]);
          interpolatedStringHandler.AppendLiteral("] Started searching offset with:");
          interpolatedStringHandler.AppendFormatted<int>(pattern.StartOffset);
          DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear());
        })), log: false))
        {
          for (int startOffset = pattern.StartOffset; startOffset < exeImage.Length - length; ++startOffset)
          {
            if (CompareData(pattern, exeImage.AsSpan<byte>(startOffset)))
            {
              flag = true;
              address[iPattern] = (long) startOffset;
              break;
            }
          }
          if (flag)
            return;
          for (int start = 0; start < pattern.StartOffset; ++start)
          {
            if (CompareData(pattern, exeImage.AsSpan<byte>(start)))
            {
              address[iPattern] = (long) start;
              break;
            }
          }
        }
      }
    }

    public bool IsInvalid() => this.Process.HasExited || this.closed;

    public IList<T> ReadNativeArray<T>(INativePtrArray ptrArray, int offset = 8) where T : unmanaged
    {
      IntPtr num = ptrArray.First;
      long int64_1 = num.ToInt64();
      num = ptrArray.Last;
      long int64_2 = num.ToInt64();
      int offset1 = offset;
      return this.ReadNativeArray<T>(int64_1, int64_2, offset1);
    }

    public IList<T> ReadNativeArray<T>(long first, long last, int offset = 8) where T : unmanaged
    {
      if (first == 0L)
        return (IList<T>) new List<T>();
      List<T> objList = new List<T>((int) (last - first) / offset);
      foreach (long readPointers in (IEnumerable<long>) this.ReadPointersArray(first, last, offset))
        objList.Add(this.Read<T>(readPointers));
      return (IList<T>) objList;
    }

    public void Dispose()
    {
      if (this.closed)
        return;
      this.closed = true;
      try
      {
        NativeWrapper.CloseHandle(this.OpenProcessHandle);
      }
      catch (Exception ex)
      {
        Logger.Log.Error("Error when dispose memory: " + ex.Message);
      }
    }

    private static string RTrimNull(string text)
    {
      int length = text.IndexOf(char.MinValue);
      return length <= 0 ? text : text.Substring(0, length);
    }
  }
}
