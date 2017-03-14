using System;

/// <summary>
/// Score data
/// </summary>
[Serializable]
public class Highscore : IComparable<Highscore>
{
    #region Variables
    public readonly float score;
    #endregion

    #region Constructors
    public Highscore(float scoreVal)
    {
        score = scoreVal;
    }
    #endregion

    #region Sorting
    public int CompareTo(Highscore other)
    {
        if (score < other.score)
            return 1;

        if (score > other.score)
            return -1;

        return 0;
    }
    #endregion

}