// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.SellWindowHideout
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class SellWindowHideout : SellWindow
  {
    public override Element YourOffer => this.SellDialog?.GetChildAtIndex(1);

    public override Element OtherOffer => this.SellDialog?.GetChildAtIndex(0);

    public override Element SellDialog => this.GetChildAtIndex(4);
  }
}
