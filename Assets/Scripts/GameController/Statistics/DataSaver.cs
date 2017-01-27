using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class DataSaver
{
    #region Variablen
    private static List<Highscore> _scoreList = new List<Highscore>(6);
    private const string _fileName = "score.mango";
    #endregion

    #region Properties
    public static List<Highscore> ScoreList
    {
        get { return _scoreList; }
    }

    private static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, _fileName); }
    }
    #endregion

    public static void Save()
    {
        Highscore score = new Highscore(Statistics.Instance.Score);
        _scoreList.Add(score);
        _scoreList.Sort(Highscore.SortDescending());

        if (_scoreList.Count > 5)
            _scoreList.RemoveAt(_scoreList.Count - 1);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(FilePath);

        bf.Serialize(file, _scoreList);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FilePath, FileMode.Open);

            _scoreList = (List<Highscore>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void Reset()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }

}