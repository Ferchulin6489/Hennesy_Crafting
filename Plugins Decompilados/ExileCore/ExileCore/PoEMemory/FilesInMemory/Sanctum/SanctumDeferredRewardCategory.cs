// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Sanctum.SanctumDeferredRewardCategory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Models;

namespace ExileCore.PoEMemory.FilesInMemory.Sanctum
{
  public class SanctumDeferredRewardCategory : RemoteMemoryObject
  {
    public BaseItemType BaseType => this.TheGame.Files.BaseItemTypes.GetFromAddress(this.M.Read<long>(this.Address));

    public string CurrencyName => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public override string ToString() => this.CurrencyName;
  }
}
