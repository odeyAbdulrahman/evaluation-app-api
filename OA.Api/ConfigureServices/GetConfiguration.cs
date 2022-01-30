using evaluation_app;
using Microsoft.Extensions.Configuration;

namespace OA.Api.ConfigureServices
{
    public static class GetConfiguration
    {
        public static DecorationSettings DecorationSettings() => Settings<DecorationSettings>(nameof(DecorationSettings));
        public static FireBaseSettings FireBaseSettings(this IConfiguration configuration) => Settings<FireBaseSettings>(configuration, nameof(FireBaseSettings));
        public static RecaptchaSettings RecaptchaSettings(this IConfiguration configuration) => Settings<RecaptchaSettings>(configuration, nameof(RecaptchaSettings));
        public static SMSGatewaySettings SMSGatewaySettings(this IConfiguration configuration) => Settings<SMSGatewaySettings>(configuration, nameof(SMSGatewaySettings));
        #region Configuration funcation
        private static T Settings<T>(string SettingClass) where T : class
        {
            var Settings = Startup.StaticConfig.GetSection(SettingClass);
            return Settings.Get<T>();
        }
        private static T Settings<T>(IConfiguration configuration, string SettingClass) where T : class
        {
            var Settings = configuration.GetSection(SettingClass);
            return Settings.Get<T>();
        }
        #endregion
    }
}
