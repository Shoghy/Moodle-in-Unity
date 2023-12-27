using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ArrayJsonWrapper<T> {
  public T[] items;

  public static T[] FromJSON(string json) {
    json = $"{{\"items\": {json} }}";
    var wrapper = JsonUtility.FromJson<ArrayJsonWrapper<T>>(json);
    return wrapper.items;
  }
}

public static class Constants{
  public const string responseType = "moodlewsrestformat=json";
}

public static class StaticHandlers{
  public static void ThrowMoodleException(string json) {
    if(!json.StartsWith("{\"exception\"")){
      return;
    }
    MoodleException moodleException;

    try{
      moodleException = JsonUtility.FromJson<MoodleException>(json);
    } catch(Exception) {
      return;
    }

    throw new MoodleException(moodleException);
  }

  public static async Task AwaitUnityRequest(UnityWebRequest request) {
    request.SendWebRequest();
    while (!request.isDone) {
      await Task.Yield();
    }
  }

  public static async Task<string> SendRequestHandler(string url){
    var response = UnityWebRequest.Get(url);
    await AwaitUnityRequest(response);

    if (response.result != UnityWebRequest.Result.Success) {
      response.Dispose();
      throw new Exception("An unknown error has occurred");
    }

    var textResponse = response.downloadHandler.text;

    response.Dispose();

    ThrowMoodleException(textResponse);

    return textResponse;
  }
}

[Serializable]
public class JsonType{
    public override string ToString(){
      return JsonUtility.ToJson(this);
    }
}