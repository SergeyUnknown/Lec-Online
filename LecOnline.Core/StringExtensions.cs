// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Extensions for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Strip string in case if it is longer then specified number.
        /// </summary>
        /// <param name="data">String where to put ellipsis.</param>
        /// <param name="length">Max length of the string.</param>
        /// <returns>String which is no longer then specified length.</returns>
        public static string Ellipsis(this string data, int length)
        {
            if (data.Length > length)
            {
                data = data.Substring(0, length - 1) + "\u2026";
            }

            return data;
        }
    }
}
