using System;
using UnityEngine;

[Serializable]
public class UserInfo {
  public int id;
  public string username;
  public string firstname;
  public string lastname;
  public string fullname;
  public string email;
  public string department;
  public long firstaccess;
  public long lastaccess;
  public string auth;
  public bool suspended;
  public bool confirmed;
  public string lang;
  public string theme;
  public string timezone;
  public int mailformat;
  public string description;
  public string descriptionformat;
  public string profileimageurlsmall;
  public string profileimageurl;

  public override string ToString(){
    return JsonUtility.ToJson(this);
  }
}

public enum UserSerchFields {
  id,
  idnumber,
  username,
  email
}

[Serializable]
public class MoodleException : Exception{
  public string exception;
  public string errorcode;
  public string message;
  public override string ToString() {
      return JsonUtility.ToJson(this);
  }
}