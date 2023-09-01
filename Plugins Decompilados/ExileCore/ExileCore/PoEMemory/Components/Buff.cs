// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Buff
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class Buff : RemoteMemoryObject
  {
    private readonly CachedValue<BuffOffsets> _cachedValue;
    private BuffDefinition _buffDefinition;

    public Buff() => this._cachedValue = (CachedValue<BuffOffsets>) new FramesCache<BuffOffsets>((Func<BuffOffsets>) (() => this.M.Read<BuffOffsets>(this.Address)), 3U);

    public BuffOffsets BuffOffsets => this._cachedValue.Value;

    public string Name => this.BuffDefinition?.Id ?? string.Empty;

    [Obsolete("Use BuffCharges instead")]
    public byte Charges => (byte) this.BuffOffsets.Charges;

    public ushort BuffCharges => this.BuffOffsets.Charges;

    public ushort BuffStacks => this.M.Read<ushort>(this.M.Read<long>(this.Address + 248L));

    public string DisplayName => this.BuffDefinition?.Name ?? string.Empty;

    public string Description => this.BuffDefinition?.Description ?? string.Empty;

    public bool IsInvisible
    {
      get
      {
        BuffDefinition buffDefinition = this.BuffDefinition;
        return buffDefinition != null && buffDefinition.IsInvisible;
      }
    }

    public bool IsRemovable
    {
      get
      {
        BuffDefinition buffDefinition = this.BuffDefinition;
        return buffDefinition != null && buffDefinition.IsRemovable;
      }
    }

    public BuffDefinition BuffDefinition
    {
      [return: MaybeNull] get => this._buffDefinition ?? (this._buffDefinition = this.TheGame.Files.BuffDefinitions.GetByAddress(this.BuffOffsets.BuffDatPtr));
    }

    public float MaxTime => this.BuffOffsets.MaxTime;

    public float Timer => this.BuffOffsets.Timer;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 5);
      interpolatedStringHandler.AppendFormatted(this.DisplayName);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(") - Charges: ");
      interpolatedStringHandler.AppendFormatted<ushort>(this.BuffCharges);
      interpolatedStringHandler.AppendLiteral(" MaxTime: ");
      interpolatedStringHandler.AppendFormatted<float>(this.MaxTime);
      interpolatedStringHandler.AppendLiteral(", BuffStacks: ");
      interpolatedStringHandler.AppendFormatted<ushort>(this.BuffStacks);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
