using System;

public enum FaceDirection { Right, Left }

public class FaceDirectionChangedEventArgs : EventArgs
{
    protected FaceDirection direction;

    public FaceDirectionChangedEventArgs(FaceDirection direction)
    {
        this.direction = direction;
    }

    public FaceDirection FaceDirection
    {
        get {
            return direction;
        }
    }
}
