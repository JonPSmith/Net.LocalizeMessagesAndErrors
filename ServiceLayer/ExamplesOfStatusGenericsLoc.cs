﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using StatusGeneric;
using System.Globalization;

namespace ServiceLayer;

[LocalizeSetClassName("StatusExamples")]
public class ExamplesOfStatusGenericsLoc<TResource>
{
    private readonly IDefaultLocalizer<TResource> _defaultLocalizer;

    public ExamplesOfStatusGenericsLoc(IDefaultLocalizer<TResource> defaultLocalizer)
    {
        _defaultLocalizer = defaultLocalizer;
    }

    /// <summary>
    /// This method shows the four main properties in the <see cref="IStatusGeneric"/>
    /// with an explanation on what effects them.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public IStatusGeneric CheckNull(string? month)
    {
        //The StatusGenericLocalizer constructor takes:
        //An instance of the IDefaultLocalizer<TResource> service
        var status = new StatusGenericLocalizer(_defaultLocalizer);

        //add error and return immediately
        if (month == null)
            //You can return just an error message, but adding the property name will improve
            //the error feedback in ASP.NET Core etc. (Note the use of the CamelToPascal method)
            return status.AddErrorString("NullParam".JustThisLocalizeKey(this), 
                "The input must not be null.", nameof(month).CamelToPascal());

        //This sets the success message 
        status.SetMessageString("Success".ClassMethodLocalizeKey(this, false), 
            "Successful completion.");

        //If no errors were added then:
        //- The Message contain the localized message you set.
        //- HasErrors and IsValid properties are false and true respectively
        //If there are errors then:
        //- the Message has the format of "Failed with NN errors", but localized.
        //- HasErrors and IsValid properties are true and false respectively
        return status;
    }

    /// <summary>
    /// The method shows how you can return a value from a method using <see cref="IStatusGeneric"/>
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public IStatusGeneric<string> StatusGenericWithResult(int year)
    {
        var status = new StatusGenericLocalizer<string>(_defaultLocalizer);

        //add error and return immediately
        if (year < 0)
            return status.AddErrorString("NumberNegative".ClassMethodLocalizeKey(this, false),
                "The property should not be negative.", nameof(year).CamelToPascal());

        //This sets the Result property in the generic status
        status.SetResult(year.ToString());

        //If no errors were added then the returned status contains the 
        //the value set by status.SetResult method
        //If there are errors, then the result is set to default.
        return status;
    }


    /// <summary>
    /// This is an example of using the <see cref="IStatusGeneric.CombineStatuses"/> method to
    /// combine the status of other methods that return a <see cref="IStatusGeneric"/> or <see cref="IStatusGeneric{TResult}"/> result.
    /// </summary>
    /// <param name="day"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public IStatusGeneric<DateTime> CreateDate(int day, string? month, int year)
    {
        var status = new StatusGenericLocalizer<DateTime>(_defaultLocalizer);

        //CombineStatuses adds the status of another method that also uses IStatusGeneric.
        //Errors in the called method are added to this method's status.
        //If the called method has set a Message, then this method's Message
        //is updated to the called method's Message.
        status.CombineStatuses(CheckNull(month));

        //The CombineStatuses also returns the combined IStatusGeneric, so you can assess its properties
        if (status.CombineStatuses(StatusGenericWithResult(year)).HasErrors)
            return status;

        try
        {
            var dateInput = $"{month}, {day}, {year}";
            var parsedDate = DateTime.Parse(dateInput, new CultureInfo("en-US"));
            status.SetMessageFormatted("Success".ClassMethodLocalizeKey(this, false),
                $"Successfully created the date {parsedDate:D}.");
            return status.SetResult(parsedDate);
        }
        catch (FormatException)
        {
            return status.AddErrorFormatted("BadDate".ClassMethodLocalizeKey(this, false),
                $"The day {day}, month {month}, year {year} doesn't turn into a valid date.");
        }


    }
}