﻿using Newtonsoft.Json;
using System.Collections.Generic;

public class ReCaptchaClass
{
    public static string Validate(string EncodedResponse)
    {
        var client = new System.Net.WebClient();

        string PrivateKey = "6Lc_938UAAAAAByhFteOineB5zQAffZjxdN9P-c6" /*"6Ld6jX8UAAAAAFPLB7E2PBsOkmGyt14O9ToQh4tO"*/;

        var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

        var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

        return captchaResponse.Success.ToLower();
    }

    [JsonProperty("success")]
    public string Success
    {
        get { return m_Success; }
        set { m_Success = value; }
    }

    private string m_Success;
    [JsonProperty("error-codes")]
    public List<string> ErrorCodes
    {
        get { return m_ErrorCodes; }
        set { m_ErrorCodes = value; }
    }


    private List<string> m_ErrorCodes;
}