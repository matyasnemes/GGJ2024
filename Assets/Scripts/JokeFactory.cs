using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeFactory : MonoBehaviour
{
	void Start()
	{
		HashSet<string> uniqueTypeNames = new HashSet<string>();
		foreach (var jit in jokeItemTypes)
		{
			uniqueTypeNames.Add(jit.name());
		}
		Debug.Assert(jokeItemTypes.Count == uniqueTypeNames.Count);
	}

	JokeItem createJokeItem()
	{
		int chosenUnopened = generateInterval(0, unopenedSprites.Count);
		int chosenType = generateInterval(0, jokeItemTypes.Count);
		return new JokeItem(unopenedSprites[chosenUnopened],
				            jokeItemTypes[chosenType]);
	}

	// min inclusive, max is exclusive.
	private int generateInterval(int min, int max)
	{
		return (int)(Random.value * (max -  min - 1) + min);
	}

	public List<Sprite> unopenedSprites;
	public List<JokeItemType> jokeItemTypes;
}

