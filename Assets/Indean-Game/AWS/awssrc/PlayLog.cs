using System;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Indean-Gamedb")]
public class PlayLog
{
    [DynamoDBHashKey]
    public int RoomID { 
        get; 
        set; 
    }

    #region PlayerNames
    //プレイヤー01の名前
    [DynamoDBProperty]
    public string P1N { 
        get; 
        set; 
    }

    //プレイヤー02の名前
    [DynamoDBProperty]
    public string P2N { 
        get; 
        set; 
    }

    //プレイヤー03の名前
    [DynamoDBProperty]
    public string P3N { 
        get; 
        set; 
    }

    //プレイヤー04の名前
    [DynamoDBProperty]
    public string P4N { 
        get; 
        set; 
    }
    #endregion

    #region PlayerQuestion
    //Player01のお題
    [DynamoDBProperty]
    public string P1Q{
        get; 
        set; 
    }

    //Player02のお題
    [DynamoDBProperty]
    public string P2Q{
        get; 
        set; 
    }

    //Player03のお題
    [DynamoDBProperty]
    public string P3Q{
        get; 
        set; 
    }

    //Player04のお題
    [DynamoDBProperty]
    public string P4Q{
        get; 
        set; 
    }
    #endregion


    #region Player Preparation
    //プレイヤー1の準備確認
    [DynamoDBProperty]
    public bool P1Pre{
        get;
        set;
    }

    //プレイヤー2の準備確認
    [DynamoDBProperty]
    public bool P2Pre{
        get;
        set;
    }

    //プレイヤー3の準備確認
    [DynamoDBProperty]
    public bool P3Pre{
        get;
        set;
    }

    //プレイヤー1の準備確認
    [DynamoDBProperty]
    public bool P4Pre{
        get;
        set;
    }

    #endregion

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

    //発言の更新
    [DynamoDBProperty]
    public bool StUpdate{
        get;
        set;
    }

    //現在更新が終了したプレイヤー数
    [DynamoDBProperty]
    public int UpNum{
        get;
        set;
    }

    //プレイヤー数
    [DynamoDBProperty]
    public int PNum{
        get;
        set;
    }
}