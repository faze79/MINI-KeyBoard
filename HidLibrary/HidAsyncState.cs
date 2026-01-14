// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidAsyncState
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

#nullable disable
namespace HidLibrary;

public class HidAsyncState
{
  private readonly object _callerDelegate;
  private readonly object _callbackDelegate;

  public HidAsyncState(object callerDelegate, object callbackDelegate)
  {
    this._callerDelegate = callerDelegate;
    this._callbackDelegate = callbackDelegate;
  }

  public object CallerDelegate => this._callerDelegate;

  public object CallbackDelegate => this._callbackDelegate;
}
