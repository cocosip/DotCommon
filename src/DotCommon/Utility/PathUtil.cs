using System.IO;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
	/// <summary>路径工具类
	/// </summary>
	public class PathUtil
	{
		/// <summary>If the path is absolute is return as is, otherwise is combined with AppDomain.CurrentDomain.SetupInformation.ApplicationBase
		/// The path are always server relative path.
		/// </summary>
		public static string LocateServerPath(string path = "")
		{
		    if (!Path.IsPathRooted(path))
		    {
		        //path = Path.Combine(.Location, path);
		    }
		    return path;
		}

		/// <summary>合并相对路径
		/// </summary>
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

		/// <summary>合并绝对路径,如果是盘符,也会被合并
		/// </summary>
		public static string Combine(params string[] paths)
		{
			var usefulPaths = paths.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
			{
				if (x.Contains(":"))
				{
					return $"{x}\\";
				}
				return x.StartsWith("\\") ? x.Remove(0, 1) : x;
			});
			return Path.Combine(usefulPaths.ToArray());
		}

		/// <summary>绝对目录回退
		/// </summary>
		public static string BackDirectory(string path, int layerCount = 1)
		{
			if (!Path.IsPathRooted(path))
			{
				return path;
			}
			var pathSpilts = path.Split('\\').ToList();
			var notEmptyPaths =
				pathSpilts.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Contains(":") ? $"{x}\\" : x).ToList();
			int endRange = 2;
			if (notEmptyPaths.Count - layerCount > 2)
			{
				endRange = notEmptyPaths.Count - layerCount;
			}
			return Path.Combine(notEmptyPaths.GetRange(0, endRange).ToArray());
		}

		/// <summary>将绝对路径转换成相对路径
		/// </summary>
		public static string MapPath(string relativePath)
		{
			string locatePath = LocateServerPath();
			var backLayer = 2; //默认回退的文件夹数为2;
			//根据相对路径获取回退的层数
			backLayer += relativePath.Length - relativePath.Replace("../", "..").Length;
			var path = BackDirectory(locatePath, backLayer);
			var usefulPaths = relativePath.Split('/').Where(x => x != ".." && x != "~");
			return Path.Combine(path, Path.Combine(usefulPaths.ToArray()));
		}

		/// <summary>获取某个路径中文件的扩展名
		/// </summary>
		public static string GetPathExtension(string path)
		{
			if (!string.IsNullOrWhiteSpace(path) && path.IndexOf('.') > 0)
			{
				return path.Substring(path.LastIndexOf('.') + 1);
			}
			return "";
		}

	}
}
