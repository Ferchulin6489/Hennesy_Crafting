// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.WorldMapElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class WorldMapElement : Element
  {
    public Element Panel => this.GetObject<Element>(this.M.Read<long>(this.Address + 2744L, 3088));

    public Element GetPartButton(int part)
    {
      if (part >= 1 && part <= 2)
        return this.FindChildRecursive("Part " + part.ToString());
      return part == 3 ? this.FindChildRecursive("Epilogue") : (Element) null;
    }

    public Element GetActButton(int act)
    {
      Element childFromIndices = this.GetChildFromIndices(2, 0, 1);
      if (act >= 1 && act <= 10)
        return childFromIndices?.FindChildRecursive("Act " + act.ToString());
      if (act != 11)
        return (Element) null;
      return childFromIndices?.FindChildRecursive("Epilogue");
    }

    public Element WaypointsRoot => this.GetChildFromIndices(2, 0, 1, 0, 0, 1, 1, 0, 0, 2);
  }
}
