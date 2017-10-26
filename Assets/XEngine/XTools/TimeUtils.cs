using System;
using System.Collections.Generic;

namespace X.Tools
{ 

public  class TimeUtils
{
    private static readonly DateTime utc_time = new DateTime(1970, 1, 1);
    /// <summary>
    /// 获取当前的时间 单位：毫秒
    /// </summary>
    public static UInt32 Now
    {
        get
        {
            return (UInt32)(Convert.ToInt64(DateTime.UtcNow.Subtract(utc_time).TotalMilliseconds) & 0xffffffff);
        }
    }
}

}