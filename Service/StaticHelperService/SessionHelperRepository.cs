using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Service.StaticHelperService
{
    public static class SessionHelperRepository
    {
        public static void SetObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetStringAsJson(ISession session, string key, string value)
        {
            try
            {
                session.SetString(key, value);
            }
            catch { }
        }

        public static string GetStringFromJson(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? "" : value;
        }
        public static void DeleteDataSession(ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
