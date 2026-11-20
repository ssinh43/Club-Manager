using System.Security.Cryptography;
using System.Text;

namespace clubmanager.Utilities
{
    public class Functions
    {
        // Lưu thông tin đăng nhập
        public static int _UserID = 0;
        public static string _UserName = string.Empty;
        public static string _FullName = string.Empty;
        public static int _RoleID = 0;
        public static string _Message = string.Empty;
        public static string _Email = string.Empty;

        // Biến đăng nhập của Admin
        public static int _AdminID = 0;
        public static string _AdminName = string.Empty;
        public static string _AdminEmail = string.Empty;
        public static string _AdminMessage = string.Empty;

        public static bool IsAdminLogin() => _AdminID > 0;

        public static string MD5Hash(string? text)
        {
            var bytes = MD5.HashData(Encoding.ASCII.GetBytes(text ?? ""));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static string MD5Password(string? text)
        {
            string str = MD5Hash(text);
            for (int i = 0; i <= 5; i++)
                str = MD5Hash(str + str);
            return str;
        }

        public static void Logout()
        {
            _UserID = 0; _UserName = ""; _FullName = ""; _RoleID = 0; _Message = "";
        }

        public static bool IsLogin()
        {
            if ((Functions._UserID <= 0) || string.IsNullOrEmpty(Functions._UserName) || string.IsNullOrEmpty(Functions._Email))
                return false;
            return true;
        }
    }
}