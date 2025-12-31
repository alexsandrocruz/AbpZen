using System;

namespace Volo.Abp.Gdpr;

[Serializable]
public class DownloadTokenResultDto
{
    public string Token { get; set; }
}