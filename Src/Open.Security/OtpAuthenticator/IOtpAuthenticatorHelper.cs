namespace Open.Security.OtpAuthenticator;

public interface IOtpAuthenticatorHelper
{
    public Task<byte[]> GenerateCode();
    public Task<string> ConvertCodeToString(byte[] code);
    public Task<bool> VerifyCode(byte[] secretKey, string code);
}
