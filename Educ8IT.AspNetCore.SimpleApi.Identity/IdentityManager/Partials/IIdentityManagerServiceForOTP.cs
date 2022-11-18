using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial interface IIdentityManagerService
    {
        #region For OTP

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="step"></param>
        /// <param name="mode"></param>
        /// <param name="totpSize"></param>
        /// <param name="timeCorrection"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GenerateOTC(
            byte[] secret,
            int step = 30,
            OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1,
            int totpSize = 6,
            OtpNet.TimeCorrection timeCorrection = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dtValue"></param>
        /// <param name="secret"></param>
        /// <param name="step"></param>
        /// <param name="mode"></param>
        /// <param name="totpSize"></param>
        /// <param name="timeCorrection"></param>
        /// <returns></returns>
        bool VerifyOTC(
            string value,
            DateTime dtValue,
            byte[] secret,
            int step = 30,
            OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1,
            int totpSize = 6,
            OtpNet.TimeCorrection timeCorrection = null);

        #endregion
    }
}
