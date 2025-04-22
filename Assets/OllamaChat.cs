using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;

public class OllamaChat : MonoBehaviour
{
    public TextMeshProUGUI responseTextUI;

    void Start()
    {
        StartCoroutine(SendMessageToOllama("Hello there!"));
    }

    IEnumerator SendMessageToOllama(string userMessage)
    {
        string url = "http://localhost:11434/api/chat";
        string json = "{\"model\": \"mistral\", \"stream\": true, \"messages\": [{\"role\": \"user\", \"content\": \"" + userMessage + "\"}]}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string[] lines = request.downloadHandler.text.Split('\n');
            StringBuilder responseBuilder = new StringBuilder();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var response = JsonUtility.FromJson<ChatChunk>(line);
                    if (response != null && response.message != null && !string.IsNullOrEmpty(response.message.content))
                    {
                        responseBuilder.Append(response.message.content);
                    }
                }
                catch
                {
                    Debug.LogWarning("Parse failed: " + line);
                }
            }

            string finalResponse = responseBuilder.ToString();
            responseTextUI.text = finalResponse;
        }
        else
        {
            responseTextUI.text = "Ollama Error: " + request.error;
        }
    }

    [System.Serializable]
    public class ChatChunk
    {
        public Message message;
        public bool done;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }
}

