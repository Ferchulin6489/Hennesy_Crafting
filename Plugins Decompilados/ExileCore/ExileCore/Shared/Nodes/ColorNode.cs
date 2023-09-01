// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.ColorNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.Nodes
{
  public sealed class ColorNode
  {
    private SharpDX.Color _value;

    public ColorNode()
    {
    }

    public ColorNode(uint color) => this.Value = SharpDX.Color.FromAbgr(color);

    public ColorNode(SharpDX.Color color) => this.Value = color;

    public string Hex { get; private set; }

    public SharpDX.Color Value
    {
      get => this._value;
      set
      {
        if (!(this._value != value))
          return;
        this.Hex = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb((int) value.A, (int) value.R, (int) value.G, (int) value.B));
        this._value = value;
        try
        {
          EventHandler<SharpDX.Color> onValueChanged = this.OnValueChanged;
          if (onValueChanged == null)
            return;
          onValueChanged((object) this, value);
        }
        catch (Exception ex)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 3);
          interpolatedStringHandler.AppendLiteral("Error in function that subscribed for: ");
          interpolatedStringHandler.AppendFormatted(nameof (ColorNode));
          interpolatedStringHandler.AppendLiteral(".");
          interpolatedStringHandler.AppendFormatted("OnValueChanged");
          interpolatedStringHandler.AppendLiteral(": ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 10f, SharpDX.Color.Red);
        }
      }
    }

    public static implicit operator SharpDX.Color(ColorNode node) => node.Value;

    public static implicit operator ColorNode(uint value) => new ColorNode(value);

    public static implicit operator ColorNode(SharpDX.Color value) => new ColorNode(value);

    public static implicit operator ColorNode(ColorBGRA value) => new ColorNode((SharpDX.Color) value);

    public event EventHandler<SharpDX.Color> OnValueChanged;
  }
}
