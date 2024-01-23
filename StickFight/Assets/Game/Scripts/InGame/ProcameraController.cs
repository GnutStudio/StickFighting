using Com.LuisPedroFonseca.ProCamera2D;
using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcameraController : Singleton<ProcameraController>
{
    public const string SETTING_ROOM = "SettingRoom";
    [SerializeField] private ProCamera2D proCamera;
    [SerializeField] private ProCamera2DRooms camRoom;
    [SerializeField] private ProCamera2DShake camShake;
    [SerializeField] private float minSize,maxSize;
    public ProCamera2D ProCamera => proCamera;
    private float valueSetting;
    public float ValueSetting => valueSetting;
    protected override void OnAwake() {
        base.OnAwake();
        LoadData();
    }

    private void Start() {
        SetUpDefault();
    }

    public void SetOrthographic(float size) {
        proCamera.GameCamera.orthographicSize = size;
    }

    public void SetValueSetting(float value) {
        valueSetting = Mathf.Clamp(value, 0, 1);
        proCamera.GameCamera.orthographicSize = GetSizCameraByPercent(valueSetting);
    }

    public void AddRoom(Rect rect) {
        Room room = new Room();
        room.Dimensions = rect;
        camRoom.AddRoom(rect.x,rect.y,rect.width,rect.height);
        camRoom.enabled = true;
    }

    public float GetSizCameraByPercent(float percent) {
        float minMax = maxSize- minSize;
        return minSize + minMax * percent;
    }

    public void SheckCamera() {
        camShake.Shake(0);
    }

    public void SetUpDefault() {
        camRoom.enabled = false;
        camRoom.Rooms.Clear();
        SetValueSetting(valueSetting);
    }

    public void SetTarget(params Transform[] targets) {
        proCamera.CameraTargets.Clear();

        foreach(var target in targets) {
            CameraTarget cameraTarget = new CameraTarget() { TargetTransform = target };
            proCamera.CameraTargets.Add(cameraTarget);
        }
    }

    #region Save And Load
    private void SaveData() {
        PlayerPrefs.SetFloat(SETTING_ROOM, valueSetting);
    }

    private void LoadData() {
        if(PlayerPrefs.HasKey(SETTING_ROOM)) {
            valueSetting = PlayerPrefs.GetFloat(SETTING_ROOM);
        } else {
            valueSetting = minSize;
            SaveData();
        }
    }
    #endregion
    #region EventGame
    private void OnDisable() {
        SaveData();
    }
    private void OnApplicationQuit() {
        SaveData();
    }

    private void OnApplicationFocus(bool focus) {
        if(!focus) {
            SaveData();
        }
    }

    private void OnApplicationPause(bool pause) {
        if(pause) {
            SaveData();
        }
    }
    #endregion
}
