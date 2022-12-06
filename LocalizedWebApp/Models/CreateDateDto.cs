// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizedWebApp.Models;

public class CreateDateDto
{
    public int Day { get; set; }
    public string? Month { get; set; }
    public int Year { get; set; }
}