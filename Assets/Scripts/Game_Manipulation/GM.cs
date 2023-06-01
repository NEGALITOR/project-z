using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<----------------Director of the game--------------->
public class GM : MonoBehaviour
{
    public bool Player_Control;
    public bool cutscene;
    public bool Dialogue_only;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player_GamePlay.Gamplay_mode = Player_Control;
        //Dialogue.Dialogue_mode = Dialogue_only;
    }
}
