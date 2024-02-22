using Godot;
using System;
using System.Collections.Generic;

public partial class AI_movement
{
    int currPath = 0;
    // for outside usage (in boss.cs)
    public bool Stop = false;
    struct Path3D{
        public int ID;
        public Vector2 from;
        public Vector2 to;
    }
    List<Path3D> directions = new List<Path3D>();
    public void MoveTowards(Vector2 currentPos, Vector2 toPos){
        Path3D path;
        path.to = toPos;
        path.from = currentPos;
        path.ID = directions.Count;
        directions.Add(path);
    }
    public Vector2 GetCurrentGoal(){
        return directions[currPath].to;
    }
    public void MoveTowardsCurved(Vector2 currentPos, Vector2 toPos){
        // implement some shit ass technique of curving later idfk

    }
    public void ResetPaths(){
        directions = new List<Path3D>();
    }

    public void CheckIfReached(Vector2 curpos){
        if((directions[currPath].to - curpos).Length() <= 10){
            
            if(currPath + 1 >= directions.Count)
            {
                if(!Stop) Stop = true;
                return;
            } 
            currPath = currPath + 1;
            return;
            // i dont fucking know why i booled it
        }
    }
}
