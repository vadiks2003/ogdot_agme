using Godot;
using System;

public partial class KeyManager
{
    private static int KeyAmount = 0;
    static int MAX_KEYS_IN_LIST = 32;
    struct Key{
        public bool exists;
    }
    Key[] keylist;
    public KeyManager(){
        keylist = new Key[MAX_KEYS_IN_LIST];
    }
    
    public void CreateKey(int id)
    {
        keylist[KeyAmount].exists = true;
        KeyAmount += 1;
    }
    public bool DoesHaveKey(int id)
    {
        if(id >= MAX_KEYS_IN_LIST-1)
        {
            GD.Print("illegal key id accessed in DoesHaveKey in KeyManager");
            return false;
        }
        return keylist[id].exists;
    }

    public bool UseKey(int id)
    {
        if(!DoesHaveKey(id)){
            return false;
        }
        return true;
    }
}
