//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_DisableBeforePlay : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_DisableBeforePlay instance;
    public static HR_DisableBeforePlay Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<HR_DisableBeforePlay>();

            return instance;

        }

    }
    #endregion

    public GameObject[] targetGameobjectsToDisable;

}
