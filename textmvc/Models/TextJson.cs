using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace textmvc.Models
{
    /// <summary>
    /// 接收從API返回的資料物件
    /// </summary>
    public class TextJson
    {
        public string language { get; set; }
        public float textAngle { get; set; }
        public string orientation { get; set; }

        /// <summary>
        /// 區域，每個區域包含數行文字字串
        /// </summary>
        public Region[] regions { get; set; }
    }

   
    public class Region
    {
        public string boundingBox { get; set; }

        /// <summary>
        /// 每行文字包含數行可辨識的字串
        /// </summary>
        public Line[] lines { get; set; }
    }

   
    public class Line
    {
        public string boundingBox { get; set; }

        /// <summary>
        /// 可辨識出的字串
        /// </summary>
        public Word[] words { get; set; }
    }

   
    public class Word
    {
        public string boundingBox { get; set; }

        /// <summary>
        /// 每個字串包含可辨識出的文字
        /// </summary>
        public string text { get; set; }
    }

}