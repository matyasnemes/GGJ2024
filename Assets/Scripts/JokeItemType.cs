using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "JokeItemType", menuName = "ScriptableObjects/JokeItemType", order = 1)]
public class JokeItemType : ScriptableObject
{
	new public string name()
	{
		return Name;
	}

	public string description()
	{
		return Description;
	}

	public List<string> jokeTexts()
	{
		return JokeTexts;
	}

	public Sprite sprite()
	{
		return Sprite;
	}

	public string Name;
	public string Description;
	public List<string> JokeTexts;
	public Sprite Sprite;
	// Other meta info
}

