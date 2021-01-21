
public interface IBonusablePieceModel : IPieceModel
{
    Bonus Bonus { get; set; }
    void ActivateBonus();
}
