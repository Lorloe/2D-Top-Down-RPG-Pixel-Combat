using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        // if (SceneManagement.Instance.SceneTransitionName != null)
        {
            if (transitionName == SceneManagement.Instance.SceneTransitionName)
            {
                PlayerController.Instance.transform.position = this.transform.position;
                // Debug.Log(SceneManagement.Instance.SceneTransitionName);
                // Debug.Log(PlayerController.Instance.transform.position);
                CameraController.Instance.SetPlayerCameraFollow();

                UIFade.Instance.FadeToClear();
            }

        }
    }
}
