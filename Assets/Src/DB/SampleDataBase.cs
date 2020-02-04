using UnityEngine;
using System.Collections.Generic;

public class SampleDataBase : MonoBehaviour {

    private void Start()
    {
        
    }
    public SqliteDatabase sqlDB;
    string query;
    public string name;
    public string state;
    public int num;
    public void SelectDB(){
　　　　　sqlDB = new SqliteDatabase("PlayerStatus.db");
　　　　　query = "SELECT * FROM example";
　　　　　var dt = sqlDB.ExecuteQuery(query);

        foreach(DataRow dr in dt.Rows){
            name = (string)dr["name"];
            state = (string)dr["state"];
            num = (int)dr["num"];
            Debug.Log("name:" + name.ToString());
            Debug.Log("State:" + state);
            Debug.Log("num:" + num);
        }
    }

    public void UpdateDB(string name, bool status, int num){
        sqlDB = new SqliteDatabase("PlayerStatus.db");
        query = "UPDATE example SET  name = \'" + name +
                                "\', state = \'" + status +
                                "\', num = \'" + num + "\'";

        var dt = sqlDB.ExecuteQuery(query);
    }

    public void DeleteDB()
    {

    }
}
