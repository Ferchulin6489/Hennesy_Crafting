// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.NativeStringReader
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class NativeStringReader
  {
    public static string ReadString(long address, IMemory M) => NativeStringReader.ReadString(address, M, 512);

    public static string ReadString(long address, IMemory M, int lengthBytes)
    {
      if (8U > M.Read<uint>(address + 24L))
        return M.ReadStringU(address, lengthBytes);
      long addr = M.Read<long>(address);
      return M.ReadStringU(addr, lengthBytes);
    }

    public static string ReadStringLong(long address, IMemory M)
    {
      int lengthBytes = (int) M.Read<uint>(address + 16L) * 2;
      if (8U > M.Read<uint>(address + 24L))
        return M.ReadStringU(address, lengthBytes);
      long addr = M.Read<long>(address);
      return M.ReadStringU(addr, lengthBytes);
    }
  }
}
