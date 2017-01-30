using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

/// <summary>
/// Speichert und lädt Score-Daten
/// </summary>
public static class DataSerializer
{
    #region Variablen
    private static List<Highscore> _scoreList = new List<Highscore>(6);
    private const string _fileName = "Scores.mango";
    private const ushort _SAVE_NUM = 5;
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

    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FilePath, FileMode.Open);

            _scoreList = (List<Highscore>)bf.Deserialize(file);
            file.Close();     
        }
        else
            Debug.LogWarningFormat("File \"{0}\" not found!\n", FilePath);

        if (_scoreList == null)
            _scoreList = new List<Highscore>();

        // TestLog();
    }

    public static void Reset()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
        else
            Debug.LogWarningFormat("File \"{0}\" not found!\n", FilePath);
    }

    private static void TestLog()
    {
        if (_scoreList.Count <= 0)
            Debug.Log("No entrys in ScoreList!\n");

        for (int i = 0; i < _scoreList.Count; ++i)
            Debug.LogFormat("{0}: {1}", i + 1, _scoreList[i].score);
    }

}