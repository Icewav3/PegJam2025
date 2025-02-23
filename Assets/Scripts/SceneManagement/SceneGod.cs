using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGod : MonoBehaviour
{
	[SerializeField]
	private string deathScene;
	[SerializeField]
	private string gameScene;	
	[SerializeField]
	private string mainMenuScene;

	[SerializeField] private string introScene;
	public static SceneGod SInstance { get; private set; }
	public enum GameState { Death, Game, MainMenu, Quit, Intro }
	// services
	[SerializeField] //this is now canvas
	public GameObject dialogue;

	[SerializeField] public DialogueManager DialogueManager;
	//state
	private GameState _currentState;
	
	//player variables
	public GameObject player;
	public int PlayerScore {get; private set; }
	public int PlayerDeaths {get; private set; }
	
	/// <summary>
	/// Initializes the SceneGod instance and ensures it persists across scenes.
	/// </summary>
	/// <remarks>
	/// This method is responsible for setting up the singleton instance of the SceneGod
	/// and ensuring it is not destroyed when loading new scenes. It also destroys duplicate
	/// instances that already exist in other scenes.
	/// </remarks>
	private void Awake()
	{
		// kill itself if there is another
		if (SInstance != null && SInstance != this)
		{
			if (SInstance.gameObject.scene.buildIndex != gameObject.scene.buildIndex)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			SInstance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		DialogueManager = this.GetComponentInChildren<DialogueManager>();
	}

	/// <summary>
	/// Increases the player's score by the specified value.
	/// </summary>
	/// <param name="score">The amount to increment the player's score by.</param>
	public void IncrementScore(int score)
	{
		PlayerScore += score;
	}
	/// <summary>
	/// Increases death count by 1
	/// </summary>
	public void IncrementDeaths()
	{
		PlayerDeaths += 1;
		ResetPlayer();
		EnterDeathState();
	}

	/// <summary>
	/// Resets the player's score to zero. & resets deathcounter
	/// </summary>
	private void ResetPlayer()
	{
		PlayerScore = 0;
	}
	
	
	public void EnterGameState()
	{
		if (_currentState != GameState.Game)
		{
			_currentState = GameState.Game;
			SceneManager.LoadScene(gameScene);
			dialogue.SetActive(true);
		}
		else
		{
			Debug.LogWarning("Already in Game Scene!");
		}
		//yes its scuffed
		DialogueManager.StartDialogueSequence("movement_tutorial");
	}
	private void EnterDeathState()
	{
		if (_currentState != GameState.Death)
		{
			_currentState = GameState.Death;
			SceneManager.LoadScene(deathScene);
			dialogue.SetActive(false);
		}
		else
		{
			Debug.LogWarning("Already in Death Scene!");
		}
	}
	public void EnterMainMenuState()
	{
		if (_currentState != GameState.MainMenu)
		{
			_currentState = GameState.MainMenu;
			SceneManager.LoadScene(mainMenuScene);
			dialogue.SetActive(false);
		}
		else
		{
			Debug.LogWarning("Already in Main Menu Scene!");
		}
	}
	
	public void EnterIntroState()
	{
		if (_currentState != GameState.Intro)
		{
			_currentState = GameState.Intro;
			SceneManager.LoadScene(introScene);
			dialogue.SetActive(true);
		}
		else
		{
			Debug.LogWarning("Already in Intro Scene!");
		}
		print("runs");
		DialogueManager.StartDialogueSequence("intro");
	}

	public void EnterQuitState()
	{
		Application.Quit();
	}
}