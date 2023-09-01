// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.InventoryElements.MapSubInventoryKey
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Elements.InventoryElements
{
  public class MapSubInventoryKey
  {
    public string Path;
    public MapType Type;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
      interpolatedStringHandler.AppendLiteral("Path:");
      interpolatedStringHandler.AppendFormatted(this.Path);
      interpolatedStringHandler.AppendLiteral(" Type:");
      interpolatedStringHandler.AppendFormatted<MapType>(this.Type);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
