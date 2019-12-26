namespace Blog.Core.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether success.
        /// </summary>
        public  bool Success{get;set;}

        /// <summary>
        /// Gets or sets the msg.
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public List<T> Data{get;set;}
    }
}