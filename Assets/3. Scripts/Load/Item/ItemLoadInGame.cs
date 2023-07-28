using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoadInGame : MonoBehaviour
{
    [SerializeField]
    private int iHealAmount;

    void Start()
    {
        ItemData itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.SetHealAmount(iHealAmount);

        itemData.SetScriptablePath("Assets/Resources/ScriptableData/Item/" +
                                "HPPotion" + ".asset");
    }
}
