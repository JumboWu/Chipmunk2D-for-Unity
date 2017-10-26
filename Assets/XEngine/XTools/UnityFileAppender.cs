using UnityEngine;
using System.IO;
using log4net.Appender;
using log4net.Core;

namespace X.Tools
{
    public class UnityFileAppender : RollingFileAppender
    {
        public override string File
        {
            get
            {
                return base.File;
            }

            set
            {
                string path;
                if (Application.isEditor)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                }
                else
                {
                    path = Path.Combine(Application.persistentDataPath, "Logs");
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                base.File = Path.Combine(path, value);
            }
        }
    }
}
