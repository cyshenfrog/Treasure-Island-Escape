using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MController_8 : MController_2 {

    //攻城獅王

    //path
    public int CornerCount;
    public int SideLength = 10;
    public Vector2 Center = new Vector2(0, 0);

    private List<Vector2> corner = new List<Vector2>();
    private int targetCornor = 0;

    protected override void Awake() {
        base.Awake();

        for (int i = 0; i < CornerCount; i++) {
            int x = Random.Range(-5, 5);
            int y = Random.Range(-5, 5);
            corner.Add(new Vector2(x, y));
        }
    }

    public override void Idle() {
        Move();
    }

    public override void Move() {

        Vector2 pos = transform.position;

        for (int i = 0; i < corner.Count; i++) {
            if (pos == corner[i]) {
                targetCornor = ++targetCornor % corner.Count;
                break;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, corner[targetCornor], Time.deltaTime * Speed);
    }

}
