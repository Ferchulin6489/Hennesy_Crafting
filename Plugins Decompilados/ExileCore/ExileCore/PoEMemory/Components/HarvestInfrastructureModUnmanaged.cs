// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HarvestInfrastructureModUnmanaged
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.InteropServices;

namespace ExileCore.PoEMemory.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  internal struct HarvestInfrastructureModUnmanaged
  {
    [FieldOffset(0)]
    public long DatPtrUnused;
    [FieldOffset(8)]
    public long DatEntryPtr;
    [FieldOffset(16)]
    public int ModLevel;
    [FieldOffset(20)]
    public int Unknown;
  }
}
