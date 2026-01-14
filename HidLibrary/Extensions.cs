// Decompiled with JetBrains decompiler
// Type: HidLibrary.Extensions
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System.Text;

#nullable disable
namespace HidLibrary;

public static class Extensions
{
  public static string ToUTF8String(this byte[] buffer)
  {
    string str = Encoding.UTF8.GetString(buffer);
    return str.Remove(str.IndexOf(char.MinValue));
  }

  public static string ToUTF16String(this byte[] buffer)
  {
    string str = Encoding.Unicode.GetString(buffer);
    return str.Remove(str.IndexOf(char.MinValue));
  }
}
