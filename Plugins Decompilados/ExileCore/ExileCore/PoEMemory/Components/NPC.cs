// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.NPC
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Components
{
  public class NPC : Component
  {
    public bool HasIconOverhead => this.M.Read<long>(this.Address + 72L) != 0L;

    public bool IsIgnoreHidden => this.M.Read<byte>(this.Address + 32L) == (byte) 1;

    public bool IsMinMapLabelVisible => this.M.Read<byte>(this.Address + 33L) == (byte) 1;
  }
}
