using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider2D))]
public class AsyncSceneLoader : MonoBehaviour
{


    [SerializeField] private BoxCollider2D _sceneTrigger;
    [SerializeField] private bool _loadScenes;

    [SerializeField] private SceneField[] _scenes;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_loadScenes)
        {
            if (_scenes.Length == 0) return;

            if (collision.CompareTag("Player"))
            {
                foreach (var scene in _scenes)
                {
                    LoadScene(scene);
                }
            }
        }
        else
        {
            if (_scenes.Length == 0) return;

            if (collision.CompareTag("Player"))
            {
                foreach (var scene in _scenes)
                {
                    UnloadScene(scene);
                }
            }
        }
    }

    private void LoadScene(SceneField scene) 
    {
        if (SceneManager.GetSceneByName(scene).isLoaded)
        {
            return;
        }
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    private void UnloadScene(SceneField scene) 
    {
        if (SceneManager.GetSceneByName(scene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
        }
    }

}
