// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.Sanctum.SanctumFloorWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;


#nullable enable
namespace ExileCore.PoEMemory.Elements.Sanctum
{
  public class SanctumFloorWindow : Element
  {
    private readonly 
    #nullable disable
    CachedValue<SanctumFloorWindowOffsets> _cachedValue;

    public SanctumFloorWindow() => this._cachedValue = (CachedValue<SanctumFloorWindowOffsets>) this.CreateStructFrameCache<SanctumFloorWindowOffsets>();

    public List<List<SanctumRoomElement>> RoomsByLayer
    {
      get
      {
        Element childFromIndices = this.GetChildFromIndices(0, 0, 0, 1);
        return (childFromIndices != null ? childFromIndices.Children.Select<Element, List<SanctumRoomElement>>((Func<Element, List<SanctumRoomElement>>) (x => x.GetChildrenAs<SanctumRoomElement>())).ToList<List<SanctumRoomElement>>() : (List<List<SanctumRoomElement>>) null) ?? new List<List<SanctumRoomElement>>();
      }
    }

    public List<SanctumRoomElement> Rooms => this.RoomsByLayer.SelectMany<List<SanctumRoomElement>, SanctumRoomElement>((Func<List<SanctumRoomElement>, IEnumerable<SanctumRoomElement>>) (x => (IEnumerable<SanctumRoomElement>) x)).ToList<SanctumRoomElement>();

    private SanctumFloorWindowDataSelector DataSelector
    {
      get
      {
        SanctumFloorWindowOffsets floorWindowOffsets = this._cachedValue.Value;
        long inSanctumDataPtr = floorWindowOffsets.InSanctumDataPtr;
        long address;
        if (inSanctumDataPtr == 0L)
        {
          long ofSanctumDataPtr = floorWindowOffsets.OutOfSanctumDataPtr;
          address = ofSanctumDataPtr == 0L ? 0L : ofSanctumDataPtr;
        }
        else
          address = inSanctumDataPtr;
        return this.GetObject<SanctumFloorWindowDataSelector>(address);
      }
    }

    public SanctumFloorData FloorData => this.DataSelector.FloorData;
  }
}
