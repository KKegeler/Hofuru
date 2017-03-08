using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Framework.Log;

/// <summary>
/// Lädt und speichert Score-Daten
/// </summary>
public static class DataSerializer
{
    #region Variables
    private static List<Highscore> _scoreList = new List<Highscore>(6);
    private const string _FILE_NAME = "Scores.mango";
    private const ushort _SAVE_NUM = 5;
    #endregion

    #region Properties
    public static List<Highscore> ScoreList
    {
        get { return _scoreList; }
    }

    private static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, _FILE_NAME); }
    }
    #endregion

    /// <summary>
    /// Speichert Highscores in Datei
    /// </summary>
    public static void Save()
    {
        Highscore score = new Highscore(Statistics.Instance.FinalScore);
        _scoreList.Add(score);
        _scoreList.Sort(Highscore.SortDescending());

        if (_scoreList.Count > _SAVE_NUM)
            _scoreList.RemoveAt(_scoreList.Count - 1);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(FilePath);

        bf.Serialize(file, _scoreList);
        file.Close();
    }

    /// <summary>
    /// Lädt Highscores aus Datei
    /// </summary>
    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FilePath, FileMode.Open);

            _scoreList = (List<Highscore>)bf.Deserialize(file);
            file.Close();
        }

        if (_scoreList == null)
            _scoreList = new List<Highscore>();
    }

    /// <summary>
    /// Löscht Datei, falls vorhanden
    /// </summary>
    public static void Reset()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
            _scoreList.Clear();
        }
        else
            CustomLogger.LogWarningFormat("File \"{0}\" not found!\n", FilePath);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void TestLog()
    {
        if (_scoreList.Count == 0)
            CustomLogger.Log("No entries in ScoreList!\n");
        else
            for (int i = 0; i < _scoreList.Count; ++i)
                CustomLogger.LogFormat("{0}: {1}\n", i + 1, _scoreList[i].score);
    }

}