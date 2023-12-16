using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class EnumExtensions {
  private static readonly ConcurrentDictionary<string, string> DisplayNameCache = new ConcurrentDictionary<string, string>();

  public static string name(this Enum value) {
    var key = $"{value.GetType().FullName}.{value}";

    var displayName = DisplayNameCache.GetOrAdd(key, x => {
      var name = (DescriptionAttribute[])value
                                        .GetType()
                                        .GetTypeInfo()
                                        .GetField(value.ToString())
                                        .GetCustomAttributes(
                                          typeof(DescriptionAttribute),
                                          false
                                        );

      return name.Length > 0 ? name[0].Description : value.ToString();
    });

    return displayName;
  }
}

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
  public static void ThrowMoodleException(string json){
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