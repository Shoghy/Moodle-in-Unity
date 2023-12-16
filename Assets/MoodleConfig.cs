using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class MoodleConfig : MonoBehaviour {
  [SerializeField] private TextAsset configFile;
  string token;
  string siteServerURL;
  const string responseType = "moodlewsrestformat=json";

  static MoodleConfig instance = null;

  void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
      return;
    }

    var configInfo = configFile.text.Split("\n");
    foreach (var configField in configInfo) {
      var field_value = configField.Split("=");

      switch (field_value[0]) {
        case "token": {
          token = field_value[1].Trim();
          break;
        }
        case "siteServerURL": {
          siteServerURL = field_value[1].Trim();
          break;
        }
      }
    }
  }

  public async Task<UserInfo[]> CoreUserGetUsersByField(UserSerchFields field, string[] values) {
    const string function = "core_user_get_users_by_field";
    var url = $"{siteServerURL}?wstoken={token}&wsfunction={function}&{responseType}&field={field.name()}";

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

    ThrowMoodleException(textResponse);

    var users = ArrayJsonWrapper<UserInfo>.FromJSON(textResponse);

    return users;
  }

  void ThrowMoodleException(string json){
    MoodleException moodleException;

    try{
      moodleException = JsonUtility.FromJson<MoodleException>(json);
    }catch(Exception){
      return;
    }

    if(moodleException == null){
      return;
    }

    throw moodleException;
  }
}
