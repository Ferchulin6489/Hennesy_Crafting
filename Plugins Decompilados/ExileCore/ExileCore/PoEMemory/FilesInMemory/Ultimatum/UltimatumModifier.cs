// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Ultimatum.UltimatumModifier
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory.Ultimatum
{
  public class UltimatumModifier : RemoteMemoryObject
  {
    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public override string ToString() => this.Name;
  }
}
