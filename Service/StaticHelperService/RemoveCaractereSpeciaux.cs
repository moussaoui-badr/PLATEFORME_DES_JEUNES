using System.Text;

namespace Service.StaticHelperService
{
    public static class RemoveCaractereSpeciaux
    {
        public static string removeCaractereSpeciaux(string str)
        {
            StringBuilder sb = new StringBuilder();
            string extension = Path.GetExtension(str);
            str = str.Replace(extension, "");
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            sb.Append(extension);
            return sb.ToString();
        }
    }
}
