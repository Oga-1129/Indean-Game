using UnityEngine;
using System.Collections.Generic;

public class SampleDataBase : MonoBehaviour {

    public SqliteDatabase sqlDB;

    void Start(){
　　　　　sqlDB = new SqliteDatabase("PlayerStatus.db");
　　　　　string query = "SELECT * FROM example";
　　　　　var dt = sqlDB.ExecuteQuery(query);

        string name;
        int num;
        foreach(DataRow dr in dt.Rows){
            name = (string)dr["name"];
            num = (int)dr["num"];
            Debug.Log("name:" + name.ToString());
            Debug.Log("num:" + num);
        }
    }
}
