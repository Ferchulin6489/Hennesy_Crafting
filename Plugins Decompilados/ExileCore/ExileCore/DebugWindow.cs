// Decompiled with JetBrains decompiler
// Type: ExileCore.DebugWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using Serilog;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore
{
  public class DebugWindow
  {
    private static readonly object locker = new object();
    private static readonly Dictionary<string, DebugMsgDescription> Messages = new Dictionary<string, DebugMsgDescription>(24);
    private static readonly List<DebugMsgDescription> MessagesList = new List<DebugMsgDescription>(24);
    private static readonly Queue<string> toDelete = new Queue<string>(24);
    private static readonly Queue<DebugMsgDescription> LogHistory = new Queue<DebugMsgDescription>(1024);
    private static readonly CircularBuffer<DebugMsgDescription> History = new CircularBuffer<DebugMsgDescription>(1024);
    private readonly Graphics graphics;
    private readonly CoreSettings settingsCoreSettings;
    private System.Numerics.Vector2 position;

    public DebugWindow(Graphics graphics, CoreSettings settingsCoreSettings)
    {
      this.graphics = graphics;
      this.settingsCoreSettings = settingsCoreSettings;
      graphics.InitImage("menu-background.png");
    }

    public void Render()
    {
      if ((bool) this.settingsCoreSettings.HideAllDebugging)
        return;
      try
      {
        if ((bool) this.settingsCoreSettings.ShowLogWindow)
        {
          using (this.graphics.UseCurrentFont())
          {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(10f, 10f), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(600f, 1000f), ImGuiCond.Once);
            bool p_open = this.settingsCoreSettings.ShowLogWindow.Value;
            ImGui.Begin("Debug log", ref p_open);
            this.settingsCoreSettings.ShowLogWindow.Value = p_open;
            foreach (DebugMsgDescription debugMsgDescription in DebugWindow.History)
            {
              if (debugMsgDescription != null)
              {
                ImGui.PushStyleColor(ImGuiCol.Text, debugMsgDescription.ColorV4);
                ImGui.TextUnformatted(debugMsgDescription.Time.ToLongTimeString() + ": " + debugMsgDescription.Msg);
                ImGui.PopStyleColor();
              }
            }
            ImGui.End();
          }
        }
        if (DebugWindow.MessagesList.Count == 0)
          return;
        this.position = new System.Numerics.Vector2(10f, 35f);
        for (int index = 0; index < DebugWindow.MessagesList.Count; ++index)
        {
          DebugMsgDescription messages = DebugWindow.MessagesList[index];
          if (messages != null)
          {
            if (messages.Time < DateTime.UtcNow)
            {
              DebugWindow.toDelete.Enqueue(messages.Msg);
            }
            else
            {
              string text = messages.Msg;
              if (messages.Count > 1)
              {
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
                interpolatedStringHandler.AppendLiteral("(");
                interpolatedStringHandler.AppendFormatted<int>(messages.Count);
                interpolatedStringHandler.AppendLiteral(")");
                interpolatedStringHandler.AppendFormatted(text);
                text = interpolatedStringHandler.ToStringAndClear();
              }
              System.Numerics.Vector2 vector2 = this.graphics.MeasureText(text);
              this.graphics.DrawImage("menu-background.png", new RectangleF(this.position.X - 5f, this.position.Y, vector2.X + 20f, vector2.Y));
              this.graphics.DrawText(text, this.position, messages.Color);
              this.position = new System.Numerics.Vector2(this.position.X, this.position.Y + vector2.Y);
            }
          }
        }
        while (DebugWindow.toDelete.Count > 0)
        {
          string key = DebugWindow.toDelete.Dequeue();
          DebugMsgDescription debugMsgDescription;
          if (DebugWindow.Messages.TryGetValue(key, out debugMsgDescription))
          {
            DebugWindow.MessagesList.Remove(debugMsgDescription);
            DebugWindow.LogHistory.Enqueue(debugMsgDescription);
            DebugWindow.History.PushBack(debugMsgDescription);
            if (debugMsgDescription.Color == Color.Red)
              Core.Logger.Error(debugMsgDescription.Msg ?? "");
            else
              Core.Logger.Information(debugMsgDescription.Msg ?? "");
          }
          DebugWindow.Messages.Remove(key);
          if (DebugWindow.LogHistory.Count >= 1024)
          {
            for (int index = 0; index < 24; ++index)
              DebugWindow.LogHistory.Dequeue();
          }
        }
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public static void LogMsg(string msg) => DebugWindow.LogMsg(msg, 1f, Color.White);

    public static void LogError(string msg, float time = 2f) => DebugWindow.LogMsg(msg, time, Color.Red);

    public static void LogMsg(string msg, float time) => DebugWindow.LogMsg(msg, time, Color.White);

    public static void LogMsg(string msg, float time, Color color)
    {
      try
      {
        DebugMsgDescription debugMsgDescription1;
        if (DebugWindow.Messages.TryGetValue(msg, out debugMsgDescription1))
        {
          debugMsgDescription1.Time = DateTime.UtcNow.AddSeconds((double) time);
          debugMsgDescription1.Color = color;
          ++debugMsgDescription1.Count;
        }
        else
        {
          DebugMsgDescription debugMsgDescription2 = new DebugMsgDescription()
          {
            Msg = msg,
            Time = DateTime.UtcNow.AddSeconds((double) time),
            ColorV4 = color.ToImguiVec4(),
            Color = color,
            Count = 1
          };
          lock (DebugWindow.locker)
          {
            DebugWindow.Messages[msg] = debugMsgDescription2;
            DebugWindow.MessagesList.Add(debugMsgDescription2);
          }
        }
      }
      catch (Exception ex)
      {
        ILogger logger = Core.Logger;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
        interpolatedStringHandler.AppendFormatted(nameof (DebugWindow));
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        logger.Error(stringAndClear);
      }
    }
  }
}
