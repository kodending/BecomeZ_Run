using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class ItemLoad : MonoBehaviour
{
    [SerializeField]
    private int iHealAmount;

    void Start()
    {
        ItemData itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.SetHealAmount(iHealAmount);

        itemData.SetScriptablePath("Assets/Resources/ScriptableData/Item/" +
                                "HPPotion" + ".asset");

        AssetDatabase.CreateAsset(itemData, itemData.strScriptablePath);
    }
}
