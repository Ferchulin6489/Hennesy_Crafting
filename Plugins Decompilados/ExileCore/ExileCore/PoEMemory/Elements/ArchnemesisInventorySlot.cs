// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ArchnemesisInventorySlot
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.Elements
{
  public class ArchnemesisInventorySlot : Element
  {
    private const int ItemOffset = 992;

    public bool HasItem => this.M.Read<long>(this.Address + 992L) != 0L;

    public ArchnemesisMod Item => this.TheGame.Files.ArchnemesisMods.GetByAddress(this.M.Read<long>(this.Address + 992L));
  }
}
