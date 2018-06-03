using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class CPlayerPrefs
{
#if !UNITY_WSA
    #region New PlayerPref Stuff
    /// <summary>
    /// Returns true if key exists in the preferences.
    /// </summary>
    public static bool HasKey (string key)
	{
		// Get hashed key
		string cKey = hashedKey (key);
		
		return PlayerPrefs.HasKey (cKey);
	}
	
	/// <summary>
	/// Removes key and its corresponding value from the preferences.
	/// </summary>
	public static void DeleteKey (string key)
	{
		// Get hashed key
		string cKey = hashedKey (key);
		
		PlayerPrefs.DeleteKey (cKey);
	}
	
	/// <summary>
	/// Removes all keys and values from the preferences. Use with caution.
	/// </summary>
	public static void DeleteAll ()
	{
		PlayerPrefs.DeleteAll ();
	}
	
	/// <summary>
	/// Writes all modified preferences to disk.
	/// </summary>
	public static void Save ()
	{
		PlayerPrefs.Save ();
	}
    #endregion
	
    #region New PlayerPref Setters
	/// <summary>
	/// Sets the value of the preference identified by key.
	/// </summary>
	public static void SetInt (string key, int val)
	{	
		// Get crypted key
		string cKey = hashedKey (key);
		
		int cryptedInt = val;
		
		// If enabled use xor algo
		if (_useXor) {
			// Get crypt helper values
			int xor = computeXorOperand (key, cKey);
			int ad = computePlusOperand (xor);
		
			// Compute crypted int
			cryptedInt = (val + ad) ^ xor;
		}
		
		// If enabled use rijndael algo
		if (_useRijndael) {
			// Save
			PlayerPrefs.SetString (cKey, encrypt (cKey, string.Empty + cryptedInt));
		} else {
			PlayerPrefs.SetInt (cKey, cryptedInt);
		}
		
	}
	
	/// <summary>
	/// Sets the value of the preference identified by key.
	/// </summary>
	public static void SetLong (string key, long val)
	{
		SetString(key, val.ToString());
	}
	
	/// <summary>
	/// Sets the value of the preference identified by key.
	/// </summary>
	public static void SetString (string key, string val)
	{
		// Get crypted key
		string cKey = hashedKey (key);
		
		string cryptedString = val;
		// If enabled use xor algo
		if (_useXor) {
			// Get crypt helper values
			int xor = computeXorOperand (key, cKey);
			int ad = computePlusOperand (xor);
		
			// Compute crypted string
			cryptedString = "";
			foreach (char c in val) {
				char cryptedChar = (char)(((int)c + ad) ^ xor);
				cryptedString += cryptedChar;
			}
		}
		
		// If enabled use rijndael algo
		if (_useRijndael) {
			// Save
			PlayerPrefs.SetString (cKey, encrypt (cKey, cryptedString));
		} else {
			PlayerPrefs.SetString (cKey, cryptedString);
		}
	}
	
	/// <summary>
	/// Sets the value of the preference identified by key.
	/// </summary>
	public static void SetFloat (string key, float val)
	{
		SetString (key, val.ToString ());
	}

    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }
    #endregion
	
    #region New PlayerPref Getters
	/// <summary>
	/// Returns the value corresponding to key in the preference file if it exists.
	/// If it doesn't exist, it will return defaultValue.
	/// </summary>
	public static int GetInt (string key, int defaultValue)
	{
		// Get crypted key
		string cKey = hashedKey (key);
		
		// If the key doesn't exists, return defaultValue
		if (!PlayerPrefs.HasKey (cKey)) {
			return defaultValue;
		}
		
		// Read storedPref
		int storedPref;
		if(_useRijndael) {
			storedPref = int.Parse (decrypt (cKey));
		} else {
			storedPref = PlayerPrefs.GetInt(cKey);
		}
		
		// If xor algo enabled
		int realValue = storedPref;
		if(_useXor) {
			// Get crypt helper values
			int xor = computeXorOperand (key, cKey);
			int ad = computePlusOperand (xor);
			
			// Compute real value
			realValue = (xor ^ storedPref) - ad;
		}
		
		return realValue;
	}
	
	public static int GetInt(string key) {
		return GetInt(key, 0);
	}

	/// <summary>
	/// Returns the value corresponding to key in the preference file if it exists.
	/// If it doesn't exist, it will return defaultValue.
	/// </summary>
	public static long GetLong (string key, long defaultValue)
	{
		return long.Parse(GetString(key, defaultValue.ToString()));
	}
	
	public static long GetLong(string key) {
		return GetLong(key, 0);
	}
	
	/// <summary>
	/// Returns the value corresponding to key in the preference file if it exists.
	/// If it doesn't exist, it will return defaultValue.
	/// </summary>
	public static string GetString (string key, string defaultValue)
	{
		// Get crypted key
		string cKey = hashedKey (key);
				
		// If the key doesn't exists, return defaultValue
		if (!PlayerPrefs.HasKey (cKey)) {
			return defaultValue;
		}
		
		// Read storedPref
		string storedPref;
		if(_useRijndael) {
			storedPref = decrypt (cKey);
		} else {
			storedPref = PlayerPrefs.GetString(cKey);
		}
		
		// XOR algo enabled?
		string realString = storedPref;
		if(_useXor) {
			// Get crypt helper values
			int xor = computeXorOperand (key, cKey);
			int ad = computePlusOperand (xor);
			
			// Compute real value
			realString = "";
			foreach (char c in storedPref) {
				char realChar = (char)((xor ^ c) - ad);
				realString += realChar;
			}
		}
		
		return realString;
	}
	
	public static string GetString(string key) {
		return GetString(key, "");
	}
	
	/// <summary>
	/// Returns the value corresponding to key in the preference file if it exists.
	/// If it doesn't exist, it will return defaultValue.
	/// </summary>
	public static float GetFloat (string key, float defaultValue)
	{
		return float.Parse (GetString (key, defaultValue.ToString ()));
	}
	
	public static float GetFloat(string key) {
		return GetFloat(key, 0);
	}

    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (!HasKey(key))
            return defaultValue;
            
        return GetInt(key) == 1;
    }
    #endregion

    #region Double
    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, DoubleToString(value));
    }

    public static double GetDouble(string key, double defaultValue)
    {
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }

    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }

    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }
    #endregion

    #region Crypto
    private static string encrypt (string cKey, string data)
	{
		return ZKW.CryptoPlayerPrefs.Helper.EncryptString (data, getEncryptionPassword (cKey));
	}
		
	private static string decrypt (string cKey)
	{
		return ZKW.CryptoPlayerPrefs.Helper.DecryptString (PlayerPrefs.GetString (cKey), getEncryptionPassword (cKey));
	}
		
	private static Dictionary<string, string> keyHashs;

	private static string hashedKey (string key)
	{
        if (!_useHashKey) return key;
		// Initialize HashKey-Dictionary
		if (keyHashs == null) {
			keyHashs = new Dictionary<string, string> ();
		}
		
		// Check if hashed key already was computed
		if (keyHashs.ContainsKey (key)) {
			// Return computed key
			return keyHashs [key];
		}
		
		// Create hashed key and add to dictionary
		string cKey = hashSum (key);
		keyHashs.Add (key, cKey);
		
		return cKey;
	}
	
	private static Dictionary<string, int> xorOperands;

	private static int computeXorOperand (string key, string cryptedKey)
	{
		if (xorOperands == null) {
			xorOperands = new Dictionary<string, int> ();
		}
		
		if (xorOperands.ContainsKey (key)) {
			return xorOperands [key];
		}
		
		int xorOperand = 0;
		foreach (char c in cryptedKey) {
			xorOperand += (int)c;
		}
		xorOperand += salt;
		
		xorOperands.Add (key, xorOperand);
		return xorOperand;
	}
	
	private static int computePlusOperand (int xor)
	{
		return xor & xor << 1;
	}
		
	public static string hashSum (string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
		byte[] bytes = ue.GetBytes (strToEncrypt);

		
		byte[] hashBytes = ZKW.CryptoPlayerPrefs.Helper.hashBytes (bytes);
 
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
 
		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
		}
 
		return hashString.PadLeft (32, '0');
	}

	private static string getEncryptionPassword (string pw)
	{
		return hashSum (pw + salt);
	}
    #endregion
	
    #region Settings
	private static int salt = int.MaxValue;
	
	/// <summary>
	/// Sets the salt. 
	/// Define this before use of the CryptoPlayerPrefs Class for your project. 
	/// NEVER change this again for this project!
	/// </summary>
	/// <param name='s'>
	/// Salt value
	/// </param>
	public static void setSalt (int s)
	{
		salt = s;
	}
	
	private static bool _useRijndael = true;
	
	/// <summary>
	/// Sets if Rijndael Algo should be used. 
	/// Define this before use of the CryptoPlayerPrefs Class for your project. 
	/// NEVER change this again for this project!
	/// </summary>
	/// <param name='use'>
	/// Use Rijndael or not
	/// </param>
	public static void useRijndael (bool use)
	{
		_useRijndael = use;
	}

    private static bool _useHashKey = false;
	private static bool _useXor = false;
	
	/// <summary>
	/// Sets if XOR Algo should be used. 
	/// Define this before use of the CryptoPlayerPrefs Class for your project. 
	/// NEVER change this again for this project!
	/// </summary>
	/// <param name='use'>
	/// Use XOR Algo or not
	/// </param>
	public static void useXor (bool use)
	{
		_useXor = use;
	}
    #endregion
#else
    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }

    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }

    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, DoubleToString(value));
    }
    
    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }

    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }

    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static double GetDouble(string key, double defaultValue)
    {
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }

    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (!HasKey(key))
            return defaultValue;

        return GetInt(key) == 1;
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void useRijndael(bool useRijndael)
    {
        // Nothing.
    }
#endif
}

