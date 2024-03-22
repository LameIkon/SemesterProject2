using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider2D))]
public class AsyncSceneLoader : MonoBehaviour
{


    private BoxCollider2D _sceneTrigger;

    [SerializeField] private SceneField[] _loadScenes;
    [SerializeField] private SceneField[] _unloadScenes;

    bool _loaded = false;


    private void Awake()
    {
        _sceneTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player") && _unloadScenes.Length != 0)
        {
            StartCoroutine(UnloadScene(_unloadScenes));
            
        }

        if (collision.CompareTag("Player") && _loadScenes.Length != 0)
        {
            StartCoroutine(LoadScene(_loadScenes));
        }

    }

    private IEnumerator LoadScene(SceneField[] scenes) 
    {
        foreach (var scene in scenes)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator UnloadScene(SceneField[] scenes) 
    {
        foreach (var scene in scenes)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
            }
            yield return new WaitForFixedUpdate();
            
        }
    }

}
