using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CoreAuth{
  readonly private string _urlStart;
  public CoreAuth(string siteServerURL, string token){
    _urlStart = $"{siteServerURL}?wstoken={token}&{Constants.responseType}";
  }

  public async Task<ConfirmUserResponse> ConfirmUser(string username, string secret){
    const string function = "core_auth_confirm_user";
    var url = $"{_urlStart}&wsfunction={function}&username={username}&secret={secret}";

    var response = UnityWebRequest.Get(url);
    await StaticHandlers.AwaitUnityRequest(response);

    if (response.result != UnityWebRequest.Result.Success) {
      response.Dispose();
      throw new Exception("An unknown error has occurred");
    }

    var textResponse = response.downloadHandler.text;

    response.Dispose();

    StaticHandlers.ThrowMoodleException(textResponse);
    
    return JsonUtility.FromJson<ConfirmUserResponse>(textResponse);
  }
}

[Serializable]
public class ConfirmUserResponse : JsonType{
  public int success;
  public MoodleWarning[] warnings;
}