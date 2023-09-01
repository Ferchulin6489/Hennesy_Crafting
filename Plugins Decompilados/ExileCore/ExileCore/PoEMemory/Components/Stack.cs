// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Stack
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Components
{
  public class Stack : Component
  {
    public int Size => this.Address != 0L ? this.M.Read<int>(this.Address + 24L) : 0;

    public CurrencyInfo Info => this.Address == 0L ? (CurrencyInfo) null : this.ReadObject<CurrencyInfo>(this.Address + 16L);

    public bool FullStack => this.Info.MaxStackSize == this.Size;
  }
}
