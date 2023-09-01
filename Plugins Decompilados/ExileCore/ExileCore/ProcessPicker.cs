// Decompiled with JetBrains decompiler
// Type: ExileCore.ProcessPicker
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;


#nullable enable
namespace ExileCore
{
  public class ProcessPicker
  {
    public static int? ShowDialogBox(
    #nullable disable
    IEnumerable<Process> processes)
    {
      Form form1 = new Form();
      form1.Text = "Pick PoE process to attach to";
      form1.FormBorderStyle = FormBorderStyle.FixedDialog;
      form1.StartPosition = FormStartPosition.CenterScreen;
      form1.MinimizeBox = false;
      form1.MaximizeBox = false;
      using (Form form2 = form1)
      {
        Button button1 = new Button();
        button1.Text = "Exit";
        button1.DialogResult = DialogResult.Cancel;
        Button button2 = button1;
        Button button3 = new Button();
        button3.Text = "Wait";
        button3.DialogResult = DialogResult.Retry;
        Button button4 = button3;
        List<Button> list = processes.Select<Process, Button>((Func<Process, Button>) (p =>
        {
          Button button5 = new Button();
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 3);
          interpolatedStringHandler.AppendLiteral("Process #");
          interpolatedStringHandler.AppendFormatted<int>(p.Id);
          interpolatedStringHandler.AppendLiteral(" (");
          interpolatedStringHandler.AppendFormatted(p.ProcessName);
          interpolatedStringHandler.AppendLiteral("), started at ");
          interpolatedStringHandler.AppendFormatted(p.StartTime.ToLongTimeString());
          button5.Text = interpolatedStringHandler.ToStringAndClear();
          button5.DialogResult = DialogResult.OK;
          return button5;
        })).ToList<Button>();
        int x = 10;
        int y1 = x;
        int height = 23;
        int width = 300;
        button4.SetBounds(x, y1, width, height);
        int y2 = y1 + height;
        button2.SetBounds(x, y2, width, height);
        int y3 = y2 + height;
        int? selectedProcessIndex = new int?();
        foreach ((Button button6, int num) in list.Select<Button, (Button, int)>((Func<Button, int, (Button, int)>) ((b, i) => (b, i))))
        {
          button6.SetBounds(x, y3, width, height);
          button6.Click += (EventHandler) ((_1, _2) => selectedProcessIndex = new int?(num));
          y3 += height;
        }
        form2.ClientSize = new Size(width + x * 2, y3 + x);
        form2.Controls.AddRange(((IEnumerable<Control>) new Control[2]
        {
          (Control) button4,
          (Control) button2
        }).Concat<Control>((IEnumerable<Control>) list).ToArray<Control>());
        form2.CancelButton = (IButtonControl) button4;
        return form2.ShowDialog() == DialogResult.Retry ? new int?(-1) : selectedProcessIndex;
      }
    }
  }
}
