// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.TriggerableBlockage
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;

namespace ExileCore.PoEMemory.Components
{
  public class TriggerableBlockage : Component
  {
    public bool IsClosed => this.Address != 0L && this.M.Read<byte>(this.Address + 48L) == (byte) 1;

    public bool IsOpened => this.Address != 0L && this.M.Read<byte>(this.Address + 48L) == (byte) 0;

    public Point Min => new Point(this.M.Read<int>(this.Address + 80L), this.M.Read<int>(this.Address + 84L));

    public Point Max => new Point(this.M.Read<int>(this.Address + 88L), this.M.Read<int>(this.Address + 92L));

    public byte[] Data
    {
      get
      {
        long addr = this.M.Read<long>(this.Address + 56L);
        long num = this.M.Read<long>(this.Address + 64L);
        return this.M.ReadBytes(addr, (int) (num - addr));
      }
    }
  }
}
