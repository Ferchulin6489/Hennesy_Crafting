// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Component
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System.Reflection;
using System.Text;

namespace ExileCore.PoEMemory
{
  public class Component : RemoteMemoryObject
  {
    public long OwnerAddress => this.M.Read<long>(this.Address + 8L);

    public Entity Owner => this.ReadObject<Entity>(this.Address + 8L);

    public string DumpObject()
    {
      PropertyInfo[] properties = this.GetType().GetProperties();
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (PropertyInfo propertyInfo in properties)
      {
        object obj = propertyInfo.GetValue((object) this, (object[]) null);
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
        if (obj is RemoteMemoryObject)
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          StringBuilder stringBuilder3 = stringBuilder2;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder2);
          interpolatedStringHandler.AppendFormatted(propertyInfo.Name);
          interpolatedStringHandler.AppendLiteral(" => ");
          interpolatedStringHandler.AppendFormatted<object>(obj);
          ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
          stringBuilder3.AppendLine(ref local1);
          StringBuilder stringBuilder4 = stringBuilder1;
          StringBuilder stringBuilder5 = stringBuilder4;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder4);
          interpolatedStringHandler.AppendLiteral("ToString => ");
          interpolatedStringHandler.AppendFormatted<object>(obj.GetType().GetMethod("ToString").Invoke(obj, (object[]) null));
          ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
          stringBuilder5.AppendLine(ref local2);
        }
        else
        {
          StringBuilder stringBuilder6 = stringBuilder1;
          StringBuilder stringBuilder7 = stringBuilder6;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder6);
          interpolatedStringHandler.AppendFormatted(propertyInfo.Name);
          interpolatedStringHandler.AppendLiteral(" => ");
          interpolatedStringHandler.AppendFormatted<object>(obj);
          ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
          stringBuilder7.AppendLine(ref local);
        }
      }
      return stringBuilder1.ToString();
    }
  }
}
