// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.AtlasNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Atlas;
using SharpDX;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class AtlasNode : RemoteMemoryObject
  {
    private WorldArea area;
    private float posX = -1f;
    private float posY = -1f;
    private string text;
    private const int TIER_LAYERS = 161;

    public WorldArea Area => this.area ?? (this.area = this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(this.Address)));

    public Vector2 PosL0 => this.GetPosByLayer(0);

    public Vector2 PosL1 => this.GetPosByLayer(1);

    public Vector2 PosL2 => this.GetPosByLayer(2);

    public Vector2 PosL3 => this.GetPosByLayer(3);

    public Vector2 PosL4 => this.GetPosByLayer(4);

    public Vector2 DefaultPos => this.PosL0;

    public float PosX => this.posX;

    public float PosY => this.posY;

    public byte Flag0 => this.M.Read<byte>(this.Address + 32L);

    public string FlavourText => this.text ?? (this.text = this.M.ReadStringU(this.M.Read<long>(this.M.Read<long>(this.Address + 49L) + 12L)));

    public AtlasRegion AtlasRegion => this.TheGame.Files.AtlasRegions.GetByAddress(this.M.Read<long>(this.Address + 65L));

    public Vector2 GetPosByLayer(int layer) => new Vector2(this.M.Read<float>(this.Address + 181L + (long) (layer * 4)), this.M.Read<float>(this.Address + 201L + (long) (layer * 4)));

    public int GetTierByLayer(int layer) => this.M.Read<int>(this.Address + 161L + (long) (layer * 4));

    public int GetLayerByTier(int tier)
    {
      for (int layerByTier = 0; layerByTier < 5; ++layerByTier)
      {
        if (this.M.Read<int>(this.Address + 161L + (long) (layerByTier * 4)) == tier)
          return layerByTier;
      }
      return -1;
    }

    public bool IsUniqueMap
    {
      get
      {
        string str = this.M.ReadStringU(this.M.Read<long>(this.Address + 16L, new int[1]));
        return !string.IsNullOrEmpty(str) && str.StartsWith("Uniq");
      }
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 4);
      interpolatedStringHandler.AppendFormatted(this.Area?.Name);
      interpolatedStringHandler.AppendLiteral(", PosX: ");
      interpolatedStringHandler.AppendFormatted<float>(this.PosX);
      interpolatedStringHandler.AppendLiteral(", PosY: ");
      interpolatedStringHandler.AppendFormatted<float>(this.PosY);
      interpolatedStringHandler.AppendLiteral(", Text: ");
      interpolatedStringHandler.AppendFormatted(this.FlavourText);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
