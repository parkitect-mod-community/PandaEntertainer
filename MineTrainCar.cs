using System;

public class MineTrainCar : Car
{
    public void Decorate(bool isFront)
    {
        backAxis = transform.Find ("backAxis");
        if (isFront)
            frontAxis = transform.Find ("frontAxis");
    }

}
