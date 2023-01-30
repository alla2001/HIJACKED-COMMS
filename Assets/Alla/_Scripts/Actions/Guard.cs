using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : GameAction
{
    List<Character> enemiesInRange= new List<Character>();
    Character enemy;
    int ammo=1;
    GridHilight gh;
    Vector2Int targetCell;
    public override void Update(Character owner)
    {

        if (ammo <= 0) return;

        List<Vector2Int> cells = GridManager.instance.GetCircle(owner.posOnGrid, owner.stats.Range);
        foreach (Vector2Int cell in cells)
        {

            Character character = CharacterManager.instance.GetCharacterOnCell(cell);
            if (character != null && character!=owner)
            {

                enemiesInRange.Add(character);
            }
        }
        if (enemiesInRange.Count > 0) { enemy = enemiesInRange[0]; } 
        else
        {
            return;
        }

        Shoot shoot = new Shoot();
        shoot.targetCell = enemy.posOnGrid;
        shoot.Start(owner, 0);
        ammo--;
    }
    public override bool CanAssigne(Character playerCharacter, Vector2Int selectedCell)
    {
        if (selectedCell != playerCharacter.ghost.posOnGrid) return false;
        return true;
    }

    public override void Do(Character owner, int Tick)
    {
        
       
    }

    public override void OnAddServer(Character owner)
    {
        UnHilight();

        //if (gh != null) GridShaderBinder.gridHilights.Remove(gh);
        //gh = new GridHilight();
        //gh.points.Add(owner.ghost.posOnGrid);
        //gh.color = Color.magenta;
        //GridShaderBinder.gridHilights.Add(gh);
    }
    public override void OnAddClient(Character owner)
    {
        base.OnAddClient(owner);
        UnHilight();
    }
    public override bool IsFinished(Character owner)
    {
       
        return true;
    }

    public override bool Removed(Character owner)
    {
        UnHilight();
        return true;
    }
    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
    {
        base.Hilight(playerCharacter, selectedCell);
        List<Vector2Int> cells = GridManager.instance.GetCircle(playerCharacter.ghost.posOnGrid, playerCharacter.stats.Range);
        if(gh!=null) GridShaderBinder.gridHilights.Remove(gh);
        gh = new GridHilight();
        gh.points .AddRange(cells);
        gh.color = new Color(1,0.5f,0.5f,0.3f);
        
        GridShaderBinder.gridHilights.Add(gh);

    }

    public override void UnHilight()
    {
        if (gh != null) GridShaderBinder.gridHilights.Remove(gh);
        base.UnHilight();
    }
}
