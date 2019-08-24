using System;
using System.Text;

namespace Natasha.Log.Model
{
    public abstract class ALogWrite
    {

        public string FormartCode;
        public StringBuilder Buffer;
        public abstract void  Write();


        public ALogWrite() => Buffer = new StringBuilder();



        /// <summary>
        /// 为代码增加行号
        /// </summary>
        /// <param name="formartCode">代码内容</param>
        /// <returns></returns>
        public void WrapperCode(string formartCode)
        {

            FormartCode = formartCode;
            var arrayLines = formartCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i += 1)
            {

                Buffer.AppendLine($"{i + 1}\t{arrayLines[i]}");

            }

        }




        /// <summary>
        /// 为日志封装一个包裹层，并显示有标题
        /// </summary>
        /// <param name="title"></param>
        public void WrapperTitle(string title)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n==================={title}===================\r\n");
            sb.Append(Buffer);
            sb.AppendLine("\r\n==========================================================");
            Buffer = sb;

        }

    }

}
