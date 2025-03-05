using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New File", menuName = "Data/BuildingData")]

[System.Serializable]
    public class RuntimeObjectReference 
    {
        public string objectName;
        private GameObject cachedObject;
        
        public GameObject GetObject() 
        {
            if (cachedObject == null)
                cachedObject = GameObject.Find(objectName);
            return cachedObject;
        }

        public void SetActive(bool isActive)
        {
            GameObject obj = GetObject();
            if (obj != null)
                obj.SetActive(isActive);
        }
    }
    

public class BuildingData : ScriptableObject
{
    public string buildingName;
    public Sprite iconSprite;
    public GameObject buildingPrefab_test;
    public RuntimeObjectReference buildingPanelUI;
    public int id;
}
