﻿// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.DelveCell
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Helpers;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.Elements
{
  public class DelveCell : Element
  {
    private DelveCellInfoStrings info;

    private NativeStringU mods => this.M.Read<NativeStringU>(this.Address + 1176L);

    public string Mods => this.mods.ToString(this.M);

    private NativeStringU mines => this.M.Read<NativeStringU>(this.M.Read<long>(this.Address + 336L) + 56L);

    public string MinesText => this.mines.ToString(this.M);

    public DelveCellInfoStrings Info => this.info = this.info ?? this.ReadObjectAt<DelveCellInfoStrings>(1600);

    public string Type => this.M.ReadStringU(this.M.Read<long>(this.Address + 1616L, new int[1]));

    public string TypeHuman => this.M.ReadStringU(this.M.Read<long>(this.Address + 1616L, 8));

    public override string Text => this.Info.TestString + " [" + this.Info.TestString5 + "]";
  }
}
