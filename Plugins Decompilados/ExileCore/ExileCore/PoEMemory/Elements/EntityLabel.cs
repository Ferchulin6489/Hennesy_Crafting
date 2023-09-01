// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.EntityLabel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
  public class EntityLabel : Element
  {
    public new string Text => NativeStringReader.ReadString(this.Address + 744L, this.M);

    public string Text2 => NativeStringReader.ReadString(this.Address + 744L, this.M);
  }
}
