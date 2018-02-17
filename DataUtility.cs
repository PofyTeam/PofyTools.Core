using System.IO;
using System.Text;
using UnityEngine;

public static class DataUtility
{
    public const string TAG = "<color=yellow><b>DataUtility: </b></color>";

    #region LOAD

    public static void LoadOverwrite (string fullPath, object objectToOverwrite, bool unscramble = false, bool decode = false)
    {
        if (objectToOverwrite == null)
        {
            Debug.LogWarningFormat ("{0}Object to overwrite is NULL! Aborting... (\"{1}\")", TAG, fullPath);
            return;
        }

        if (string.IsNullOrEmpty (fullPath))
        {
            Debug.LogWarningFormat ("{0}Invalid path! Aborting...", TAG);
            return;
        }

        if (!File.Exists (fullPath))
        {
            Debug.LogWarningFormat ("{0}File \"{1}\" not found! Aborting...", TAG, fullPath);
            return;
        }

        var json = File.ReadAllText (fullPath);

        json = (unscramble) ? DataUtility.UnScramble (json) : json;
        json = (decode) ? DataUtility.DecodeFrom64 (json) : json;

        JsonUtility.FromJsonOverwrite (json, objectToOverwrite);
    }

    //TODO: T Load

    #endregion

    #region SAVE

    public static void Save (string fullPath, object objectToSave, bool scramble = false, bool encode = false)
    {
        if (objectToSave == null)
        {
            Debug.LogWarningFormat ("{0}Object you are trying to save is NULL! Aborting... (\"{1}\")", TAG, fullPath);
            return;
        }

        if (string.IsNullOrEmpty (fullPath))
        {
            Debug.LogWarningFormat ("{0}Invalid path! Aborting...", TAG);
            return;
        }

        var json = JsonUtility.ToJson (objectToSave);

        json = (encode) ? DataUtility.EncodeTo64 (json) : json;
        json = (scramble) ? DataUtility.Scramble (json) : json;

        File.WriteAllText (fullPath, json);
    }

    #endregion

    #region SCRAMBLE

    static string Scramble (string toScramble)
    {
        StringBuilder toScrambleSB = new StringBuilder (toScramble);
        StringBuilder scrambleAddition = new StringBuilder (toScramble.Substring (0, toScramble.Length / 2 + 1));
        for (int i = 0, j = 0; i < toScrambleSB.Length; i = i + 2, ++j)
        {
            scrambleAddition[j] = toScrambleSB[i];
            toScrambleSB[i] = 'c';
        }

        StringBuilder finalString = new StringBuilder ();
        int totalLength = toScrambleSB.Length;
        string length = totalLength.ToString ();
        finalString.Append (length);
        finalString.Append ("!");
        finalString.Append (toScrambleSB.ToString ());
        finalString.Append (scrambleAddition.ToString ());

        return finalString.ToString ();
    }

    static string UnScramble (string scrambled)
    {
        int indexOfLenghtMarker = scrambled.IndexOf ("!");
        string strLength = scrambled.Substring (0, indexOfLenghtMarker);
        int lengthOfRealData = int.Parse (strLength);
        StringBuilder toUnscramble = new StringBuilder (scrambled.Substring (indexOfLenghtMarker + 1, lengthOfRealData));
        string substitution = scrambled.Substring (indexOfLenghtMarker + 1 + lengthOfRealData);
        for (int i = 0, j = 0; i < toUnscramble.Length; i = i + 2, ++j)
            toUnscramble[i] = substitution[j];

        return toUnscramble.ToString ();
    }

    #endregion

    #region ENCODE

    public static string EncodeTo64 (string toEncode)
    {
        byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes (toEncode);
        string returnValue = System.Convert.ToBase64String (toEncodeAsBytes);
        return returnValue;
    }

    public static string DecodeFrom64 (string encodedData)
    {
        byte[] encodedDataAsBytes = System.Convert.FromBase64String (encodedData);
        string returnValue = System.Text.Encoding.Unicode.GetString (encodedDataAsBytes);
        return returnValue;
    }

    #endregion


    #region Textures

    public static void IncrementSaveToPNG (string filePath, string fileName, Texture2D texture)
    {
        int count = 0;

        if (texture == null)
        {
            Debug.LogWarningFormat ("{0}Texture you are trying to save is NULL! Aborting... (\"{1}\")", TAG, fileName);
            return;
        }

        if (string.IsNullOrEmpty (filePath) || string.IsNullOrEmpty (fileName))
        {
            Debug.LogWarningFormat ("{0}Invalid path! Aborting...", TAG);
            return;
        }

        if (filePath[filePath.Length - 1] != '/' && fileName[0] != '/')
        {
            filePath += "/";
        }

        while (File.Exists (filePath + fileName + count + ".png"))
        {
            count++;
        }

        SaveToPNG (filePath + fileName + count + ".png", texture);
    }

    public static void SaveToPNG (string fullPath, Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogWarningFormat ("{0}Texture you are trying to save is NULL! Aborting... (\"{1}\")", TAG, fullPath);
            return;
        }

        if (string.IsNullOrEmpty (fullPath))
        {
            Debug.LogWarningFormat ("{0}Invalid path! Aborting...", TAG);
            return;
        }

        File.WriteAllBytes (fullPath, texture.EncodeToPNG ());
    }

    #endregion
}