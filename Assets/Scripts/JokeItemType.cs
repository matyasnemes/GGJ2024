using System;
using UnityEngine;

[Serializable]
public class JokeItemType
{
	public string name()
	{
		return _name;
	}

	public string description()
	{
		return _description;
	}

	public Sprite sprite()
	{
		return _sprite;
	}

	private string _name;
	private string _description;
	private Sprite _sprite;
	// Other meta info
}

