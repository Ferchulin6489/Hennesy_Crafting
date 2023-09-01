// Decompiled with JetBrains decompiler
// Type: ExileCore.RenderQ.ImGuiRender
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ClickableTransparentOverlay;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using Serilog;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace ExileCore.RenderQ
{
  public class ImGuiRender
  {
    private readonly 
    #nullable disable
    ActionOverlay _overlay;
    private const string DefaultFontName = "Default:13";
    [Obsolete]
    public static readonly ImGuiWindowFlags InvisibleWindow = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings;
    private ImDrawListPtr _backGroundTextWindowPtr;
    private ImDrawListPtr _backGroundWindowPtr;
    private readonly DX11 _dx11;
    private FontContainer _lastFontContainer;

    public ImGuiRender(DX11 dx11, ActionOverlay overlay, CoreSettings coreSettings)
    {
      this._overlay = overlay;
      this._dx11 = dx11;
      this.CoreSettings = coreSettings;
      this.Initialize();
    }

    [Obsolete]
    public DX11 Dx11 => this._dx11;

    public CoreSettings CoreSettings { get; }

    [Obsolete]
    public ImGuiIOPtr io => ImGui.GetIO();

    public Dictionary<string, FontContainer> fonts { get; } = new Dictionary<string, FontContainer>();

    public ImDrawListPtr LowLevelApi => this._backGroundWindowPtr;

    public float TextScale { get; set; } = 1f;

    public FontContainer CurrentFont
    {
      get
      {
        FontContainer currentFont;
        if (this.fonts.TryGetValue(this.CoreSettings.Font.Value ?? "", out currentFont) || this.fonts.TryGetValue("Default:13", out currentFont))
          return currentFont;
        return !this.fonts.Any<KeyValuePair<string, FontContainer>>() ? this._lastFontContainer : this.fonts.First<KeyValuePair<string, FontContainer>>().Value;
      }
    }

    private void Initialize()
    {
      this.CoreSettings.FontGlyphRange.OnValueSelected += (Action<string>) (_ => this.SetManualFont());
      try
      {
        using (new PerformanceTimer("Load manual fonts"))
        {
          try
          {
            this.SetManualFont();
          }
          catch (Exception ex)
          {
            ILogger logger = Core.Logger;
            if (logger == null)
              return;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
            interpolatedStringHandler.AppendLiteral("Cant load fonts -> ");
            interpolatedStringHandler.AppendFormatted<Exception>(ex);
            logger.Error(interpolatedStringHandler.ToStringAndClear());
          }
        }
      }
      catch (DllNotFoundException ex)
      {
        throw new DllNotFoundException("Need put in directory cimgui.dll");
      }
    }

    private unsafe void SetManualFont() => this._overlay.ReplaceFont((Overlay.FontLoadDelegate) (config =>
    {
      ImGuiIOPtr io = ImGui.GetIO();
      this.fonts["Default:13"] = new FontContainer((ImFont*) io.Fonts.AddFontDefault((ImFontConfigPtr) config), "Default", 13);
      if (Directory.Exists("fonts") && ((IEnumerable<string>) Directory.GetFiles("fonts")).Contains<string>("fonts\\config.ini"))
      {
        string[] strArray1 = File.ReadAllLines("fonts\\config.ini");
        IntPtr glyphRange = ImGuiRender.GetGlyphRange(this.CoreSettings.FontGlyphRange.Value);
        foreach (string str1 in strArray1)
        {
          string[] strArray2 = str1.Split(':');
          string str2 = "fonts\\" + strArray2[0] + ".ttf";
          int num = int.Parse(strArray2[1]);
          DefaultInterpolatedStringHandler interpolatedStringHandler;
          if (!File.Exists(str2))
          {
            interpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
            interpolatedStringHandler.AppendLiteral("Font ");
            interpolatedStringHandler.AppendFormatted(str2);
            interpolatedStringHandler.AppendLiteral(" specified in ");
            interpolatedStringHandler.AppendFormatted("fonts\\config.ini");
            interpolatedStringHandler.AppendLiteral(" does not exist");
            DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
          }
          else
          {
            Dictionary<string, FontContainer> fonts = this.fonts;
            interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
            interpolatedStringHandler.AppendFormatted(str2.Replace(".ttf", "").Replace("fonts\\", ""));
            interpolatedStringHandler.AppendLiteral(":");
            interpolatedStringHandler.AppendFormatted<int>(num);
            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
            FontContainer fontContainer = new FontContainer((ImFont*) io.Fonts.AddFontFromFileTTF(str2, (float) num, (ImFontConfigPtr) config, glyphRange), str2, num);
            fonts[stringAndClear] = fontContainer;
          }
        }
      }
      this._lastFontContainer = this.fonts.First<KeyValuePair<string, FontContainer>>().Value;
      this.CoreSettings.Font.Values = this.fonts.Keys.ToList<string>();
      if (this.CoreSettings.Font.Value != null && this.fonts.ContainsKey(this.CoreSettings.Font.Value))
        return;
      this.CoreSettings.Font.Value = this.CoreSettings.Font.Values.First<string>();
    }));

    private static IntPtr GetGlyphRange(string value)
    {
      ImGuiIOPtr io = ImGui.GetIO();
      FontGlyphRangeType result;
      IntPtr glyphRange;
      switch (Enum.TryParse<FontGlyphRangeType>(value, out result) ? (int) result : 7)
      {
        case 0:
          glyphRange = io.Fonts.GetGlyphRangesDefault();
          break;
        case 1:
          glyphRange = io.Fonts.GetGlyphRangesChineseSimplifiedCommon();
          break;
        case 2:
          glyphRange = io.Fonts.GetGlyphRangesChineseFull();
          break;
        case 3:
          glyphRange = io.Fonts.GetGlyphRangesJapanese();
          break;
        case 4:
          glyphRange = io.Fonts.GetGlyphRangesKorean();
          break;
        case 5:
          glyphRange = io.Fonts.GetGlyphRangesThai();
          break;
        case 6:
          glyphRange = io.Fonts.GetGlyphRangesVietnamese();
          break;
        case 7:
          glyphRange = io.Fonts.GetGlyphRangesCyrillic();
          break;
        default:
          glyphRange = io.Fonts.GetGlyphRangesCyrillic();
          break;
      }
      return glyphRange;
    }

    public void BeginBackGroundWindow()
    {
      ImGuiIOPtr io = ImGui.GetIO();
      ImGui.SetNextWindowContentSize(io.DisplaySize);
      ImGui.SetNextWindowPos(new System.Numerics.Vector2(0.0f, 0.0f));
      ImGui.Begin("Background Window", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoBringToFrontOnFocus);
      this._backGroundWindowPtr = ImGui.GetWindowDrawList();
      ImGui.End();
      ImGui.SetNextWindowContentSize(io.DisplaySize);
      ImGui.SetNextWindowPos(new System.Numerics.Vector2(0.0f, 0.0f));
      ImGui.Begin("Background Text Window", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing);
      this._backGroundTextWindowPtr = ImGui.GetWindowDrawList();
      ImGui.End();
    }

    public System.Numerics.Vector2 MeasureText(string text) => ImGui.CalcTextSize(text) * this.TextScale;

    public System.Numerics.Vector2 MeasureText(string text, int height) => ImGui.CalcTextSize(text) * this.TextScale;

    public unsafe System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      int height,
      string fontName,
      FontAlign align)
    {
      try
      {
        int num = fontName != "" ? 1 : 0;
        FontContainer container;
        if (fontName == null)
        {
          container = this._lastFontContainer = this.CurrentFont;
        }
        else
        {
          if (!this.fonts.TryGetValue(fontName, out container))
          {
            container = this.fonts.First<KeyValuePair<string, FontContainer>>().Value;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
            interpolatedStringHandler.AppendLiteral("Font: ");
            interpolatedStringHandler.AppendFormatted(fontName);
            interpolatedStringHandler.AppendLiteral(" not found. Using: ");
            interpolatedStringHandler.AppendFormatted(container.Name);
            interpolatedStringHandler.AppendLiteral(":");
            interpolatedStringHandler.AppendFormatted<int>(container.Size);
            DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
          }
          this._lastFontContainer = container;
        }
        using (num != 0 ? this.UseFont(container) : (IDisposable) null)
        {
          System.Numerics.Vector2 vector2 = this.MeasureText(text);
          if ((align & FontAlign.Center) != FontAlign.Left)
            position.X -= vector2.X / 2f;
          if ((align & FontAlign.VerticalCenter) != FontAlign.Left)
            position.Y -= vector2.Y / 2f;
          if ((align & FontAlign.Top) != FontAlign.Left)
            position.Y -= vector2.Y;
          if ((align & FontAlign.Right) != FontAlign.Left)
            position.X -= vector2.X;
          this._backGroundTextWindowPtr.AddText((ImFontPtr) container.Atlas, (float) container.Size * this.TextScale, position, color.ToImgui(), text);
          return vector2;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        throw;
      }
    }

    public void DrawImage(string fileName, RectangleF rectangle, RectangleF uv) => this.DrawImage(fileName, rectangle, uv, Color.White);

    public void DrawImage(string fileName, RectangleF rectangle, RectangleF uv, Color color) => this._backGroundTextWindowPtr.AddImage(this._dx11.GetTexture(fileName), rectangle.TopLeft.ToVector2Num(), rectangle.BottomRight.ToVector2Num(), uv.TopLeft.ToVector2Num(), uv.BottomRight.ToVector2Num(), color.ToImgui());

    public void DrawImage(
      string filename,
      System.Numerics.Vector2 TopLeft,
      System.Numerics.Vector2 BottomRight,
      System.Numerics.Vector2 TopLeft_UV,
      System.Numerics.Vector2 BottomRight_UV)
    {
      this._backGroundTextWindowPtr.AddImage(this._dx11.GetTexture(filename), TopLeft, BottomRight, TopLeft_UV, BottomRight_UV);
    }

    [Description("Count Colors means how many colors used in text, if you use a lot colors need put number more than colors you have.This used for optimization.")]
    public unsafe System.Numerics.Vector2 DrawMultiColoredText(
      string text,
      System.Numerics.Vector2 position,
      FontAlign align = FontAlign.Left,
      int countColors = 10)
    {
      ReadOnlySpan<char> span1 = text.AsSpan();
      int num1 = 0;
      int length1 = countColors;
      // ISSUE: untyped stack allocation
      Span<uint> span2 = new Span<uint>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length1) * 4)), length1);
      int length2 = countColors * 2 + 1;
      // ISSUE: untyped stack allocation
      Span<int> span3 = new Span<int>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length2) * 4)), length2);
      int num2 = 0;
      int num3 = 0;
      for (int index1 = 0; index1 < text.Length; ++index1)
      {
        if (text[index1] == '#' && index1 + 10 < text.Length && text[index1 + 9] == '#')
        {
          span2[num3++] = span1.Slice(index1 + 1, 8).HexToUInt();
          if (index1 != 0 && num1 == 0)
          {
            ref Span<int> local1 = ref span3;
            int index2 = num2;
            int num4 = index2 + 1;
            local1[index2] = num1;
            ref Span<int> local2 = ref span3;
            int index3 = num4;
            int num5 = index3 + 1;
            local2[index3] = index1;
            num1 = index1 + 10;
            ref Span<int> local3 = ref span3;
            int index4 = num5;
            num2 = index4 + 1;
            local3[index4] = num1;
            index1 += 9;
          }
          else
          {
            if (index1 != 0)
              span3[num2++] = index1 - num1;
            num1 = index1 + 10;
            index1 += 9;
            span3[num2++] = num1;
          }
        }
      }
      ref Span<int> local = ref span3;
      int index5 = num2;
      int num6 = index5 + 1;
      local[index5] = text.Length - num1;
      int num7 = (int) Math.Ceiling((double) num6 / (double) num3);
      using (this.UseCurrentFont())
      {
        if (align == FontAlign.Center)
          position.X -= this.MeasureText(text, this.CurrentFont.Size).X / 4f;
        float x = position.X;
        if (num7 == 2)
        {
          int num8 = 0;
          for (int index6 = 0; index6 < num6; index6 += 2)
          {
            int start = span3[index6];
            int len = span3[index6 + 1];
            uint clr = span2[num8++];
            this.DrawClrText2(ref span1, ref position, x, align, start, len, clr);
          }
        }
        else
        {
          if (num7 < 3)
            throw new Exception("Something wrong");
          int num9 = 0;
          for (int index7 = 0; index7 < num6; index7 += 2)
          {
            int start = span3[index7];
            int len = span3[index7 + 1];
            if (index7 == 0)
            {
              this.DrawClrText2(ref span1, ref position, x, align, start, len, Color.White.ToImgui());
            }
            else
            {
              uint clr = span2[num9++];
              this.DrawClrText2(ref span1, ref position, x, align, start, len, clr);
            }
          }
        }
        return position;
      }
    }

    private unsafe System.Numerics.Vector2 DrawClrText2(
      ref ReadOnlySpan<char> span,
      ref System.Numerics.Vector2 position,
      float xStart,
      FontAlign align,
      int start,
      int len,
      uint clr)
    {
      string str = span.Slice(start, len).ToString();
      FontContainer currentFont = this.CurrentFont;
      System.Numerics.Vector2 vector2 = this.MeasureText(str, currentFont.Size);
      switch (align)
      {
        case FontAlign.Left:
          this._backGroundWindowPtr.AddText((ImFontPtr) currentFont.Atlas, (float) currentFont.Size, position, clr, str);
          position.X += vector2.X;
          break;
        case FontAlign.Center:
          this._backGroundWindowPtr.AddText((ImFontPtr) currentFont.Atlas, (float) currentFont.Size, position, clr, str);
          position.X += vector2.X;
          break;
        case FontAlign.Right:
          position.X -= vector2.X;
          this._backGroundWindowPtr.AddText((ImFontPtr) currentFont.Atlas, (float) currentFont.Size, position, clr, str);
          break;
      }
      if (str[len - 1] == '\n')
      {
        position.X = xStart;
        position.Y += vector2.Y;
      }
      return vector2;
    }

    [Description("Count Colors means how many colors used in text, if you use a lot colors need put number more than colors you have.This used for optimization.")]
    public unsafe void MultiColoredText(string text, int countColors = 10)
    {
      ReadOnlySpan<char> span1 = text.AsSpan();
      int num1 = 0;
      int length1 = countColors;
      // ISSUE: untyped stack allocation
      Span<uint> span2 = new Span<uint>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length1) * 4)), length1);
      int length2 = countColors * 2 + 1;
      // ISSUE: untyped stack allocation
      Span<int> span3 = new Span<int>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length2) * 4)), length2);
      int num2 = 0;
      int num3 = 0;
      for (int index1 = 0; index1 < text.Length; ++index1)
      {
        if (text[index1] == '#' && index1 + 10 < text.Length && text[index1 + 9] == '#')
        {
          span2[num3++] = span1.Slice(index1 + 1, 8).HexToUInt();
          if (index1 != 0 && num1 == 0)
          {
            ref Span<int> local1 = ref span3;
            int index2 = num2;
            int num4 = index2 + 1;
            local1[index2] = num1;
            ref Span<int> local2 = ref span3;
            int index3 = num4;
            int num5 = index3 + 1;
            local2[index3] = index1;
            num1 = index1 + 10;
            ref Span<int> local3 = ref span3;
            int index4 = num5;
            num2 = index4 + 1;
            local3[index4] = num1;
            index1 += 9;
          }
          else
          {
            if (index1 != 0)
              span3[num2++] = index1 - num1;
            num1 = index1 + 10;
            index1 += 9;
            span3[num2++] = num1;
          }
        }
      }
      ref Span<int> local = ref span3;
      int index5 = num2;
      int spanIndex = index5 + 1;
      local[index5] = text.Length - num1;
      switch ((int) Math.Ceiling((double) spanIndex / (double) num3))
      {
        case 2:
          int num6 = 0;
          for (int index6 = 0; index6 < spanIndex; index6 += 2)
          {
            int start = span3[index6];
            int len = span3[index6 + 1];
            uint color = span2[num6++];
            this.DrawClrText(ref span1, start, len, Color.FromRgba(color), index6, spanIndex);
          }
          break;
        case 3:
          int num7 = 0;
          for (int index7 = 0; index7 < spanIndex; index7 += 2)
          {
            int start = span3[index7];
            int len = span3[index7 + 1];
            if (index7 == 0)
            {
              this.DrawClrText(ref span1, start, len, Color.Transparent, index7, spanIndex, true);
            }
            else
            {
              uint color = span2[num7++];
              this.DrawClrText(ref span1, start, len, Color.FromRgba(color), index7, spanIndex);
            }
          }
          break;
        default:
          throw new Exception("Something wrong");
      }
    }

    private unsafe void DrawClrText(
      ref ReadOnlySpan<char> span,
      int start,
      int len,
      Color clr,
      int index,
      int spanIndex,
      bool noColor = false)
    {
      ReadOnlySpan<char> readOnlySpan = span.Slice(start, len);
      fixed (char* chars = &readOnlySpan.GetPinnableReference())
      {
        byte* numPtr = stackalloc byte[readOnlySpan.Length];
        Encoding.UTF8.GetBytes(chars, readOnlySpan.Length, numPtr, readOnlySpan.Length);
        if (noColor)
          ImGuiNative.igText(numPtr);
        else
          ImGuiNative.igTextColored(clr.ToImguiVec4(), numPtr);
        if (index + 2 < spanIndex)
        {
          if (chars[len - 1] == '\n')
            return;
          ImGuiNative.igSameLine(0.0f, 0.0f);
        }
      }
    }

    public IDisposable UseCurrentFont() => this.UseFont(this.CurrentFont);

    private unsafe IDisposable UseFont(FontContainer container)
    {
      this._lastFontContainer = container;
      ImGui.PushFont((ImFontPtr) container.Atlas);
      return (IDisposable) new ImGuiRender.PopFont();
    }

    private record PopFont() : IDisposable
    {
      public void Dispose() => ImGui.PopFont();
    }
  }
}
