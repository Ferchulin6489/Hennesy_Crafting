// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.AtlasElements.AtlasMasterMissionPanelElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements.AtlasElements
{
  public class AtlasMasterMissionPanelElement : Element
  {
    public Element AtlasMasterMissionInfoIcon => this.GetChildAtIndex(0);

    public Element AtlasMasterMissions => this.GetChildFromIndices(1, 0);

    public Dictionary<MasterMissionColour, int> EinharMissions => new Dictionary<MasterMissionColour, int>()
    {
      {
        MasterMissionColour.White,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(1, 0, 0, 0).Text)
      },
      {
        MasterMissionColour.Yellow,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(1, 0, 1, 0).Text)
      },
      {
        MasterMissionColour.Red,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(1, 0, 2, 0).Text)
      }
    };

    public Dictionary<MasterMissionColour, int> AlvaMissions => new Dictionary<MasterMissionColour, int>()
    {
      {
        MasterMissionColour.White,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(2, 0, 0, 0).Text)
      },
      {
        MasterMissionColour.Yellow,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(2, 0, 1, 0).Text)
      },
      {
        MasterMissionColour.Red,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(2, 0, 2, 0).Text)
      }
    };

    public Dictionary<MasterMissionColour, int> NikoMissions => new Dictionary<MasterMissionColour, int>()
    {
      {
        MasterMissionColour.White,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(4, 0, 0, 0).Text)
      },
      {
        MasterMissionColour.Yellow,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(4, 0, 1, 0).Text)
      },
      {
        MasterMissionColour.Red,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(4, 0, 2, 0).Text)
      }
    };

    public Dictionary<MasterMissionColour, int> JunMissions => new Dictionary<MasterMissionColour, int>()
    {
      {
        MasterMissionColour.White,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(5, 0, 0, 0).Text)
      },
      {
        MasterMissionColour.Yellow,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(5, 0, 1, 0).Text)
      },
      {
        MasterMissionColour.Red,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(5, 0, 2, 0).Text)
      }
    };

    public Dictionary<MasterMissionColour, int> KiracMissions => new Dictionary<MasterMissionColour, int>()
    {
      {
        MasterMissionColour.White,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(6, 0, 0, 0).Text)
      },
      {
        MasterMissionColour.Yellow,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(6, 0, 1, 0).Text)
      },
      {
        MasterMissionColour.Red,
        int.Parse(this.AtlasMasterMissions.GetChildFromIndices(6, 0, 2, 0).Text)
      }
    };
  }
}
