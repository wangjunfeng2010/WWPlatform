using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.Models
{
    public class  QzoneBase
    {
        /// <summary>
        /// 返回码 
        /// </summary>
        public int Ret { get; set; }
        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码 
        /// </summary>
        public string Msg { get; set; }
    }
}
