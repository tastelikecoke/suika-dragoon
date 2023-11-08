using System.Collections.Generic;
using Proyecto26;

namespace FirebaseREST
{
    public struct ErrorResponse
    {
        public double code;
        public string message;

        public ErrorResponse(RequestException exception)
        {
            var dict = Json.Deserialize(exception.Response) as Dictionary<string, object>;
            var errorDict = dict["error"] as Dictionary<string, object>;
            this.code = (double)errorDict["code"];
            this.message = errorDict["message"].ToString();
        }
    }
}
