using System;

public class BookRecommendationMessage
{
    public string[] SendTo { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string EmailTitle { get; set; } = default!;
    public string BookLink { get; set; } = default!;
}
