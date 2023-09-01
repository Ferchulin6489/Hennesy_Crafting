// Decompiled with JetBrains decompiler
// Type: ExileCore.SoundController
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExileCore
{
  public class SoundController : IDisposable
  {
    private readonly List<SourceVoice> _list = new List<SourceVoice>();
    private readonly bool _initialized;
    private readonly MasteringVoice _masteringVoice;
    private readonly Dictionary<string, MyWave> _sounds = new Dictionary<string, MyWave>();
    private readonly string _soundsDir;
    private readonly SharpDX.XAudio2.XAudio2 _xAudio2;

    public SoundController(string dir)
    {
      this._soundsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
      if (!Directory.Exists(this._soundsDir))
      {
        this._initialized = false;
        DebugWindow.LogError("Sounds dir not found, continue working without any sound.");
      }
      else
      {
        try
        {
          this._xAudio2 = new SharpDX.XAudio2.XAudio2();
          this._xAudio2.StartEngine();
          this._masteringVoice = new MasteringVoice(this._xAudio2);
          this._initialized = true;
        }
        catch (Exception ex)
        {
          DebugWindow.LogError(ex.ToString());
        }
      }
    }

    public void Dispose()
    {
      foreach (KeyValuePair<string, MyWave> sound in this._sounds)
        sound.Value.Buffer.Stream.Dispose();
      this._xAudio2.StopEngine();
      this._masteringVoice?.Dispose();
      this._xAudio2?.Dispose();
    }

    public void PlaySound(string name)
    {
      if (!this._initialized)
        return;
      MyWave myWave = this._sounds.GetValueOrDefault<string, MyWave>(name) ?? this.LoadSound(name);
      if (myWave == null)
      {
        DebugWindow.LogError("Sound file: " + name + ".wav not found.");
      }
      else
      {
        SourceVoice sourceVoice1 = new SourceVoice(this._xAudio2, myWave.WaveFormat, true);
        sourceVoice1.SubmitSourceBuffer(myWave.Buffer, myWave.DecodedPacketsInfo);
        sourceVoice1.Start();
        this._list.Add(sourceVoice1);
        for (int index = 0; index < this._list.Count; ++index)
        {
          SourceVoice sourceVoice2 = this._list[index];
          if (sourceVoice2.State.BuffersQueued <= 0)
          {
            sourceVoice2.Stop();
            sourceVoice2.DestroyVoice();
            sourceVoice2.Dispose();
            this._list.RemoveAt(index);
          }
        }
      }
    }

    public void PreloadSound(string name)
    {
      if (!this._initialized)
        return;
      this.LoadSound(name);
    }

    private MyWave LoadSound(string name)
    {
      if (name.IndexOf(".wav", StringComparison.Ordinal) == -1)
        name = Path.Combine(this._soundsDir, name + ".wav");
      FileInfo fileInfo = new FileInfo(name);
      if (!fileInfo.Exists)
        return (MyWave) null;
      SoundStream soundStream = new SoundStream((Stream) File.OpenRead(name));
      WaveFormat format = soundStream.Format;
      AudioBuffer audioBuffer = new AudioBuffer()
      {
        Stream = soundStream.ToDataStream(),
        AudioBytes = (int) soundStream.Length,
        Flags = BufferFlags.EndOfStream
      };
      soundStream.Close();
      MyWave myWave = new MyWave()
      {
        Buffer = audioBuffer,
        WaveFormat = format,
        DecodedPacketsInfo = soundStream.DecodedPacketsInfo
      };
      this._sounds[((IEnumerable<string>) fileInfo.Name.Split('.')).First<string>()] = myWave;
      this._sounds[fileInfo.Name] = myWave;
      return myWave;
    }

    public void SetVolume(float volume)
    {
      if (!this._initialized)
        return;
      this._masteringVoice.SetVolume(volume);
    }
  }
}
