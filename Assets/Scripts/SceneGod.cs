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
	public static SceneGod SInstance { get; private set; }
	private enum GameState { Death, Game, MainMenu }

	private GameState _currentState;
	
	//player variables

	public int PlayerScore {get; private set; }
	
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

	/// <summary>
	/// Increases the player's score by the specified value.
	/// </summary>
	/// <param name="score">The amount to increment the player's score by.</param>
	public void IncrementScore(int score)
	{
		PlayerScore += score;
	}

	/// <summary>
	/// Resets the player's score to zero.
	/// </summary>
	public void ResetScore()
	{
		PlayerScore = 0;
	}
	
	
	public void EnterGameState()
	{
		if (_currentState != GameState.Game)
		{
			_currentState = GameState.Game;
			SceneManager.LoadScene(gameScene);
		}
		else
		{
			Debug.LogWarning("Already in Game Scene!");
		}
	}
	public void EnterDeathState()
	{
		if (_currentState != GameState.Death)
		{
			_currentState = GameState.Death;
			SceneManager.LoadScene(deathScene);
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
		}
		else
		{
			Debug.LogWarning("Already in Main Menu Scene!");
		}
	}
}