// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Transitionable
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Components
{
  public class Transitionable : Component
  {
    public byte Flag1 => this.M.Read<byte>(this.Address + 288L);

    public byte Flag2 => this.M.Read<byte>(this.Address + 292L);
  }
}
