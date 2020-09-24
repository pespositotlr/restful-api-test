using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBankingRESTfulService.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace InternetBankingRESTfulService
{
    [Route("/")]
    [ApiController]
    public class CustomController : ControllerBase, IInternetBankingApi
    {
        [HttpGet]
        [Route("bank/api/calc/MD5/{value}")]
        [Route("bank/api/calc/{value}/MD5")]
        /// <summary>
        /// The CalculateMD5 should calculate the MD5 hash for the data passed as a function parameter.
        /// The MD5 hash should be returned as hex characters(the ex for test-string-1 should be 7FE8C14D5E3D1CFB648F77F05766A013).
        /// The passed parameter will never be empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CalculateMD5(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputValueBytes = Encoding.ASCII.GetBytes(value);
                byte[] computedHashBytes = md5.ComputeHash(inputValueBytes);

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < computedHashBytes.Length; i++)
                {
                    stringBuilder.Append(computedHashBytes[i].ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }

        [HttpGet]
        [Route("bank/api-version")]
        [Route("bank/api/version")]
        /// <summary>
        /// The GetApiVersion method should return the current version (1.0) in the following format: YYYY.MM.DD.1.0, where:
        /// YYYY is the current year(UTC time),
        /// MM is the current month(UTC time),
        /// DD is the current day(UTC time).
        /// </summary>
        /// <returns></returns>
        public string GetApiVersion()
        {
            DateTime utcNow = DateTime.UtcNow;

            return utcNow.Year + "." + utcNow.Month.ToString().PadLeft(2, '0') + "." + utcNow.Day.ToString().PadLeft(2, '0') + ".1.0";
        }

        [HttpGet]
        [Route("bank/api/password/strong/{password}")]
        [Route("bank/api/is-password-strong/{password}")]
        /// <summary>
        /// The IsPasswordStrong should check if the passed password (as a parameter) is strong.
        /// The password will be recognized as strong if it is at least eight characters long and contains at least 
        /// one uppercase letter, 
        /// one lowercase letter, 
        /// one number 
        /// and at least one character other than an uppercase letter, a lowercase letter or a number.
        /// The password cannot contain white-space characters.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsPasswordStrong(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasNumber = false;
            bool hasSpecialCharacter = false;

            foreach(char c in password)
            {
                if (c == ' ')
                    return false;

                var isUpper = Char.IsUpper(c);
                var isLower = Char.IsLower(c);
                var isNumber = Char.IsDigit(c);

                if (isUpper)
                    hasUppercase = true;

                if (isLower)
                    hasLowercase = true;

                if (isNumber)
                    hasNumber = true;

                if (!isUpper && !isLower && !isNumber)
                {
                    hasSpecialCharacter = true;
                }
            }

            if (hasUppercase && hasLowercase && hasNumber && hasSpecialCharacter)
                return true;

            return false;
        }
    }
}