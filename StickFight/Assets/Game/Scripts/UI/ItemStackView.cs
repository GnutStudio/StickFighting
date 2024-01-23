using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemStackView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txt_Amount;
    [SerializeField] private string txtBefor;
    private ItemStack itemStack;
    public ItemStack Model => itemStack;
    public void Show(ItemStack itemStack) {
        this.itemStack = itemStack;
        icon.sprite = itemStack.Data.Icon;
        Extentions.CheckSetString(txt_Amount, txtBefor + itemStack.Amount.ToString());
    }
}
