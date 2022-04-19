using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ExceptionExtension
    {
        public static void TrackError(this Exception exception)
        {
            Crashes.TrackError(exception);
        }
    }
}
