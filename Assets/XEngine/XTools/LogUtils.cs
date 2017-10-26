using System;
using log4net;

//直接支持的字符颜色
/***********************************
Color name  Hex value   Swatch
aqua(same as cyan) 同青色	#00ffffff	
black 黑色	#000000ff	
blue 蓝色	#0000ffff	
brown 棕色	#a52a2aff	
cyan(same as aqua) 青色	#00ffffff	
darkblue 深蓝色	#0000a0ff	
fuchsia(same as magenta) 紫红色（同洋红）	#ff00ffff	
green 绿色	#008000ff	
grey 灰色	#808080ff	
lightblue 浅蓝色	#add8e6ff	
lime 青橙绿	#00ff00ff	
magenta(same as fuchsia) 洋红色（同紫红色）	#ff00ffff	
maroon 褐红色	#800000ff	
navy 海军蓝	#000080ff	
olive 橄榄色	#808000ff	
orange 橙黄色	#ffa500ff	
purple 紫色	#800080ff	
red 红色	#ff0000ff	
silver 银灰色	#c0c0c0ff	
teal 蓝绿色	#008080ff	
white 白色	#ffffffff	
yellow 黄色	#ffff00ff	

************************************/



namespace X.Tools
{
    public enum LogTag
    {
        Default = 0,
        UI,
        Res,
        Net,
        Pay,
        Update,
        Test,
        Sync,
        AI,
    }
    struct Tag
    {

        public int Id;
        public int Size;
        public string Name;
        public string Color;

        public Tag(int id, string name, string color = null, int size = -1)
        {
            Id = id;
            Size = size;
            Name = name;
            Color = color;
        }
    }

    class LogUtils
    {

        public static Tag[] tag = new Tag[]
        {
            new Tag((int)LogTag.Default, "Default", "white", 10),
            new Tag((int)LogTag.Net, "Net", "brown", 12),
            new Tag((int)LogTag.Pay, "Pay", "orange", 16),
            new Tag((int)LogTag.Res, "Res", "green", 10),
            new Tag((int)LogTag.Test, "Test", "aqua", 10),
            new Tag((int)LogTag.UI, "UI", "yellow", 10),
            new Tag((int)LogTag.Update, "Update", "blue", 10),
            new Tag((int)LogTag.Sync, "Sync", "olive", 10),
            new Tag((int)LogTag.AI, "AI", "purple", 10),
        };

        private static Tag GetTag(LogTag logTag)
        {
            for (int i = 0; i < tag.Length; i++)
            {
                if ((int)logTag == tag[i].Id)
                {
                    return tag[i];
                }
            }

            return tag[0];
        }
        //后期可以通过标签过滤不需要的打出的日志
        private static readonly ILog log;
        static LogUtils()
        {
            UnityEngine.TextAsset configText = UnityEngine.Resources.Load("log4net") as UnityEngine.TextAsset;
            if (configText != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(configText.bytes);

                log4net.Config.XmlConfigurator.Configure(ms);
                
            }

            log = LogManager.GetLogger("Unity");
            
        }

        //尽可能早初始化
        public static void Init()
        {

        }

        public static void Debug(LogTag tag, string message)
        {
            Tag t = GetTag(tag);
            message = string.Format("<color={0}><size={1}><b>[{2}]:{3}</b></size></color>", t.Color, t.Size, t.Name, message);
            if (log.IsDebugEnabled)
                log.Debug(message);
        }

        public static void Debug(object message)
        {
            if (log.IsDebugEnabled)
                log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
            if (log.IsDebugEnabled)
                log.Debug(message, exception);
        }

        public static void Debug(string format, params object[] args)
        {

            if (log.IsDebugEnabled)
                log.DebugFormat(format, args);
        }

        public static void Debug(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(provider, format, args);
        }

        public static void Error(LogTag tag, string message)
        {
            Tag t = GetTag(tag);
            message = string.Format("<color={0}><size={1}><b>[{2}]:{3}</b></size></color>", t.Color, t.Size, t.Name, message);

            if (log.IsErrorEnabled)
                log.Error(message);
        }

        public static void Error(object message)
        {
            if (log.IsErrorEnabled)
               log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            if (log.IsErrorEnabled)
                log.Error(message, exception);
        }
        public static void Error(string format, params object[] args)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, args);
        }

        public static void Error(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(provider, format, args);
        }

        public static void Fatal(LogTag tag, string message)
        {
            Tag t = GetTag(tag);
            message = string.Format("<color={0}><size={1}><b>[{2}]:{3}</b></size></color>", t.Color, t.Size, t.Name, message);

            if (log.IsFatalEnabled)
                log.Fatal(message);
        }

        public static void Fatal(object message)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message, exception);
        }

        public static void Fatal(string format, params object[] args)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(format, args);
        }

        public static void Fatal(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(provider, format, args);
        }

        public static void Info(LogTag tag, string message)
        {
            Tag t = GetTag(tag);
            message = string.Format("<color={0}><size={1}><b>[{2}]:{3}</b></size></color>", t.Color, t.Size, t.Name, message);

            if (log.IsInfoEnabled)
                log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            if (log.IsInfoEnabled)
                log.Info(message, exception);
        }

        public static void Info(object message)
        {
            if (log.IsInfoEnabled)
               log.Info(message);
        }

        public static void Info(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(provider, format, args);
        }

        public static void Info(string format, params object[] args)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(format, args);
        }

        public static void Warn(LogTag tag, string message)
        {
            Tag t = GetTag(tag);
            message = string.Format("<color={0}><size={1}><b>[{2}]:{3}</b></size></color>", t.Color, t.Size, t.Name, message);

            if (log.IsWarnEnabled)
                log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            if (log.IsWarnEnabled)
                log.Warn(message, exception);
        }

        public static void Warn(object message)
        {
            if (log.IsWarnEnabled)
                log.Warn(message);
        }

        public static void Warn(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(provider, format, args);
        }

        public static void Warn(string format, params object[] args)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(format, args);
        }
    }
}
