using System;

namespace Xeovis.UI;

public class ApiActivityChangedEventArgs : EventArgs
{
    public string Message {get;set;}

    public ApiActivityChangedEventArgs(string message)
    {
        Message = message;
    }
}