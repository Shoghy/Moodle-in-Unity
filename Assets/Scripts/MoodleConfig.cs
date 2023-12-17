using System;
using UnityEngine;

public class MoodleConfig : MonoBehaviour {
  static MoodleConfig instance = null;

  [SerializeField] private TextAsset configFile;

  private string _token;
  private string _siteServerURL;

  [HideInInspector] public CoreUser coreUser;

  void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
      return;
    }

    SetUpValues();
    SetUpFunctions();
  }

  void SetUpValues(){
    var configInfo = JsonUtility.FromJson<ConfigFile>(configFile.text);
    _token = configInfo.token;
    _siteServerURL = configInfo.siteServerURL;
  }

  void SetUpFunctions(){
    coreUser = new CoreUser(_siteServerURL, _token);
  }
}

[Serializable]
public class ConfigFile{
  public string token;
  public string siteServerURL;
}