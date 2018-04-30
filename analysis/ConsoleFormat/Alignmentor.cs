using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleFormat
{
    public class AlignmentHelper
    {
        /// <summary>
        /// 对StringBuidler集合进行数据格式对齐
        /// </summary>
        /// <param name="list">StringBuilder集合</param>
        /// <returns>对齐之后集合元素的真实长度</returns>
        public static int Alignment(IEnumerable<StringBuilder> list, string virtualSplite = null, string realSplite = "|", AlignmentType type = AlignmentType.Left)
        {
            if (list != null && list.Count() > 0)
            {
                int maxLength = list.Max(ele => { return GetRealLength(ele); });

                foreach (var item in list)
                {
                    if (GetRealLength(item) < maxLength)
                    {
                        int AlignmentSpace = maxLength - GetRealLength(item);
                        int offsetControl = AlignmentSpace / 2;
                        StringBuilder LeftSpace = new StringBuilder();
                        for (int i = 0; i < AlignmentSpace; i += 1)
                        {
                            switch (type)
                            {
                                case AlignmentType.Left:
                                    item.Append(" ");
                                    break;
                                case AlignmentType.Right:
                                    LeftSpace.Append(" ");
                                    break;
                                case AlignmentType.Center:
                                    if (i < offsetControl)
                                    {
                                        item.Append(" ");
                                    }
                                    else
                                    {
                                        LeftSpace.Append(" ");
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (type == AlignmentType.Right || type == AlignmentType.Center)
                        {
                            if (virtualSplite != null)
                            {
                                item.Replace(virtualSplite, realSplite + LeftSpace);
                            }
                            else
                            {
                                item.Insert(0, LeftSpace);
                            }
                        }
                        item.Append(virtualSplite);
                    }
                    else
                    {
                        if (virtualSplite != null)
                        {
                            item.Replace(virtualSplite, realSplite);
                        }
                        item.Append(virtualSplite);
                    }
                }
                return maxLength;
            }
            return -1;
        }


        /// <summary>
        ///  对StringBuilder集合进行数据对齐，并添加头尾
        /// </summary>
        /// <param name="list">StringBuilder集合</param>
        /// <param name="chars">char[0]为头，char[1]为尾。</param>
        /// <param name="alignmentType">对齐模式,默认为None,不做操作</param>
        /// <returns>对齐之后集合元素的真实长度</returns>
        public static int Packet(IEnumerable<StringBuilder> list, string chars, AlignmentType alignmentType = AlignmentType.None)
        {
            //集合对齐
            Alignment(list);

            //长度记录
            int result = 0;
            foreach (var item in list)
            {
                //记录对齐长度
                if (result == 0)
                {
                    result = Packet(item, chars, alignmentType);
                }
                else
                {
                    Packet(item, chars, alignmentType);
                }
            }

            //返回对齐长度
            return result;
        }

        /// <summary>
        ///  对StringBuilder集合进行数据对齐，并添加头尾
        /// </summary>
        /// <param name="list">StringBuilder集合</param>
        /// <param name="start">头</param>
        /// <param name="end">尾</param>
        /// <param name="alignmentType">对齐模式,默认为None,不做操作</param>
        /// <returns>对齐之后集合元素的真实长度</returns>
        public static int Packet(IEnumerable<StringBuilder> list, string start, string end, AlignmentType alignmentType = AlignmentType.None)
        {
            //集合对齐
            Alignment(list);

            //长度记录
            int result = 0;
            foreach (var item in list)
            {
                //记录对齐长度
                if (result == 0)
                {
                    result = Packet(item, start, end, alignmentType);
                }
                else
                {
                    Packet(item, start, end, alignmentType);
                }
            }

            //返回对齐长度
            return result;
        }

        /// <summary>
        /// 对单个StringBuilder实例进行前后插入
        /// </summary>
        /// <param name="instance">StringBuilder实例</param>
        /// <param name="chars">前chars[0],后字符chars[1]</param>
        /// <param name="alignmentType">对齐模式,默认为None,不做操作</param>
        /// <returns>返回补齐之后的真实长度</returns>
        public static int Packet(StringBuilder instance, string chars, AlignmentType alignmentType = AlignmentType.None)
        {
            //判空
            if (chars != null)
            {
                //插头补尾
                if (chars.Length > 0)
                {
                    instance.Insert(0, chars[0]);
                }
                if (chars.Length > 1)
                {
                    instance.Append(chars[1]);
                }

                //重计算
                int evenAlignmentLength = GetRealLength(instance);

                //奇偶对齐
                switch (alignmentType)
                {
                    case AlignmentType.Even:
                        if ((evenAlignmentLength & 1) != 0)
                        {
                            instance.Insert(instance.Length - 1, " ");
                            return evenAlignmentLength + 1;
                        }
                        break;
                    case AlignmentType.Odd:
                        if ((evenAlignmentLength & 1) == 0)
                        {
                            instance.Insert(instance.Length - 1, " ");
                            return evenAlignmentLength + 1;
                        }
                        break;
                    default:
                        return evenAlignmentLength;
                }
            }
            //返回默认长度
            return GetRealLength(instance);
        }

        /// <summary>
        /// 对单个StringBuilder实例进行前后插入
        /// </summary>
        /// <param name="instance">StringBuilder实例</param>
        /// <param name="start">头</param>
        /// <param name="end">尾</param>
        /// <param name="alignmentType">对齐模式,默认为None,不做操作</param>
        /// <returns>返回补齐之后的真实长度</returns>
        public static int Packet(StringBuilder instance, string start, string end, AlignmentType alignmentType = AlignmentType.None)
        {
            //插头补尾
            instance.Insert(0, start);
            instance.Append(end);

            //记录插入后的长度
            int evenAlignmentLength = GetRealLength(instance);

            //奇偶对齐
            switch (alignmentType)
            {
                case AlignmentType.Even:

                    if ((evenAlignmentLength & 1) != 0)
                    {
                        instance.Insert(instance.Length - 1, " ");
                        return evenAlignmentLength + 1;
                    }

                    break;

                case AlignmentType.Odd:

                    if ((evenAlignmentLength & 1) == 0)
                    {
                        instance.Insert(instance.Length - 1, " ");
                        return evenAlignmentLength + 1;
                    }

                    break;

                default:
                    return evenAlignmentLength;
            }

            //返回默认长度
            return evenAlignmentLength;
        }

        /// <summary>
        /// 获取字符串在输出缓冲区的真实长度
        /// </summary>
        /// <param name="instance">需要计算的字符串</param>
        /// <returns>字符串的真是长度</returns>
        public static int GetRealLength(string instance)
        {
            int length = 0;
            for (int i = 0; i < instance.Length; i += 1)
            {
                if (instance[i] >= 0 && instance[i] <= 127)
                {
                    length += 1;
                }
            }
            return instance.Length * 2 - length;
        }

        /// <summary>
        /// 获取StringBuilder在输出缓冲区的真实长度
        /// </summary>
        /// <param name="instance">需要计算的StringBuilder实例</param>
        /// <returns>StringBuilder的真是长度</returns>
        public static int GetRealLength(StringBuilder instance)
        {
            int length = 0;
            for (int i = 0; i < instance.Length; i += 1)
            {
                if (instance[i] >= 0 && instance[i] <= 127)
                {
                    length += 1;
                }
            }
            return instance.Length * 2 - length;
        }
    }
}
