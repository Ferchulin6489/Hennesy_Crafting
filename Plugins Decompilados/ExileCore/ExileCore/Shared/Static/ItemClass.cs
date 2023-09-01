// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Static.ItemClass
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.Shared.Static
{
  public class ItemClass
  {
    public ItemClass(string className, string classCategory)
    {
      this.ClassName = className;
      this.ClassCategory = classCategory;
    }

    public string ClassName { get; set; }

    public string ClassCategory { get; set; }
  }
}
