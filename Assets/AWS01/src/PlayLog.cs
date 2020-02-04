using System;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("test-aws")]
public class PlayLog
{
    [DynamoDBHashKey]
    public int id { 
        get; 
        set; 
    }

    [DynamoDBProperty]
    public string username { 
        get; 
        set; 
    }

    [DynamoDBProperty]
    public DateTime date { 
        get; 
        set; 
    }
}