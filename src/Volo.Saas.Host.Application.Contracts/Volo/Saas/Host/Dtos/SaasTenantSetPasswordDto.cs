using System;

namespace Volo.Saas.Host.Dtos;

[Serializable]
public class SaasTenantSetPasswordDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
