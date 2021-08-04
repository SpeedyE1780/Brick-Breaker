using UnityEditor;

public class Spawn : EditorWindow
{
    [MenuItem("CONTEXT/SpawnBricks/Spawn Bricks")]
    static void SpawnBricks(MenuCommand command)
    {
        SpawnBricks body = (SpawnBricks)command.context;
        body.PlaceBricks(false);
    }
}