using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CoreUser{
  readonly private string _urlStart;

  public CoreUser(string siteServerURL, string token){
    _urlStart = $"{siteServerURL}?wstoken={token}&{Constants.responseType}";
  }

  public async Task<UserInfo[]> GetUsersByField(EGetUsersByField field, string[] values) {
    const string function = "core_user_get_users_by_field";
    var url = $"{_urlStart}&wsfunction={function}&field={field}";

    for (var i = 0; i < values.Length; ++i) {
      url += $"&values[{i}]={values[i]}";
    }

    var response = await StaticHandlers.SendRequestHandler(url);

    return ArrayJsonWrapper<UserInfo>.FromJSON(response);
  }

  public async Task<GetUsersResponse> GetUsers(GetUsersCriteria[] criteria){
    const string function = "core_user_get_users";
    var url = $"{_urlStart}&wsfunction={function}";

    for(var i = 0; i < criteria.Length; ++i){
      var c = criteria[i];
      url += $"&criteria[{i}][key]={c.key}";
      url += $"&criteria[{i}][value]={c.value}";
    }
    
    var response = UnityWebRequest.Get(url);
    await StaticHandlers.AwaitUnityRequest(response);

    if (response.result != UnityWebRequest.Result.Success) {
      response.Dispose();
      throw new Exception("An unknown error has occurred");
    }

    var textResponse = response.downloadHandler.text;

    response.Dispose();

    StaticHandlers.ThrowMoodleException(textResponse);

    return JsonUtility.FromJson<GetUsersResponse>(textResponse);
  }
}

public class GetUsersCriteria{
  public GetUsersCriteriaKeys key;
  public string value;

  public GetUsersCriteria(GetUsersCriteriaKeys key, string value){
    this.key = key;
    this.value = value;
  }
}

[Serializable]
public class GetUsersResponse : JsonType{
  public UserInfo[] users;
  public MoodleWarning[] warnings;
}

public enum GetUsersCriteriaKeys{
  id,
  lastname,
  firtsname,
  idnumber,
  username,
  email,
  auth
}

public enum EGetUsersByField {
  id,
  idnumber,
  username,
  email
}