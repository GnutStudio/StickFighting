using DG.Tweening;
using TMPro;
using UnityEngine;

public static class Extentions {
    public static void CheckKillTween(this Tween tween, bool OnComplate = false) {
        if(tween != null) {
            tween.Kill(OnComplate);
        }
    }

    public static Color GetColorByInt(this int index) { //0.green, 1.red, 2.yellow, 3.blue, Defaul.While
        if(index == 0) {
            return Color.green;
        } else if(index == 1) {
            return Color.red;
        } else if(index == 2) {
            return Color.yellow;
        } else if(index == 3) {
            return Color.blue;
        } else {
            return Color.white;
        }
    }

    public static ItemData GetDataByID(this ItemID id) {
        return DataManager.Instance.GetItemDataByID(id);
    }

    public static ItemStack GetSaveByID(this ItemID id) {
        return DataManager.Instance.PlayerData.GetItemSaveByItemId(id);
    }

    public static string GetHoursBySeconds(this int secs) {
        int hours = secs / 3600;
        int mins = (secs % 3600) / 60;
        secs = secs % 60;
        return string.Format("{0:D2} : {1:D2} : {2:D2}", hours, mins, secs);
    }

    public static bool CheckSetString(this TextMeshProUGUI text, string content) {
        if(text == null || string.IsNullOrEmpty(content)) {
            Debug.Log("Fall: text or content == null");
            return false;
        }
        text.text = content;
        return true;
    }

    public static WeaponData GetDataWeaponByID(this WeaponID ID) {
        return DataManager.Instance.GetWeaponByID(ID);
    }


    public static T[] Append<T>(this T[] array, T item) {
        if(array == null) {
            return new T[] { item };
        }

        T[] result = new T[array.Length + 1];
        for(int i = 0; i < array.Length; i++) {
            result[i] = array[i];
        }

        result[array.Length] = item;
        return result;
    }

    #region rect
    public static void SetLeft(this RectTransform rt, float left) {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right) {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top) {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom) {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
    #endregion
}
