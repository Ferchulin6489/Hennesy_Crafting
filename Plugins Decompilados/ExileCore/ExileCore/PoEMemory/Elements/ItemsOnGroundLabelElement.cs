// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ItemsOnGroundLabelElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using GameOffsets;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class ItemsOnGroundLabelElement : Element
  {
    private ItemsOnGroundLabelElementOffsets Data => this.M.Read<ItemsOnGroundLabelElementOffsets>(this.Address);

    public Element LabelOnHover
    {
      get
      {
        Element element = this.GetObject<Element>(this.Data.LabelOnHoverPtr);
        return element.Address != 0L ? element : (Element) null;
      }
    }

    public Entity ItemOnHover
    {
      get
      {
        Entity entity = this.GetObject<Entity>(this.Data.ItemOnHoverPtr);
        return entity.Address != 0L ? entity : (Entity) null;
      }
    }

    public string ItemOnHoverPath => this.ItemOnHover == null ? "Null" : this.ItemOnHover.Path;

    public string LabelOnHoverText => this.LabelOnHover == null ? "Null" : this.LabelOnHover.Text;

    public int CountLabels => this.M.Read<int>(this.Address + 688L);

    public int CountLabels2 => this.M.Read<int>(this.Address + 752L);

    public List<LabelOnGround> LabelsOnGround
    {
      get
      {
        long labelsOnGroundListPtr = this.Data.LabelsOnGroundListPtr;
        List<LabelOnGround> labelsOnGround = new List<LabelOnGround>();
        if (labelsOnGroundListPtr <= 0L)
          return (List<LabelOnGround>) null;
        int num = 0;
        for (long index = this.M.Read<long>(labelsOnGroundListPtr); index != labelsOnGroundListPtr; index = this.M.Read<long>(index))
        {
          LabelOnGround labelOnGround = this.GetObject<LabelOnGround>(index);
          if (labelOnGround.Label.IsValid)
            labelsOnGround.Add(labelOnGround);
          ++num;
          if (num > 5000)
            return (List<LabelOnGround>) null;
        }
        return labelsOnGround;
      }
    }
  }
}
