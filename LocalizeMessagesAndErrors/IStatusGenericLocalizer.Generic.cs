// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is a version of <see cref="IStatusGenericLocalizer"/> that contains a result.
/// Useful if you want to return something with the status.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStatusGenericLocalizer<out T> : IStatusGeneric
{
    /// <summary>
    /// This contains the return result, or if there are errors it will return default(T)
    /// </summary>
    T Result { get; }


}