// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ServerStashTab
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ServerStashTab : RemoteMemoryObject
  {
    private static readonly int ColorOffset = Extensions.GetOffset<ServerStashTabOffsets>((Expression<Func<ServerStashTabOffsets, object>>) (x => (object) x.Color));
    private readonly CachedValue<ServerStashTabOffsets> _cachedValue;

    public ServerStashTab() => this._cachedValue = (CachedValue<ServerStashTabOffsets>) new FrameCache<ServerStashTabOffsets>((Func<ServerStashTabOffsets>) (() => this.M.Read<ServerStashTabOffsets>(this.Address)));

    public ServerStashTabOffsets ServerStashTabOffsets => this._cachedValue.Value;

    public string NameOld => NativeStringReader.ReadString(this.Address + 8L, this.M) + (this.RemoveOnly ? " (Remove-only)" : string.Empty);

    public string Name => this.ServerStashTabOffsets.Name.ToString(this.M);

    public uint Color => this.ServerStashTabOffsets.Color;

    public SharpDX.Color Color2 => new SharpDX.Color(this.M.Read<byte>(this.Address + (long) ServerStashTab.ColorOffset), this.M.Read<byte>(this.Address + (long) ServerStashTab.ColorOffset + 1L), this.M.Read<byte>(this.Address + (long) ServerStashTab.ColorOffset + 2L));

    public InventoryTabPermissions MemberFlags => (InventoryTabPermissions) this.ServerStashTabOffsets.MemberFlags;

    public InventoryTabPermissions OfficerFlags => (InventoryTabPermissions) this.ServerStashTabOffsets.OfficerFlags;

    public InventoryTabType TabType => (InventoryTabType) this.ServerStashTabOffsets.TabType;

    public ushort VisibleIndex => this.ServerStashTabOffsets.DisplayIndex;

    public InventoryTabFlags Flags => (InventoryTabFlags) this.ServerStashTabOffsets.Flags;

    public bool RemoveOnly => (this.Flags & InventoryTabFlags.RemoveOnly) == InventoryTabFlags.RemoveOnly;

    public bool IsHidden => (this.Flags & InventoryTabFlags.Hidden) == InventoryTabFlags.Hidden;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(", DisplayIndex: ");
      interpolatedStringHandler.AppendFormatted<ushort>(this.VisibleIndex);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted<InventoryTabType>(this.TabType);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
