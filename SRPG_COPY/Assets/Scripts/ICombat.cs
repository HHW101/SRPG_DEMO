using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICombat 
{
    // Start is called before the first frame update
    void Attack();
    void Damaged(float x);
}
