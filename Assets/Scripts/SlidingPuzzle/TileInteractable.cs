
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TileInteractable : XRGrabInteractable
{
    private SlidingPuzzle puzzleManager;
    
    protected override void Awake()
    {
        base.Awake();
        puzzleManager = FindObjectOfType<SlidingPuzzle>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        puzzleManager.TryMoveTile(this, puzzleManager.GetTilePosition(gameObject));
    }
}
