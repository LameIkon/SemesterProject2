using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider2D))]
public class AsyncSceneLoader : MonoBehaviour
{


    private BoxCollider2D _sceneTrigger;

    //[SerializeField] private SceneField[] _scenes;
    [SerializeField] private SceneField[] _scenes;

    private void Awake()
    {
        _sceneTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_scenes.Length == 0) return;

        if (collision.CompareTag("Player"))
        {
            foreach (var scene in _scenes)
            {
                if (UnloadScene(scene)) 
                {
                    return;
                }
            }
        }

        if (_scenes.Length == 0) return;

        if (collision.CompareTag("Player"))
        {
            foreach (var scene in _scenes)
            {
                LoadScene(scene);
            }
        }

    }

    private bool LoadScene(SceneField scene) 
    {
        if (!SceneManager.GetSceneByName(scene).isLoaded)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool UnloadScene(SceneField scene) 
    {
        if (SceneManager.GetSceneByName(scene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
            return true;
        }
        else
        {
            return false;
        }
    }

}
