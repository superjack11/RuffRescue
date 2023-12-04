using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogClass : MonoBehaviour
{


    public class dog
    {
        public string spriteName;
        public string dogName;
        public string dogDescription;

        public int size;
        public int energy;
        public int dogSociability;
        public int nonDogSociability;
        public int vocality;
        public int love;

        private string endString = "}.<$";
        private string seperatorString = "@|&%";

        public dog(string spriteName, string dogName, string dogDescription, int size, int energy, int dogSociability, int nonDogSociability, int vocality, int love)
        {
            this.spriteName = spriteName;
            this.dogName = dogName;
            this.dogDescription = dogDescription;

            this.size = size;
            this.energy = energy;
            this.dogSociability = dogSociability;
            this.nonDogSociability = nonDogSociability;
            this.vocality = vocality;
            this.love = love;
        }

        public string generateCode()
        {
            return "" + this.spriteName
                + seperatorString + this.dogName
                + seperatorString + this.dogDescription
                + seperatorString + this.size
                + seperatorString + this.energy
                + seperatorString + this.dogSociability
                + seperatorString + this.nonDogSociability
                + seperatorString + this.vocality
                + seperatorString + this.love
                + seperatorString
                + endString;
        }
    }

    public class item {
        private string endString = "}.<$";
        private string seperatorString = "@|&%";

        public string spriteName;   // The name of the sprite in the files so that it can be loaded from Resources
        public string itemName; // The name of the item to be displayed in game
        public string itemType; // The type of item eg. floor, wall, decoration
        public int value; // The monetary value of the item

        public item(string spriteName, string itemName, string itemType, int value) {
            this.spriteName = spriteName;
            this.itemName = itemName;
            this.itemType = itemType;
            this.value = value;
        }

        public string generateCode() {
            return "" + this.spriteName
                + seperatorString + this.itemName
                + seperatorString + this.itemType
                + seperatorString + this.value
                + seperatorString
                + endString;
        }

    }

    public List<item> allItems = new List<item> {
        new item("wallTile","Dingy Tiles","wall", 20)

    };
}
