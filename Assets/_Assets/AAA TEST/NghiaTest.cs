using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NghiaTest : MonoBehaviour
{

    void Start()
    {
        Calculate();
    }

    private void Calculate()
    {
        int type_1 = 0, type_2 = 0, type_3 = 0, type_4 = 0, type_5 = 0;

        for (int i = 0; i < 100000; i++)
        {
            int random = UnityEngine.Random.Range(1, 1001);

            if (random <= 500)
            {
                type_1++;
            }
            else if (random <= 700)
            {
                type_2++;
            }
            else if (random <= 850)
            {
                type_3++;
            }
            else if (random <= 950)
            {
                type_4++;
            }
            else
            {
                type_5++;
            }
        }

        DebugCustom.Log(string.Format("type1={0},type2={1},type3={2},type4={3},type5={4}", type_1, type_2, type_3, type_4, type_5));
    }
}
