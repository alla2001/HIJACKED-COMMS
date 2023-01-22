using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonMonoBehaviour<CharacterManager>
{
    public List<Character> allCharacters;
	public int maxActionPoints;
	public bool HasAllCharactersFinishedActions()
	{
		
		foreach (Character _chara in allCharacters)
		{
			if (!_chara.finishedActions)
			{
				return false;
			}
		}

		return true;
	}
	public void OnCharacterFinishedActions()
    {
		foreach (Character _chara in allCharacters)
		{
			if (!_chara.finishedActions)
			{
				return;
			}
		}
		//GameManager.instance.ch

	}
	public void RestForAction()
    {
		foreach (Character _chara in allCharacters)
		{
			_chara.finishedActions= false;
		}
	}
	public Character GetCharacterOnCell(Vector2Int cell)
    {
        foreach (Character chara in allCharacters)
        {
            if (chara.posOnGrid==cell)
            {
				return chara;

            }
        }
		return null;
    }
}
