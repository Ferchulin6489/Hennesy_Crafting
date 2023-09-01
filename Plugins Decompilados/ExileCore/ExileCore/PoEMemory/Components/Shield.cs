// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Shield
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Components
{
  public class Shield : Component
  {
    public int ChanceToBlock
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        return this.M.Read<int>(this.Address + 16L, 16);
      }
    }
  }
}
