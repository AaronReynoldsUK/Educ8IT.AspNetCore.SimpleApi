// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// Utility Class for passwords
    /// </summary>
    public class Passwords
    {
        /// <summary>
        /// Generate a random password of at least 8 characters
        /// </summary>
        /// <param name="length"></param>
        /// <param name="includeLowerCase"></param>
        /// <param name="includeNumbers"></param>
        /// <param name="includeSymbols"></param>
        /// <param name="includeUpperCase"></param>
        /// <returns></returns>
        public static string GeneratePassword(int length, bool includeLowerCase, bool includeNumbers, bool includeSymbols, bool includeUpperCase)
        {
            var __numbers = "0123456789";   // Number = 48-57
            var __lowerCase = "abcdefghijklmnopqrstuvwxyz"; // Lower-case = 97-122
            var __upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";   // Upper-case = 65-90
            var __symbols = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; // Symbols = 33-47,58-64,91-96,123-126

            if (length < 8)
                length = 8;

            string __password = "";
            Random __rnd = new Random();

            while (__password.Length < length)
            {
                if (includeLowerCase)
                    __password += __lowerCase[__rnd.Next(__lowerCase.Length)];

                if (includeUpperCase)
                    __password += __upperCase[__rnd.Next(__upperCase.Length)];

                if (includeNumbers)
                    __password += __numbers[__rnd.Next(__numbers.Length)];

                if (includeSymbols)
                    __password += __symbols[__rnd.Next(__symbols.Length)];
            }

            // Jumble the order and return the password
            return Jumble(__password);
        }

        /// <summary>
        /// Jumble the characters in a string
        /// </summary>
        /// <param name="stringToJumble"></param>
        /// <returns></returns>
        public static string Jumble(string stringToJumble)
        {
            char[] __characters = stringToJumble.ToCharArray();

            Random r = new Random();
            for (int i = __characters.Length - 1; i > 0; i--)
            {
                int __random = r.Next(i);
                var __temp = __characters[i];
                __characters[i] = __characters[__random];
                __characters[__random] = __temp;
            }

            return new string(__characters);
        }
    }
}
