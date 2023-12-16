using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class CoreUser{
  readonly MoodleConfig config;
  public CoreUser(MoodleConfig moodleConfig){
    config = moodleConfig;
  }

  public async Task<UserInfo[]> CoreUserGetUsersByField(UserSerchFields field, string[] values) {
    const string function = "core_user_get_users_by_field";
    var url = $"{config.siteServerURL}?wstoken={config.token}&wsfunction={function}&{Constants.responseType}&field={field.name()}";

    for (var i = 0; i < values.Length; ++i) {
      url += $"&values[{i}]={values[i]}";
    }

    var response = UnityWebRequest.Get(url);
    response.SendWebRequest();

    while (!response.isDone) {
      await Task.Yield();
    }

    var textResponse = response.downloadHandler.text;
  
    if (response.result != UnityWebRequest.Result.Success) {
      response.Dispose();
      throw new Exception("An unknown error has occurred");
    }
    response.Dispose();

    Constants.ThrowMoodleException(textResponse);

    var users = ArrayJsonWrapper<UserInfo>.FromJSON(textResponse);

    return users;
  }
}