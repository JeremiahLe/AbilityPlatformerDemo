using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityScript : MonoBehaviour
{
    public Image SkillTreeBG;

    // Start is called before the first frame update
    void Awake()
    {
        SkillTreeBG.canvasRenderer.SetAlpha(0.5f);
    }
}
