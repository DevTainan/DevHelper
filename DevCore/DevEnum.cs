using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    public enum ValueTypeEnum   // 數值類型
    {
        NONE,
        UINT,
        INT,
        STRING,
        LIST,
    }

    public enum AnalyzeResultEnum   // 解析結果
    {
        Error,
        Success,
    }
}
