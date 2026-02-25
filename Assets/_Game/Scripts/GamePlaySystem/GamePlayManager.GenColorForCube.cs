using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public partial class GamePlayManager
{

    private string _nextColor;
    private bool _hasCube;
    public bool HasCube => _hasCube;

    private int _currentIndexColor;

    public void GenColorForDefaultCube(int indexCube, bool isFirstGen = false)
    {
        if (indexCube == -1) return;
        if (_currentIndexColor >= _currentLevelConfigData.BoxsQueue.Count)
        {
            _currentIndexColor++;
            _hasCube = false;
            _nextColor = ShaderPropertiesLib.IgnoredWoolColorKey;
            return;
        }
        _cubeTargetPrio.Remove(indexCube);
        _cubeTargetPrio.Add(indexCube);
        _nextColor = _currentLevelConfigData.BoxsQueue[_currentIndexColor];
        _colorTargets[indexCube] = _nextColor;
        CurrentCubeTargets[indexCube].SetColor(_nextColor);
        _hasCube = true;
        _currentIndexColor++;
    }


    #region HELPER

    private void LockOpenCube()
    {
        foreach (var cube in CurrentCubeTargets)
        {
            if (cube.IsActive) continue;
            cube.ActiveOpenCube(false);
        }
    }


    #endregion
}
