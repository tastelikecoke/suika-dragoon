using System.Text;
using UnityEngine;

namespace FirebaseREST
{
    public class DebugUtility
    {
        public static void PrintFields(object obj)
        {
            var fields = obj.GetType().GetFields();
            var sb = new StringBuilder();

            foreach (var field in fields)
            {
                sb.AppendLine($"<b>{field.Name}</b> : {field.GetValue(obj)}");
            }

            Debug.Log(sb);
        }

        public static void PrintProperties(object obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();

            foreach (var prop in props)
            {
                sb.AppendLine($"<b>{prop.Name}</b> : {prop.GetValue(obj)}");
            }

            Debug.Log(sb);
        }
    }

}
