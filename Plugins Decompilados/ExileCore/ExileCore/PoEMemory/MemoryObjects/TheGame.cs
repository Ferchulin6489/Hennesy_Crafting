// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.TheGame
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using GameOffsets.Objects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class TheGame : RemoteMemoryObject
  {
    private static readonly int DataOffset = Extensions.GetOffset<IngameStateOffsets>((Expression<Func<IngameStateOffsets, object>>) (x => (object) x.Data));
    private static readonly int CurrentAreaHashOffset = Extensions.GetOffset<IngameDataOffsets>((Expression<Func<IngameDataOffsets, object>>) (x => (object) x.CurrentAreaHash));
    private static long PreGameStatePtr = -1;
    private static long LoginStatePtr = -1;
    private static long SelectCharacterStatePtr = -1;
    private static long WaitingStatePtr = -1;
    private static long InGameStatePtr = -1;
    private static long LoadingStatePtr = -1;
    private static long EscapeStatePtr = -1;
    private static CachedValue<DiagnosticInfoType> _DiagnosticInfoType;
    private static TheGame Instance;
    private readonly CachedValue<int> _AreaChangeCount;
    private readonly CachedValue<bool> _inGame;
    private readonly CachedValue<int> _blackBarSize;
    private readonly CachedValue<byte[]> _rotationHelper;
    private readonly CachedValue<byte[]> _rotationSelector;
    public readonly Dictionary<GameStateTypes, long> AllGameStates;

    public TheGame(IMemory m, ExileCore.Shared.Cache.Cache cache, CoreSettings settings)
    {
      TheGame theGame = this;
      RemoteMemoryObject.pM = m;
      RemoteMemoryObject.pCache = cache;
      RemoteMemoryObject.pTheGame = this;
      TheGame.Instance = this;
      this.Address = m.Read<long>(m.BaseOffsets[OffsetsName.GameStateOffset] + m.AddressOfProcess);
      this._AreaChangeCount = (CachedValue<int>) new TimeCache<int>((Func<int>) (() => theGame.M.Read<int>(theGame.M.AddressOfProcess + theGame.M.BaseOffsets[OffsetsName.AreaChangeCount])), 50L);
      TheGame._DiagnosticInfoType = (CachedValue<DiagnosticInfoType>) new TimeCache<DiagnosticInfoType>((Func<DiagnosticInfoType>) (() => theGame.M.Read<DiagnosticInfoType>(theGame.M.AddressOfProcess + theGame.M.BaseOffsets[OffsetsName.DiagnosticInfoTypeOffset])), 5L);
      this._blackBarSize = (CachedValue<int>) new TimeCache<int>((Func<int>) (() => !(bool) settings.DisableBlackBarAdjustment ? theGame.M.Read<int>(theGame.M.AddressOfProcess + theGame.M.BaseOffsets[OffsetsName.BlackBarSize]) : 0), 250L);
      this._rotationSelector = (CachedValue<byte[]>) new StaticValueCache<byte[]>((Func<byte[]>) (() => theGame.M.ReadBytes(theGame.M.AddressOfProcess + theGame.M.BaseOffsets[OffsetsName.TerrainRotationSelector], 8)));
      this._rotationHelper = (CachedValue<byte[]>) new StaticValueCache<byte[]>((Func<byte[]>) (() => theGame.M.ReadBytes(theGame.M.AddressOfProcess + theGame.M.BaseOffsets[OffsetsName.TerrainRotationHelper], 24)));
      this.AllGameStates = this.ReadStates(this.Address);
      TheGame.PreGameStatePtr = this.AllGameStates[GameStateTypes.PreGameState];
      TheGame.LoginStatePtr = this.AllGameStates[GameStateTypes.LoginState];
      TheGame.SelectCharacterStatePtr = this.AllGameStates[GameStateTypes.SelectCharacterState];
      TheGame.WaitingStatePtr = this.AllGameStates[GameStateTypes.WaitingState];
      TheGame.InGameStatePtr = this.AllGameStates[GameStateTypes.InGameState];
      TheGame.LoadingStatePtr = this.AllGameStates[GameStateTypes.LoadingState];
      TheGame.EscapeStatePtr = this.AllGameStates[GameStateTypes.EscapeState];
      this.LoadingState = this.GetObject<AreaLoadingState>(this.AllGameStates[GameStateTypes.AreaLoadingState]);
      this.IngameState = this.GetObject<IngameState>(this.AllGameStates[GameStateTypes.InGameState]);
      this._inGame = (CachedValue<bool>) new FrameCache<bool>((Func<bool>) (() => theGame.IngameState.Address != 0L && theGame.IngameState.Data.Address != 0L && theGame.IngameState.ServerData.Address != 0L && !theGame.IsLoading));
      this.Files = new FilesContainer(m);
    }

    public FilesContainer Files { get; set; }

    public AreaLoadingState LoadingState { get; }

    public IngameState IngameState { get; }

    public IList<GameState> CurrentGameStates => this.M.ReadDoublePtrVectorClasses<GameState>(this.Address + 8L, (RemoteMemoryObject) this.IngameState);

    public IList<GameState> ActiveGameStates => this.M.ReadDoublePtrVectorClasses<GameState>(this.Address + 32L, (RemoteMemoryObject) this.IngameState, true);

    public bool IsPreGame => TheGame.GameStateActive(TheGame.PreGameStatePtr);

    public bool IsLoginState => TheGame.GameStateActive(TheGame.LoginStatePtr);

    public bool IsSelectCharacterState => TheGame.GameStateActive(TheGame.SelectCharacterStatePtr);

    public bool IsWaitingState => TheGame.GameStateActive(TheGame.WaitingStatePtr);

    public bool IsInGameState => TheGame.GameStateActive(TheGame.InGameStatePtr);

    public bool IsLoadingState => TheGame.GameStateActive(TheGame.LoadingStatePtr);

    public bool IsEscapeState => TheGame.GameStateActive(TheGame.EscapeStatePtr);

    public bool IsLoading => this.LoadingState.IsLoading;

    public int AreaChangeCount => this._AreaChangeCount.Value;

    public bool InGame => this._inGame.Value;

    public DiagnosticInfoType DiagnosticInfoType => TheGame._DiagnosticInfoType.Value;

    public int BlackBarSize => this._blackBarSize.Value;

    public byte[] TerrainRotationSelector => this._rotationSelector.Value;

    public byte[] TerrainRotationHelper => this._rotationHelper.Value;

    public uint CurrentAreaHash => this.M.Read<uint>(this.IngameState.Address + (long) TheGame.DataOffset, TheGame.CurrentAreaHashOffset);

    public void Init()
    {
    }

    public void ReloadFiles() => this.Files = new FilesContainer(RemoteMemoryObject.pM);

    private static bool GameStateActive(long stateAddress)
    {
      TheGame instance = TheGame.Instance;
      if (instance == null)
        return false;
      IMemory m = instance.M;
      long addr1 = TheGame.Instance.Address + 32L;
      long addr2 = m.Read<long>(addr1);
      int size = (int) (m.Read<long>(addr1 + 16L) - addr2);
      byte[] numArray = m.ReadMem(addr2, size);
      for (int startIndex = 0; startIndex < size; startIndex += 16)
      {
        long int64 = BitConverter.ToInt64(numArray, startIndex);
        if (stateAddress == int64)
          return true;
      }
      return false;
    }

    private Dictionary<GameStateTypes, long> ReadStates(long pointer)
    {
      Dictionary<GameStateTypes, long> dictionary = new Dictionary<GameStateTypes, long>();
      GameStateOffsets gameStateOffsets = this.M.Read<GameStateOffsets>(pointer);
      dictionary[GameStateTypes.AreaLoadingState] = gameStateOffsets.State0;
      dictionary[GameStateTypes.WaitingState] = gameStateOffsets.State1;
      dictionary[GameStateTypes.CreditsState] = gameStateOffsets.State2;
      dictionary[GameStateTypes.EscapeState] = gameStateOffsets.State3;
      dictionary[GameStateTypes.InGameState] = gameStateOffsets.State4;
      dictionary[GameStateTypes.ChangePasswordState] = gameStateOffsets.State5;
      dictionary[GameStateTypes.LoginState] = gameStateOffsets.State6;
      dictionary[GameStateTypes.PreGameState] = gameStateOffsets.State7;
      dictionary[GameStateTypes.CreateCharacterState] = gameStateOffsets.State8;
      dictionary[GameStateTypes.SelectCharacterState] = gameStateOffsets.State9;
      dictionary[GameStateTypes.DeleteCharacterState] = gameStateOffsets.State10;
      dictionary[GameStateTypes.LoadingState] = gameStateOffsets.State11;
      return dictionary;
    }

    private class GameStateHashNode : RemoteMemoryObject
    {
      public TheGame.GameStateHashNode Previous => this.ReadObject<TheGame.GameStateHashNode>(this.Address);

      public TheGame.GameStateHashNode Root => this.ReadObject<TheGame.GameStateHashNode>(this.Address + 8L);

      public TheGame.GameStateHashNode Next => this.ReadObject<TheGame.GameStateHashNode>(this.Address + 16L);

      public bool IsNull => this.M.Read<byte>(this.Address + 25L) > (byte) 0;

      public string Key => this.M.ReadNativeString(this.Address + 32L);

      public GameState Value1 => this.ReadObject<GameState>(this.Address + 64L);
    }
  }
}
