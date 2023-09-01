// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.IngameData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class IngameData : RemoteMemoryObject
  {
    private const double TileHeightFinalMultiplier = 7.8125;
    private static readonly int EntitiesCountOffset = Extensions.GetOffset<IngameDataOffsets>((Expression<Func<IngameDataOffsets, object>>) (x => (object) x.EntitiesCount));
    private readonly CachedValue<IngameDataOffsets> _cacheStruct;
    private readonly CachedValue<AreaTemplate> _CurrentArea;
    private readonly CachedValue<WorldArea> _CurrentWorldArea;
    private readonly CachedValue<long> _EntitiesCount;
    private EntityList _EntityList;
    private readonly CachedValue<ServerData> _serverData;
    private readonly CachedValue<Entity> _localPlayer;
    private NativePtrArray cacheStats;
    private Dictionary<GameStat, int> mapStats = new Dictionary<GameStat, int>();
    private readonly CachedValue<float[][]> _terrainHeight;
    private readonly CachedValue<int[][]> _terrainPathfindingData;
    private readonly CachedValue<Vector2i> _areaDimensions;

    public IngameData()
    {
      this._cacheStruct = (CachedValue<IngameDataOffsets>) new AreaCache<IngameDataOffsets>((Func<IngameDataOffsets>) (() => this.M.Read<IngameDataOffsets>(this.Address)));
      this._serverData = (CachedValue<ServerData>) new AreaCache<ServerData>((Func<ServerData>) (() => this.GetObject<ServerData>(this._cacheStruct.Value.ServerData)));
      this._localPlayer = (CachedValue<Entity>) new AreaCache<Entity>((Func<Entity>) (() => this.GetObject<Entity>(this._cacheStruct.Value.LocalPlayer)));
      this._CurrentArea = (CachedValue<AreaTemplate>) new AreaCache<AreaTemplate>((Func<AreaTemplate>) (() => this.GetObject<AreaTemplate>(this._cacheStruct.Value.CurrentArea)));
      this._CurrentWorldArea = (CachedValue<WorldArea>) new AreaCache<WorldArea>((Func<WorldArea>) (() => this.TheGame.Files.WorldAreas.GetByAddress(this.CurrentArea.Address)));
      this._EntitiesCount = (CachedValue<long>) new FrameCache<long>((Func<long>) (() => this.M.Read<long>(this.Address + (long) IngameData.EntitiesCountOffset)));
      this._terrainHeight = (CachedValue<float[][]>) new AreaCache<float[][]>(new Func<float[][]>(this.GetTerrainHeight));
      this._terrainPathfindingData = (CachedValue<int[][]>) new AreaCache<int[][]>(new Func<int[][]>(this.GetTerrainPathfindingData));
      this._areaDimensions = (CachedValue<Vector2i>) new AreaCache<Vector2i>((Func<Vector2i>) (() => new Vector2i(this.Terrain.BytesPerRow * 2, (int) (this.Terrain.LayerMelee.Size / (long) this.Terrain.BytesPerRow))));
    }

    public IngameDataOffsets DataStruct => this._cacheStruct.Value;

    public long EntitiesCount => this._EntitiesCount.Value;

    public AreaTemplate CurrentArea => this._CurrentArea.Value;

    public WorldArea CurrentWorldArea => this._CurrentWorldArea.Value;

    public int CurrentAreaLevel => (int) this._cacheStruct.Value.CurrentAreaLevel;

    public uint CurrentAreaHash => this._cacheStruct.Value.CurrentAreaHash;

    public Entity LocalPlayer => this._localPlayer.Value;

    public ServerData ServerData => this._serverData.Value;

    public long EntiteisTest => this.DataStruct.EntityList;

    public EntityList EntityList => this._EntityList ?? (this._EntityList = this.GetObject<EntityList>(this.DataStruct.EntityList));

    private long LabDataPtr => this._cacheStruct.Value.LabDataPtr;

    public LabyrinthData LabyrinthData => this.LabDataPtr != 0L ? this.GetObject<LabyrinthData>(this.LabDataPtr) : (LabyrinthData) null;

    public TerrainData Terrain => this._cacheStruct.Value.Terrain;

    public Vector2i AreaDimensions => this._areaDimensions.Value;

    public int[][] RawPathfindingData => this._terrainPathfindingData.Value;

    public float[][] RawTerrainHeightData => this._terrainHeight.Value;

    public float GetTerrainHeightAt(Vector2 gridPosition) => this._terrainHeight.Value[(int) gridPosition.Y][(int) gridPosition.X];

    public int GetPathfindingValueAt(Vector2 gridPosition) => this._terrainPathfindingData.Value[(int) gridPosition.Y][(int) gridPosition.X];

    private int[][] GetTerrainPathfindingData()
    {
      byte[] mapTextureData = this.M.ReadStdVector<byte>(this.Terrain.LayerMelee);
      int bytesPerRow = this.Terrain.BytesPerRow;
      int toExclusive = mapTextureData.Length / bytesPerRow;
      int[][] processedTerrainData = new int[toExclusive][];
      int xSize = bytesPerRow * 2;
      for (int index = 0; index < toExclusive; ++index)
        processedTerrainData[index] = new int[xSize];
      Parallel.For(0, toExclusive, (Action<int>) (y =>
      {
        for (int index1 = 0; index1 < xSize; index1 += 2)
        {
          byte num1 = mapTextureData[y * bytesPerRow + index1 / 2];
          for (int index2 = 0; index2 < 2; ++index2)
          {
            int num2 = (int) num1 >> 4 * index2 & 15;
            processedTerrainData[y][index1 + index2] = num2;
          }
        }
      }));
      return processedTerrainData;
    }

    private float[][] GetTerrainHeight()
    {
      byte[] rotationSelector = this.TheGame.TerrainRotationSelector;
      byte[] rotationHelper = this.TheGame.TerrainRotationHelper;
      TerrainData terrainMetadata = this.Terrain;
      TileStructure[] tileData = this.M.ReadStdVector<TileStructure>(terrainMetadata.TgtArray);
      Dictionary<long, sbyte[]> tileHeightCache = ((IEnumerable<TileStructure>) tileData).Select<TileStructure, long>((Func<TileStructure, long>) (x => x.SubTileDetailsPtr)).Distinct<long>().AsParallel<long>().Select(addr => new
      {
        addr = addr,
        data = this.M.ReadStdVector<sbyte>(this.M.Read<SubTileStructure>(addr).SubTileHeight)
      }).ToDictionary(x => x.addr, x => x.data);
      int gridSizeX = (int) terrainMetadata.NumCols * 23;
      int toExclusive = (int) terrainMetadata.NumRows * 23;
      float[][] result = new float[toExclusive][];
      Parallel.For(0, toExclusive, (Action<int>) (y =>
      {
        result[y] = new float[gridSizeX];
        for (int index1 = 0; index1 < gridSizeX; ++index1)
        {
          int index2 = y / 23 * (int) terrainMetadata.NumCols + index1 / 23;
          if (index2 < 0 || index2 >= tileData.Length)
          {
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
            interpolatedStringHandler.AppendLiteral("Tile data array length is ");
            interpolatedStringHandler.AppendFormatted<int>(tileData.Length);
            interpolatedStringHandler.AppendLiteral(", index was ");
            interpolatedStringHandler.AppendFormatted<int>(index2);
            DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
            result[y][index1] = 0.0f;
          }
          else
          {
            TileStructure tileStructure = tileData[index2];
            sbyte[] tileHeightArray = tileHeightCache[tileStructure.SubTileDetailsPtr];
            int num1 = 0;
            if (tileHeightArray.Length == 1)
              num1 = (int) tileHeightArray[0];
            else if (tileHeightArray.Length != 0)
            {
              int num2 = index1 % 23;
              int num3 = y % 23;
              int num4 = 22;
              int[] numArray = new int[4]
              {
                num4 - num2,
                num2,
                num4 - num3,
                num3
              };
              int index3 = (int) rotationSelector[(int) tileStructure.RotationSelector] * 3;
              int num5 = (int) rotationHelper[index3];
              int num6 = (int) rotationHelper[index3 + 1];
              int num7 = (int) rotationHelper[index3 + 2];
              int num8 = numArray[num5 * 2 + num6];
              int index4 = numArray[num7 + (1 - num5) * 2] * 23 + num8;
              num1 = IngameData.GetTileHeightFromPackedArray(tileHeightArray, index4);
            }
            result[y][index1] = -((float) ((int) tileStructure.TileHeight * terrainMetadata.TileHeightMultiplier + num1) * (125f / 16f));
          }
        }
      }));
      return result;
    }

    private static int GetTileHeightFromPackedArray(sbyte[] tileHeightArray, int index)
    {
      (int, int, int, int, int, bool) valueTuple1;
      switch (tileHeightArray.Length)
      {
        case 69:
          valueTuple1 = (3, 2, 7, 1, 1, true);
          break;
        case 137:
          valueTuple1 = (2, 4, 3, 2, 3, true);
          break;
        case 281:
          valueTuple1 = (1, 16, 1, 4, 15, true);
          break;
        default:
          valueTuple1 = ();
          break;
      }
      (int, int, int, int, int, bool) valueTuple2 = valueTuple1;
      int num1 = valueTuple2.Item1;
      int num2 = valueTuple2.Item2;
      int num3 = valueTuple2.Item3;
      int num4 = valueTuple2.Item4;
      int num5 = valueTuple2.Item5;
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      if (!valueTuple2.Item6)
      {
        if (index >= 0 && index < tileHeightArray.Length)
          return (int) tileHeightArray[index];
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
        interpolatedStringHandler.AppendLiteral("Tile height array length is ");
        interpolatedStringHandler.AppendFormatted<int>(tileHeightArray.Length);
        interpolatedStringHandler.AppendLiteral(", index (0) was ");
        interpolatedStringHandler.AppendFormatted<int>(index);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
      int index1 = (index >> num1) + num2;
      if (index1 < 0 || index1 >= tileHeightArray.Length)
      {
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
        interpolatedStringHandler.AppendLiteral("Tile height array length is ");
        interpolatedStringHandler.AppendFormatted<int>(tileHeightArray.Length);
        interpolatedStringHandler.AppendLiteral(", index (1) was ");
        interpolatedStringHandler.AppendFormatted<int>(index1);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
      else
      {
        int index2 = (int) (byte) tileHeightArray[index1] >> (index & num3) * num4 & num5;
        if (index2 >= 0 && index2 < tileHeightArray.Length)
          return (int) tileHeightArray[index2];
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
        interpolatedStringHandler.AppendLiteral("Tile height array length is ");
        interpolatedStringHandler.AppendFormatted<int>(tileHeightArray.Length);
        interpolatedStringHandler.AppendLiteral(", index (2) was ");
        interpolatedStringHandler.AppendFormatted<int>(index1);
        interpolatedStringHandler.AppendLiteral(", ");
        interpolatedStringHandler.AppendFormatted<int>(index2);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
      return 0;
    }

    public Dictionary<GameStat, int> MapStats
    {
      get
      {
        if (this.cacheStats.Equals(this._cacheStruct.Value.MapStats))
          return this.mapStats;
        if ((int) (this._cacheStruct.Value.MapStats.Last - this._cacheStruct.Value.MapStats.First) / 8 > 200)
          return (Dictionary<GameStat, int>) null;
        (GameStat, int)[] source = this.M.ReadStdVector<(GameStat, int)>(this._cacheStruct.Value.MapStats);
        this.cacheStats = this._cacheStruct.Value.MapStats;
        this.mapStats = ((IEnumerable<(GameStat, int)>) source).ToDictionary<(GameStat, int), GameStat, int>((Func<(GameStat, int), GameStat>) (x => x.stat), (Func<(GameStat, int), int>) (x => x.value));
        return this.mapStats;
      }
    }

    public IList<IngameData.PortalObject> TownPortals => (IList<IngameData.PortalObject>) this.M.ReadStructsArray<IngameData.PortalObject>(this.M.Read<long>(this.Address + 2360L), this.M.Read<long>(this.Address + 2368L), 56, (RemoteMemoryObject) this.TheGame);

    public class PortalObject : RemoteMemoryObject
    {
      public const int StructSize = 56;

      public long Id => this.M.Read<long>(this.Address);

      public string PlayerOwner => NativeStringReader.ReadString(this.Address + 8L, this.M);

      public WorldArea Area => this.TheGame.Files.WorldAreas.GetAreaByWorldId(this.M.Read<int>(this.Address + 44L));

      public override string ToString() => this.PlayerOwner + " => " + this.Area.Name;
    }
  }
}
