// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SentinelSubPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
  public class SentinelSubPanel : Element
  {
    public SentinelData SentinelData => this.Tooltip.ReadObjectAt<SentinelData>(776);

    public Entity SentinelItem => this.GetChildFromIndices(new int[2])?.ReadObjectAt<Entity>(928);
  }
}
