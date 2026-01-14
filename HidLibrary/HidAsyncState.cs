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
