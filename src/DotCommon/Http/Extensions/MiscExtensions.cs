using System.IO;
using System.Text;

namespace DotCommon.Http
{
    public static class MiscExtensions
    {
        /// <summary>将byte[]保存到指定路径
        /// </summary>
        public static void SaveAs(this byte[] input, string path)
        {
            File.WriteAllBytes(path, input);
        }

        /// <summary>从Stream中读取byte[]
        /// </summary>
        public static byte[] ReadAsBytes(this Stream input)
        {
            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }

        /// <summary>Stream对拷
        /// </summary>
        public static void CopyTo(this Stream input, Stream output)
        {
            var buffer = new byte[32768];

            while (true)
            {
                var read = input.Read(buffer, 0, buffer.Length);

                if (read <= 0)
                    return;

                output.Write(buffer, 0, read);
            }
        }

        /// <summary>将数组转换为string
        /// </summary>
        public static string AsString(this byte[] buffer)
        {
            if (buffer == null)
                return "";
            // Ansi as default
            var encoding = Encoding.UTF8;

            return encoding.GetString(buffer, 0, buffer.Length);
        }
    }
}
