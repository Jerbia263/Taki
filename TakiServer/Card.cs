using System;
using System.Collections.Generic;
using System.Text;

public class Card
{
    private string color;
    private cardValue value;
    private string pictureFile;

    public enum cardValue
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        ChangeColor,
        ChangeDirection,
        CrazyCard,
        Plus,
        Stop,
        Taki,
        Deck
    }

    public Card(string color, cardValue value)
    {
        this.color = color;
        this.value = value;
        SetPictureFile();
    }

    public Card(string value, string color)
    {
        this.color = color;

        // parse value
        if (value == "One")
            this.value = cardValue.One;
        if (value == "Two")
            this.value = cardValue.Two;
        if (value == "Three")
            this.value = cardValue.Three;
        if (value == "Four")
            this.value = cardValue.Four;
        if (value == "Five")
            this.value = cardValue.Five;
        if (value == "Six")
            this.value = cardValue.Six;
        if (value == "Seven")
            this.value = cardValue.Seven;
        if (value == "Eight")
            this.value = cardValue.Eight;
        if (value == "Nine")
            this.value = cardValue.Nine;
        if (value == "Plus")
            this.value = cardValue.Plus;
        if (value == "ChangeDirection")
            this.value = cardValue.ChangeDirection;
        if (value == "CrazyCard")
            this.value = cardValue.CrazyCard;
        if (value == "Stop")
            this.value = cardValue.Stop;
        if (value == "ChangeColor")
            this.value = cardValue.ChangeColor;
        if (value == "Taki")
            this.value = cardValue.Taki;

        SetPictureFile();
    }

    public string GetColor()
    {
        return color;
    }

    public cardValue GetValue()
    {
        return value;
    }
    public bool isEquals(Card c)
    {
        if (c.color == this.color && c.value == this.value)
            return true;
        return false;
    }

    public string GetPictureFile()
    {
        return pictureFile;
    }

    private void SetPictureFile()
    {
        if (value <= cardValue.Nine)
        {
            pictureFile = @".\imagesOfCards\" + value.ToString() + "_" + color + ".png";
        }
        else
        {
            // change color
            if (value == cardValue.ChangeColor)
            {
                pictureFile = @".\imagesOfCards\" + "changeColor" + "_" + color + ".png";
            }
            // change direction
            if (value == cardValue.ChangeDirection)
            {
                pictureFile = @".\imagesOfCards\" + "changeDirection" + "_" + color + ".png";
            }
            // crazy card
            if (value == cardValue.CrazyCard)
            {
                pictureFile = @".\imagesOfCards\" + "crazyCard.png";
            }
            // plus
            if (value == cardValue.Plus)
            {
                pictureFile = @".\imagesOfCards\" + "plus" + "_" + color + ".png";
            }
            // stop
            if (value == cardValue.Stop)
            {
                pictureFile = @".\imagesOfCards\" + "stop" + "_" + color + ".png";
            }
            // taki
            if (value == cardValue.Taki)
            {
                pictureFile = @".\imagesOfCards\" + "taki" + "_" + color + ".png";
            }
            // deck
            if (value == cardValue.Deck)
            {
                pictureFile = @".\imagesOfCards\deckOfCards.png";
            }
        }

    }
}

