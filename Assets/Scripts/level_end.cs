using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level_end : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(message: "level end");
        PlayerMover player = other.GetComponent<PlayerMover>();
        SceneManager.LoadScene("Level_2");
    }
   
}
