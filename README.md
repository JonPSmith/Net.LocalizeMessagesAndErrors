# Net.LocalizeMessagesAndErrors

This library provides extra code to make it easier to support in different languages in your .NET application (_known as [localization](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization#make-the-apps-content-localizable) in .NET_). The code in this library _wraps_ the .NET's localization services with a nicer front-end that makes the localization parts easier to code and understand.

This library is an open-source library under the MIT licence and the NuGet package can be [found here](https://www.nuget.org/packages/Net.LocalizeMessagesAndErrors). The documentation can be found in the [GitHub wiki](https://github.com/JonPSmith/Net.LocalizeMessagesAndErrors/wiki) and see [ReleaseNotes](https://github.com/JonPSmith/Net.LocalizeMessagesAndErrors/blob/main/ReleaseNotes.md) for details of changes.

## List of versions and which .NET framework they support

The versions of this library has changed to make it easier to create a new version when a new release of .NET farmework. Now the first number defines the .NET version, e.g EfCore.TestSupport version 8.?.? only runs on .NET 8. 

- Version 2.?.?: Supports NET 6, 7 and 8
- Version 1.0.?: Supports NET 6 and 7

## Why I built this library

A user of the of my library [AuthPermissions.AspNetCore](https://github.com/JonPSmith/AuthPermissions.AspNetCore) (shortened to AuthP) wanted support for multiple languages (known as _localization_) added. I looked at the .NET localization service and here the main issues I had:

1. **Makes your code harder to understand**: The .NET's Localization replaces the message string to represent a message, which makes the code harder to understand, e.g. the error message “When using sharding the the hasOwnDb parameter must not be null.” would become a key something like “ShardingHasOwnDb”.
2. **Makes it harder add localization to an existing app**: Localization isn’t usually added at the start of a project (see MVP - [Minimum Viable Product](https://www.productplan.com/glossary/minimum-viable-product/) approach) so you most likely have code with messages built into your code at to start. .NET's Localization requires to remove the messages in your code and put them in the resource files - that's a lot of work.
3. **Missing localized messages doesn't provide a good user experience**:  If you don't add a localizes message to the resource files, the the .NET localisation shows the key, e.g. “ShardingHasOwnDb”, which isn’t that useful to a user.
4. **Makes a NuGet library hard to use / understand**: Using a NuGet library that uses .NET's Localization would force you to set up the localization and it's resource files, which instantly makes the library hard to setup and understand.

It took me some time to design a library called [Net.LocalizeMessagesAndErrors](https://github.com/JonPSmith/Net.LocalizeMessagesAndErrors) that solves issues above (and more). Plus, using the library on my AuthP library (which has hundreds of errors / messages) provides lots feedback on the best design for the library. 

You can find [list of the improvements](https://github.com/JonPSmith/Net.LocalizeMessagesAndErrors/wiki/Why-I-build-this-localization-library%3F#the-full-list-of-the-benefits-of-the-localizewithdefault-approach) that this library provides better handling localization in .NET applications.
