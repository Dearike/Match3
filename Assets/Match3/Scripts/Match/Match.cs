public enum MatchType
{
    HORIZONTAL,
    VERTICAL,
    DIAGONAL_RIGHT,
    DIAGONAL_LEFT
}

public class Match : IMatch
{
    protected bool isHorizontalMatch = false;
    protected bool isVerticalMatch = false;

    public virtual void SetMatch(MatchType matchType, bool value = true)
    {
        switch (matchType)
        {
            case MatchType.HORIZONTAL:
                isHorizontalMatch = value;
                break;
            case MatchType.VERTICAL:
                isVerticalMatch = value;
                break;
        }
    }

    public virtual bool HasMatch(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.HORIZONTAL:
                return isHorizontalMatch;
            case MatchType.VERTICAL:
                return isVerticalMatch;
            default:
                return false;
        }
    }
}
