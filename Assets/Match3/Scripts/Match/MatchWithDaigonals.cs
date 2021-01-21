public class MatchWithDaigonals : Match
{
    private bool isDiagonalMatchRight;
    private bool isDiagonalMatchLeft;

    public override void SetMatch(MatchType matchType, bool value = true)
    {
        switch (matchType)
        {
            case MatchType.DIAGONAL_LEFT:
                isDiagonalMatchLeft = value;
                break;
            case MatchType.DIAGONAL_RIGHT:
                isDiagonalMatchRight = value;
                break;
            case MatchType.HORIZONTAL:
                isHorizontalMatch = value;
                break;
            case MatchType.VERTICAL:
                isVerticalMatch = value;
                break;
        }
    }

    public override bool HasMatch(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.DIAGONAL_LEFT:
                return isDiagonalMatchLeft;
            case MatchType.DIAGONAL_RIGHT:
                return isDiagonalMatchRight;
            case MatchType.HORIZONTAL:
                return isHorizontalMatch;
            case MatchType.VERTICAL:
                return isVerticalMatch;
            default:
                return false;
        }
    }
}
