using System;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Indean-Game-State")]
public class GameLog
{
    [DynamoDBHashKey]
    public int RoomID { 
        get; 
        set; 
    }
    
    //発言内容
    [DynamoDBProperty]
    public string Remarks{
        get;
        set;
    }

    //開始時間
    [DynamoDBProperty]
    public DateTime OpenRoom{
        get;
        set;
    }

    //発言者
    [DynamoDBProperty]
    public string StPlayer{
        get;
        set;
    }

    //プレイヤー数
    [DynamoDBProperty]
    public int PNum{
        get;
        set;
    }
    //発言ID
    [DynamoDBProperty]
    public int StatementID{
        get;
        set;
    }

    [DynamoDBProperty]
    public string GameState{
        get;
        set;
    }
}