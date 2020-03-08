using UnityEngine;
using System.Collections.Generic;

public class SampleDataBase : MonoBehaviour {

    private void Start()
    {
        
    }
    public SqliteDatabase sqlDB;
    string query;
    public string PlayerName;
    public string state;
    public int num;
    public void SelectDB(){
　　　　　sqlDB = new SqliteDatabase("PlayerStatus.db");
　　　　　query = "SELECT * FROM example";
　　　　　var dt = sqlDB.ExecuteQuery(query);

        foreach(DataRow dr in dt.Rows){
            PlayerName = (string)dr["name"];
            state = (string)dr["state"];
            num = (int)dr["num"];
        }
    }

    public void UpdateDB(string name, string status, int num){
        sqlDB = new SqliteDatabase("PlayerStatus.db");
        state = status;
        query = "UPDATE example SET  name = \'" + name +
                                "\', state = \'" + state +
                                "\', num = \'" + num + "\'";

        var dt = sqlDB.ExecuteQuery(query);
    }
}
