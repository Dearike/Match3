public interface IMatch
{
    void SetMatch(MatchType matchType, bool value = true);
    bool HasMatch(MatchType matchType);
}
