using System.Collections.Generic;
using System.Threading.Tasks;

public class CoreGrades{
  readonly private string _urlStart;

  public CoreGrades(string siteServerURL, string token){
    _urlStart = $"{siteServerURL}?wstoken={token}&{Constants.responseType}";
  }
  
  public async Task UpdateGrades(
    string source,
    int courseid,
    string component,
    int activityid,
    int itemnumber,
    MoodleGrade[] grades,
    UpdateGradesDetails[] itemdetails = null
  ){
    const string function = "core_grades_update_grades";
    var url = $"{_urlStart}&wsfunction={function}";
    url += $"&source={source}";
    url += $"&courseid={courseid}";
    url += $"&component={component}";
    url += $"&activityid={activityid}";
    url += $"&itemnumber={itemnumber}";

    for(var i = 0; i < grades.Length; ++i){
      var grade = grades[i];
      url += $"&grades[{i}][studentid]={grade.studentid}";
      url += $"&grades[{i}][grade]={grade.grade}";
      if(grade.str_feedback != null){
        url += $"&grades[{i}][str_feedback]={grade.str_feedback}";
      }
    }

    itemdetails ??= new UpdateGradesDetails[0];

    for(var i = 0; i < itemdetails.Length; ++i){
      var itemdetail = itemdetails[i].toDict();
      foreach(var element in itemdetail){
        if(element.Value == null) continue;

        url += $"&itemdetails[{i}][{element.Key}]={element.Value}";
      }
    }

  }
}

public class UpdateGradesDetails{
  public string itemname;
  public int? idnumber;
  public MoodleGradeType? gradetype;
  public float? grademax;
  public float? grademin;
  public int? scaleid;
  public float? multfactor;
  public float? plusfactor;
  public bool? deleted;
  public bool? hidden;

  public UpdateGradesDetails(
    string itemname = null,
    int? idnumber = null,
    MoodleGradeType? gradetype = null,
    float? grademax = null,
    float? grademin = null,
    int? scaleid = null,
    float? multfactor = null,
    float? plusfactor = null,
    bool? deleted = null,
    bool? hidden = null
  ){
    this.itemname = itemname;
    this.idnumber = idnumber;
    this.gradetype = gradetype;
    this.grademax = grademax;
    this.grademin = grademin;
    this.scaleid = scaleid;
    this.multfactor = multfactor;
    this.plusfactor = plusfactor;
    this.deleted = deleted;
    this.hidden = hidden;
  }

  public Dictionary<string, object> toDict(){
    var dict = new Dictionary<string, object>{
        { "itemname", itemname },
        { "idnumber", idnumber },
        { "gradetype", (int?)gradetype },
        { "grademax", grademax },
        { "grademin", grademin },
        { "scaleid", scaleid },
        { "multfactor", multfactor },
        { "plusfactor", plusfactor }
    };
  
    if(deleted != null){
      dict.Add("deleted", (bool)deleted ? 1 : 0);
    }
    if(hidden != null){
      dict.Add("hidden", (bool)hidden ? 1 : 0);
    }

    return dict;
  }
}