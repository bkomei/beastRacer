using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTracker : MonoBehaviour {


    private int yesCount = 0;
    private int noCount = 0;

    void DialogueSignal(string msg) {
        switch (msg) {
            case "yes":
                yesCount += 1;
                break;
            case "no":
                noCount += 1;
                break;
        }
        print(string.Format("Yes count: {0}   No count: {1}", yesCount, noCount));
    }
}
