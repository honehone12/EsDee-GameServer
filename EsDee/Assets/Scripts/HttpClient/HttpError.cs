using UnityEngine.Networking;

namespace EsDee
{
    public class HttpError
    {
        public string message;
        public UnityWebRequest.Result code;

        public HttpError(string message, UnityWebRequest.Result code)
        {
            this.message = message;
            this.code = code;
        }
    }
}
