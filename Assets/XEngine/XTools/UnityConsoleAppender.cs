using log4net.Appender;
using log4net.Core;

namespace X.Tools
{
    public  class UnityConsoleAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (Level.Warn == loggingEvent.Level)
            {
                UnityEngine.Debug.LogWarning(loggingEvent.RenderedMessage);
            }
           else if (Level.Error == loggingEvent.Level)
            {
                UnityEngine.Debug.LogError(loggingEvent.RenderedMessage);
            }
           else if (Level.Fatal == loggingEvent.Level)
            {
                UnityEngine.Debug.LogException(new System.Exception( loggingEvent.RenderedMessage));
            }
           else
            {
                UnityEngine.Debug.Log(loggingEvent.RenderedMessage);
            }

        }
    }
}
