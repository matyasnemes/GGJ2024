using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeFactory : MonoBehaviour
{
	public void Start()
	{
		HashSet<string> uniqueTypeNames = new HashSet<string>();
		foreach (var jit in jokeItemTypes)
		{
			uniqueTypeNames.Add(jit.name());
		}
		Debug.Assert(jokeItemTypes.Count == uniqueTypeNames.Count);
		Debug.Assert(jokeItemTypes.Count > 0);
		Debug.Assert(unopenedSprites.Count > 0);
		Debug.Assert(openedSprites.Count > 0);
	}

	public JokeItem createRandomJokeItem()
	{
		int chosenUnopened = generateInterval(0, unopenedSprites.Count);
		int chosenOpened = generateInterval(0, openedSprites.Count);
		int chosenType = generateInterval(0, jokeItemTypes.Count);
		JokeItemType jit = jokeItemTypes[chosenType];
		int numJokeTexts = jit.jokeTexts().Count;
		int numJokeClips = jit.jokeClips().Count;
		int chosenText = generateInterval(0, numJokeTexts);
		return new JokeItem(jit,
				            jit.jokeTexts()[chosenText],
							chosenText < numJokeClips ? jit.jokeClips()[chosenText] : null,
							unopenedSprites[chosenUnopened],
							openedSprites[chosenOpened],
							revealDuration);
	}

	public List<string> getJokeTypeNames()
	{
		List<string> result = new List<string>();
		foreach (var jit in jokeItemTypes)
		{
			result.Add(jit.name());
		}
		return result;
	}

	// min inclusive, max is exclusive.
	private int generateInterval(int min, int max)
	{
		// 3rd time is the charm
		return Random.Range(min, max);
	}

	public bool CanSpawnNewJokePaper()
	{
		return numActiveJokePapers < MaximumNumberOfActiveJokePapers;
	}

	public void OnJokePaperSpawned()
	{
		numActiveJokePapers += 1;
	}

	public void OnJokePaperCollected()
	{
		numActiveJokePapers -= 1;
	}

	public List<Sprite> unopenedSprites;
	public List<Sprite> openedSprites;
	public List<JokeItemType> jokeItemTypes;
	public float revealDuration = 5.0F; // In seconds.
	
	/**
	 * The maximum allowed number of active joke papers.
	 */
	public int MaximumNumberOfActiveJokePapers = 10;

	/**
	 * The current number of joke papers lying around on the floor.
	 */
	private int numActiveJokePapers;
}

