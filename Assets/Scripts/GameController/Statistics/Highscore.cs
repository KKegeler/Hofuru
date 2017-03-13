using System;
using System.Collections.Generic;

/// <summary>
/// Score data
/// </summary>
[Serializable]
public class Highscore
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
    private class SortDescendingHelper : IComparer<Highscore>
    {
        public int Compare(Highscore x, Highscore y)
        {
            if (x.score < y.score)
                return 1;

            if (x.score > y.score)
                return -1;

            return 0;
        }

    }

    public static IComparer<Highscore> SortDescending()
    {
        return new SortDescendingHelper();
    }
    #endregion

}