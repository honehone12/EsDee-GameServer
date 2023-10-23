using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace EsDee
{
    public class MatchingClient : MonoBehaviour
    {
        public class TicketCreateResponse
        {
            public string Uuid;
        }

        public class MatchingResult
        {
            public string Ip;
            public string[] Uuids;
        }

        public class StatusValue
        {
            public int StatusCode;
            public MatchingResult Result;
        }

        public const int StatusDone = 1;
        public const int StatusWaiting = 2;

        [SerializeField]
        ServerUrl matchingServerUrl;
        [SerializeField]
        float pollingInterval = 1.0f;
        [SerializeField]
        string pollingRoute = "/status/poll";
        [SerializeField]
        string standbyRoute = "/status/standby";
        [Header("UI")]
        [SerializeField]
        MatchingUI matchingUI;
        [Header("Scene")]
        [SerializeField]
        SceneLoader sceneLoader;

        uint numTry = 0;

        void Awake()
        {
            Assert.IsNotNull(matchingServerUrl);
            Assert.IsTrue(pollingInterval > 0.0f);
            Assert.IsFalse(string.IsNullOrEmpty(pollingRoute));
            Assert.IsNotNull(matchingUI);
            Assert.IsNotNull(sceneLoader);
        }

        void Start()
        {
            StartMatching();
        }

        void StartMatching()
        {
            _ = StartCoroutine(TicketCreate(
                OnTicketCreateSuccess,
                (err) =>
                {
                    Debug.LogError($"[{err.code}], {err.message}");
                }
            ));
        }

        IEnumerator TicketCreate(
            UnityAction<TicketCreateResponse> OnSuccess, 
            UnityAction<HttpError> OnError)
        {
            Assert.IsNotNull(OnSuccess);
            Assert.IsNotNull(OnError);

            var url = matchingServerUrl.Url.GetUrlString();
            using var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<TicketCreateResponse>(req.downloadHandler.text);
                if (res != null)
                {
                    OnSuccess(res);
                }
                else
                {
                    OnError(new HttpError(
                        "failed to parse json",
                        UnityWebRequest.Result.DataProcessingError
                    ));
                }
            }
            else
            {
                OnError(new HttpError(req.error, req.result));
            }
        }

        void OnTicketCreateSuccess(TicketCreateResponse res)
        {
            _ = StartCoroutine(StatusPoll(
                res.Uuid,
                OnPollingContinue,
                OnMatchingDone,
                (err) => 
                {
                    Debug.LogError($"[{err.code}], {err.message}");
                }
            ));
        }

        IEnumerator StatusPoll(string uuid,
            UnityAction OnPollingContinue,
            UnityAction<string, StatusValue> OnMatchingDone,
            UnityAction<HttpError> OnError)
        {
            Assert.IsNotNull(OnPollingContinue);
            Assert.IsNotNull(OnMatchingDone);
            Assert.IsNotNull(OnError);

            var ticker = new WaitForSeconds(pollingInterval);
            var isWaiting = true;
            var form = new WWWForm();
            form.AddField("uuid", uuid);
            var url = matchingServerUrl.Url.GetUrlString(pollingRoute);
            
            while (isWaiting)
            {
                yield return ticker;
                using var req = UnityWebRequest.Post(url, form);
                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    var res = JsonUtility.FromJson<StatusValue>(req.downloadHandler.text);
                    if (res != null)
                    {
                        if (res.StatusCode == StatusDone)
                        {
                            OnMatchingDone(uuid, res);
                        }
                        else if (res.StatusCode == StatusWaiting)
                        {
                            OnPollingContinue();
                            continue;
                        }
                    }
                    else
                    {
                        OnError(new HttpError(
                            "failed to parse json",
                            UnityWebRequest.Result.DataProcessingError
                        ));
                    }
                }
                else
                {
                    OnError(new HttpError(req.error, req.result));
                }

                isWaiting = false;
            }
        }

        void OnMatchingDone(string uuid, StatusValue res)
        {
            matchingUI.SwapTextAsDone();
            _ = StartCoroutine(Standby(
                uuid, res,
                (err) =>
                {
                    Debug.LogError($"[{err.code}], {err.message}");
                }
            ));
        }

        void OnPollingContinue()
        {
            numTry++;
            matchingUI.IncrementText(numTry);
        }

        IEnumerator Standby(string uuid, StatusValue res, UnityAction<HttpError> OnError)
        {
            Assert.IsNotNull(OnError);

            var form = new WWWForm();
            form.AddField("uuid", uuid);
            using var req = UnityWebRequest.Post(matchingServerUrl.Url.GetUrlString(standbyRoute), form);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                sceneLoader.Load();
            }
            else
            {
                OnError(new HttpError(req.error, req.result));
            }
        }
    }
}
