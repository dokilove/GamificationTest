using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BreakableItem
{
    private VisualElement item;
    public VisualElement Item { get
        {
            return item;
        } 
    }

    private int hp = 3;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp == 2)
            {
                item.style.backgroundColor = new StyleColor(new Color(1.0f, 0.3366022f, 0.0f));
            }else if (hp == 1)
            {
                item.style.backgroundColor = new StyleColor(new Color(1.0f, 0.0f, 0.0f));
            }
            else if (hp ==0)
            {
                item.style.visibility = Visibility.Hidden;
            }
        }
    }
    
    public BreakableItem(VisualElement item)
    {
        this.item = item;
    }
}
