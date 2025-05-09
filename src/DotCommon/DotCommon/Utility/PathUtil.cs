﻿using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public static class PathUtil
    {

        /// <summary>
        /// 如果路径是绝对路径,则原样返回,否则就结合AppDomain.CurrentDomain.SetupInformation.ApplicationBase
        /// 路径总是服务器的相对路径
        /// </summary>
        /// <param name="path">路径地址</param>
        /// <returns></returns>
        public static string LocateServerPath(string path = "")
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return path;
        }

        /// <summary>
        /// 合并相对路径
        /// </summary>
        /// <param name="relativePaths">多个相对路径数组</param>
        /// <returns></returns>
        public static string CombineRelative(params string[] relativePaths)
        {
            var pathBuilder = new StringBuilder();
            for (int i = 0; i < relativePaths.Length; i++)
            {
                if (i > 0)
                {
                    relativePaths[i] = relativePaths[i].Replace("~/", "").Replace("../", "");
                }
                if (!relativePaths[i].EndsWith("/"))
                {
                    relativePaths[i] = $"{relativePaths[i]}/";
                }
                if (relativePaths[i].StartsWith("/"))
                {
                    relativePaths[i] = relativePaths[i].Remove(0, 1);
                }
                pathBuilder.Append(relativePaths[i]);
            }
            pathBuilder.Remove(pathBuilder.Length - 1, 1);
            return pathBuilder.ToString();
        }


        /// <summary>
        /// 绝对目录回退,兼容Linux
        /// </summary>
        /// <param name="path">绝对路径地址</param>
        /// <param name="layerCount">回退层数</param>
        /// <returns></returns>
        public static string BackDirectory(string path, int layerCount = 1)
        {
            if (!Path.IsPathRooted(path))
            {
                return path;
            }
            //默认为Linux合并符
            var combineChar = Path.DirectorySeparatorChar;
            //默认Linux盘符,为空
            var rootPath = Path.GetPathRoot(path);
            //windows不包含盘符
            var pathSpilts = path.Split(combineChar).Where(x => !x.IsNullOrWhiteSpace() && !x.Contains(":")).ToList();
            //如果路径集的个数小于回退目录的个数,那么就直接返回根目录
            if (pathSpilts.Count <= layerCount)
            {
                return rootPath;
            }
            //移除不要的
            pathSpilts.RemoveRange(pathSpilts.Count - layerCount, layerCount);
            //路径集合中加入盘符
            pathSpilts.Insert(0, rootPath);
            return Path.Combine(pathSpilts.ToArray());
        }

        /// <summary>
        /// 将相对路径转换成绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns></returns>
        public static string MapPath(string relativePath)
        {
            string locatePath = LocateServerPath();
            //默认回退的文件夹数为2;
            var backLayer = 2;
            //根据相对路径获取回退的层数
            backLayer += relativePath.Length - relativePath.Replace("../", "..").Length;
            var path = BackDirectory(locatePath, backLayer);
            var usefulPaths = relativePath.Split('/').Where(x => x != ".." && x != "~");
            return Path.Combine(path, Path.Combine(usefulPaths.ToArray()));
        }

        /// <summary>
        /// 获取某个路径,或者文件中的文件扩展名
        /// </summary>
        /// <param name="path">路径或者文件地址</param>
        /// <returns></returns>
        public static string GetPathExtension(string path)
        {
            if (!path.IsNullOrWhiteSpace() && path.IndexOf('.') >= 0)
            {
                return path.Substring(path.LastIndexOf('.'));
            }
            return "";
        }

    }
}
