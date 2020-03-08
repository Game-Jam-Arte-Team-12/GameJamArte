using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCardManager : MonoBehaviour
{
    public static TitleCardManager Instance;

    [SerializeField]
    private GameObject _titleCard;
    [SerializeField]
    private GameObject _intercalary;
    [SerializeField]
    public CustomCamera _cam;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _intercalary.SetActive(false);
    }


    public void StartGame()
    {
        _titleCard.SetActive(false);
        GameManager.Instance.Play();
        // TODO: slowly move camera to character
        // TODO: hide everything
        // TODO: Boom sfx
        // TODO: heavy breathing sfx
        // TODO: show intercalary
        // TODO: start game
    }

    public void StartIntercalary()
    {
    }

    public void QuitGame()
    {
        Debug.Log("coucou");
        Application.Quit();
    }



}
