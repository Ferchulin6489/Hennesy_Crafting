// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.IngameState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class IngameState : RemoteMemoryObject
  {
    private readonly CachedValue<Camera> _camera;
    private readonly CachedValue<Vector2> _CurrentUIElementPos;
    private readonly CachedValue<Vector2i> _MousePos;
    private readonly CachedValue<EntityLabelMapOffsets> _EntityLabelMap;
    private readonly CachedValue<IngameData> _ingameData;
    private readonly CachedValue<IngameStateOffsets> _ingameState;
    private readonly CachedValue<IngameUIElements> _ingameUi;
    private readonly CachedValue<float> _TimeInGameF;
    private readonly CachedValue<Element> _UIHover;
    private readonly CachedValue<Element> _UIHoverElement;
    private readonly CachedValue<Vector2> _UIHoverPos;
    private readonly CachedValue<Element> _UIRoot;
    private static readonly int WorldDataOffset = Extensions.GetOffset<IngameStateOffsets>((Expression<Func<IngameStateOffsets, object>>) (x => (object) x.WorldData));
    private static readonly int CameraOffset = Extensions.GetOffset<WorldDataOffsets>((Expression<Func<WorldDataOffsets, object>>) (x => (object) x.Camera));

    public IngameState()
    {
      this._ingameState = (CachedValue<IngameStateOffsets>) new FrameCache<IngameStateOffsets>((Func<IngameStateOffsets>) (() => this.M.Read<IngameStateOffsets>(this.Address)));
      this._camera = (CachedValue<Camera>) new AreaCache<Camera>((Func<Camera>) (() => this.GetObject<Camera>(this.M.Read<long>(this.Address + (long) IngameState.WorldDataOffset) + (long) IngameState.CameraOffset)));
      this._ingameData = (CachedValue<IngameData>) new AreaCache<IngameData>((Func<IngameData>) (() => this.GetObject<IngameData>(this._ingameState.Value.Data)));
      this._ingameUi = (CachedValue<IngameUIElements>) new AreaCache<IngameUIElements>((Func<IngameUIElements>) (() => this.GetObject<IngameUIElements>(this._ingameState.Value.IngameUi)));
      this._UIRoot = (CachedValue<Element>) new AreaCache<Element>((Func<Element>) (() => this.GetObject<Element>(this._ingameState.Value.UIRoot)));
      this._UIHover = (CachedValue<Element>) new FrameCache<Element>((Func<Element>) (() => this.GetObject<Element>(this._ingameState.Value.UIHover)));
      this._UIHoverElement = (CachedValue<Element>) new FrameCache<Element>((Func<Element>) (() => this.GetObject<Element>(this._ingameState.Value.UIHoverElement)));
      this._UIHoverPos = (CachedValue<Vector2>) new FrameCache<Vector2>((Func<Vector2>) (() => this._ingameState.Value.UIHoverPos));
      this._CurrentUIElementPos = (CachedValue<Vector2>) new FrameCache<Vector2>((Func<Vector2>) (() => this._ingameState.Value.CurentUIElementPos));
      this._MousePos = (CachedValue<Vector2i>) new FrameCache<Vector2i>((Func<Vector2i>) (() => this._ingameState.Value.MouseGlobal));
      this._TimeInGameF = (CachedValue<float>) new FrameCache<float>((Func<float>) (() => this._ingameState.Value.TimeInGameF));
      this._EntityLabelMap = (CachedValue<EntityLabelMapOffsets>) new AreaCache<EntityLabelMapOffsets>((Func<EntityLabelMapOffsets>) (() => this.M.Read<EntityLabelMapOffsets>(this._ingameState.Value.EntityLabelMap)));
    }

    public Camera Camera => this._camera.Value;

    public IngameData Data => this._ingameData.Value;

    public bool InGame => this.ServerData.IsInGame;

    public ServerData ServerData => this._ingameData.Value.ServerData;

    public IngameUIElements IngameUi => this._ingameUi.Value;

    public Element UIRoot => this._UIRoot.Value;

    public ShortcutSettings ShortcutSettings => this.UIRoot?.GetChildAtIndex(0).AsObject<ShortcutSettings>();

    public Element UIHover => this._UIHover.Value;

    public float UIHoverX => this._UIHoverPos.Value.X;

    public float UIHoverY => this._UIHoverPos.Value.Y;

    public Element UIHoverElement => this._UIHoverElement.Value;

    public Element UIHoverTooltip => this.UIHoverElement;

    public float CurentUElementPosX => this._CurrentUIElementPos.Value.X;

    public float CurentUElementPosY => this._CurrentUIElementPos.Value.Y;

    public int MousePosX => this._MousePos.Value.X;

    public int MousePosY => this._MousePos.Value.Y;

    public long EntityLabelMap => this._EntityLabelMap.Value.EntityLabelMap;

    public TimeSpan TimeInGame => TimeSpan.FromSeconds((double) this._ingameState.Value.TimeInGameF);

    public float TimeInGameF => this._TimeInGameF.Value;

    public void UpdateData() => this._ingameData.ForceUpdate();
  }
}
