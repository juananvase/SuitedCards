using System.Collections;
using UnityEngine;
public interface IParryUser
{
    bool IsParrying { get; set; }
    void StartParryWindow(float parryWindow);

}
