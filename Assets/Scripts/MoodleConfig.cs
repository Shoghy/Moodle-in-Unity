using UnityEngine;

public class MoodleConfig : MonoBehaviour {
  static MoodleConfig instance = null;

  [SerializeField] private TextAsset configFile;

  private string _token;
  private string _siteServerURL;
  public string token{
    get => _token;
  }
  public string siteServerURL{
    get => _siteServerURL;
  }

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
    var configInfo = configFile.text.Split("\n");

    foreach (var configField in configInfo) {
      var field_value = configField.Split("=");

      switch (field_value[0]) {
        case "token": {
          _token = field_value[1].Trim();
          break;
        }
        case "siteServerURL": {
          _siteServerURL = field_value[1].Trim();
          break;
        }
      }
    }
  }

  void SetUpFunctions(){
    coreUser = new CoreUser(this);
  }
}
