using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbAppRequestData
{
    // When GET apprequests FB will return apprequest id with format RequestId_LoggedInAppScopedUserId
    // When Delete apprequests will have two case:
    //     If delete only one app request FB will return {"success":true/false} if no problem happen else is error message
    //     If delete mutiple app request FB will return Dictionary have item format like this [Key: RequestId:RealUserId Value: true/false] if no problem else is error message
    public string requestId; 
    public string requestIdPrefix;
    public string data;
    public string senderName;
    public string senderId;

    public FbAppRequestData(string requestId, string data, string senderId, string senderName)
    {
        this.requestId = requestId;
        this.requestIdPrefix = requestId.Split('_')[0];
        this.data = data;
        this.senderId = senderId;
        this.senderName = senderName;
    }

    public override string ToString()
    {
        return string.Format("Request ID: {0}, Sender ID: {1}, Sender Name: {2}, Data: {3}", requestId, senderId, senderName, data);
    }
}
