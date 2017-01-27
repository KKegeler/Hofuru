using System;
using System.Collections.Generic;

[Serializable]
public class Highscore
{
    public readonly float score;
    
    public Highscore(float scoreVal)
    {
        score = scoreVal;
    }

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

}