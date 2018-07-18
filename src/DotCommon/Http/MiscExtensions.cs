using System.Text;

namespace DotCommon.Http
{
    public static class MiscExtensions
    {
        public static string AsString(this byte[] buffer)
        {
            if (buffer == null)
            {
                return "";
            }
            // Ansi as default
            var encoding = Encoding.UTF8;
            return encoding.GetString(buffer, 0, buffer.Length);
        }
    }
}
