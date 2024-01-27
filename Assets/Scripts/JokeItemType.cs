using System;
using UnityEngine;

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

	public Sprite sprite()
	{
		return Sprite;
	}

	public string Name;
	public string Description;
	public Sprite Sprite;
	// Other meta info
}

