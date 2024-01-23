using STU;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager> {
    private const string  FILE_DATA_NAME = "GameData.dat";
    private string pathFlie => Path.Combine(Application.persistentDataPath, FILE_DATA_NAME);
    private PlayerData playerData;

    public PlayerData PlayerData 
    {
        get 
        {
            if(playerData != null) 
            {
                return playerData;
            }
            
            else 
            {
                LoadData();
                return playerData;
            }
        }
    }

    #region Datas
    private const string ITEMS_PATH = "Items";
    [SerializeField] private List<ItemData> lstItem = new List<ItemData>();
    public IEnumerable<ItemData> LstItem => lstItem;
    
    private const string LEVELMAPS_PATH = "Maps";
    [SerializeField] private List<LevelMap> lstMap = new List<LevelMap>();
    public int LevelMapMax => 30;// lstMap.Count - 1;
    private const string WEAPONS_PATH = "Weapons";
    [SerializeField] private List<WeaponData> lstWeapon = new List<WeaponData>();
    public IEnumerable<WeaponData> LstWeapon => lstWeapon;
    #endregion

    protected override void OnAwake() {
        base.OnAwake();
        LoadItemData();
        LoadWeaponData();
        LoadData();
        LoadLevelMap();
        playerData.Update();
    }

    private void Start() {

    }

    #region SaveAndLoadPlayer
    public void SaveData() 
    {
        //PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(playerData));
        string data = JsonUtility.ToJson(playerData);
        //if(!Directory.Exists(FILE_DATA_PATH)) {
        //    Debug.LogError("CreateFile");
        //    Directory.CreateDirectory(FILE_DATA_PATH);
        //}

        try 
        {
            using(StreamWriter writer = File.CreateText(pathFlie)) 
            {
                writer.Write(data);
                Debug.Log($"[DATA] Write file completed.\n <path>: {pathFlie}\n <content>: {data}");
                writer.Close();
            }
        } 
        catch(Exception e) 
        {
            Debug.LogError($"[DATA] Write file failed.\n <path>: {pathFlie}\n <error>: {e}");
        }
    }
    public void LoadData() 
    {
        if(File.Exists(pathFlie)) 
        {
            try 
            {
                using(StreamReader reader = File.OpenText(pathFlie)) 
                {
                    string data = reader.ReadToEnd();
                    playerData = JsonUtility.FromJson<PlayerData>(data);
                    reader.Close();
                }
            } 
            catch(Exception e) 
            {
                Debug.Log($"[DATA] Read file no found.\n <path>: {pathFlie}\n <error>: {e}");
            }
        } 
        else 
        {
            Debug.Log($"[DATA] Read file no found.\n <path>: {pathFlie}");
            playerData = new PlayerData();
        }
        //SaveData();
    }

    private void OnApplicationPause(bool pause) 
    {
        if(pause) 
        {
            SaveData();
        }
    }

    private void OnApplicationQuit() 
    {
        SaveData();
    }
    #endregion

    #region ItemData
    private void LoadItemData() {
        foreach(ItemData item in Resources.LoadAll<ItemData>(ITEMS_PATH)) {
            lstItem.Add(item);
        }
    }
    public ItemData GetItemDataByID(ItemID itemID) {
        ItemData result = lstItem.Find(x => x.ItemID == itemID);
        return result;
    }
    public List<ItemData> GetItemDataByRank(int startID, int endID) {
        List<ItemData> lstResult = new List<ItemData>();
        if(startID < 0 || endID >= lstItem.Count || startID > endID) {
            return lstResult;
        }
        foreach(var item in lstResult) {
            if((int)item.ItemID >= startID && (int)item.ItemID <= endID) {
                lstResult.Add(item);
            }
        }
        return lstResult;
    }

    private void LoadLevelMap() {
        foreach(LevelMap map in Resources.LoadAll<LevelMap>(LEVELMAPS_PATH)) 
        {
            lstMap.Add(map);
        }
    }

    public LevelMap GetlevelMapByLevel(int level) {
        return lstMap.Find(x=> x.Level == level);
    }

    private void LoadWeaponData() {
        foreach(WeaponData weapon in Resources.LoadAll<WeaponData>(WEAPONS_PATH)) {
            lstWeapon.Add(weapon);
        }
    }

    public WeaponData GetWeaponByID(WeaponID weaponID) {
        WeaponData result = lstWeapon.Find(x => x.ID == weaponID);
        return result;
    }
    #endregion
}
