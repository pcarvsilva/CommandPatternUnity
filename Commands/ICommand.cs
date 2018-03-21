using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void OnClick(GameObject agent,RaycastHit hit);
}
