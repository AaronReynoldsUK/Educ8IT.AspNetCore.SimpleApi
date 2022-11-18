using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial class IdentityManagerService
    {
        #region For OTP

        /// <inheritdoc/>
        public System.Threading.Tasks.Task<string> GenerateOTC(
            byte[] secret,
            int step = 30,
            OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1,
            int totpSize = 6,
            OtpNet.TimeCorrection timeCorrection = null)
        {
            var __totp = new OtpNet.Totp(secret, step, mode, totpSize, timeCorrection);

            return System.Threading.Tasks.Task.FromResult(__totp.ComputeTotp());
        }

        /// <inheritdoc/>
        public bool VerifyOTC(
            string value,
            DateTime dtValue,
            byte[] secret,
            int step = 30,
            OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1,
            int totpSize = 6,
            OtpNet.TimeCorrection timeCorrection = null)
        {
            var __totp = new OtpNet.Totp(secret, step, mode, totpSize, timeCorrection);

            return __totp.VerifyTotp(dtValue, value, out long timeStepMatched);
        }

        #endregion
    }
}
