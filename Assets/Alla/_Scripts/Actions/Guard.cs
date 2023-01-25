using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : GameAction
{
    List<Character> enemiesInRange;
    Character enemy;
    int ammo=3;
    public override void Update(Character owner)
    {
        
    }
    public override bool CanAssigne(Character playerCharacter, Vector2Int selectedCell)
    {
        return true;
    }

    public override void Do(Character owner, int Tick)
    {
        List<Vector2Int> cells= GridManager.instance.GetCircle(owner.posOnGrid, owner.stats.Range);
        foreach (Vector2Int cell in cells)
        {
            Character character = CharacterManager.instance.GetCharacterOnCell(cell);
            if (character != null)
            {
                enemiesInRange.Add(character);
            }
        }
        if (enemiesInRange.Count > 0) enemy = enemiesInRange[0];
    }

    public override void initilize(Character owner)
    {
        
    }

    public override bool IsFinished(Character owner)
    {
        return true;
    }

    public override bool Removed(Character owner)
    {
        return true;
    }
    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
    {
        base.Hilight(playerCharacter, selectedCell);

    }

    public override void UnHilight()
    {
        base.UnHilight();
    }
}
