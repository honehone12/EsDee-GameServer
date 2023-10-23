using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Http/ServerUrl", fileName = "ServerUrl")]
    public class ServerUrl : ScriptableObject
    {
        public class UrlCreator
        {
            public string address;
            public string port;

            public UrlCreator(string address, string port)
            {
                this.address = address;
                this.port = port;
            }

            public string GetUrlString()
            {
                return HttpPrefix + address + Colon + port;
            }

            public string GetUrlString(string route)
            {
                return HttpPrefix + address + Colon + port + route;
            }
        }

        public const string HttpPrefix = "http://";
        public const string Colon = ":";

        [SerializeField]
        string serverAddress = "127.0.0.1";
        [SerializeField]
        string serverPort = "9999";

        public UrlCreator Url => new (serverAddress, serverPort);
    }
}
