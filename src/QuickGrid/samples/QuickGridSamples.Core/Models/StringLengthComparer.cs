namespace QuickGridSamples.Core.Models;

public class StringLengthComparer : IComparer<string>
{
    public static readonly StringLengthComparer Instance = new StringLengthComparer();

    public int Compare(string x, string y)
    {
        // longer first
        return -x.Length.CompareTo(y.Length);
    }
}

