using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MController_8 : MController_2 {

    //攻城獅王

    //path
    public int VertexCount;
    public int SideRange = 10;
    public Vector2 Center = new Vector2(0, 0);

    private List<Vector2> vertex = new List<Vector2>();
    private int targetCornor = 0;

    protected override void Awake() {
        base.Awake();

        for (int i = 0; i < VertexCount; i++) {
            int x = Random.Range(-SideRange/2, SideRange/2);
            int y = Random.Range(-SideRange/2, SideRange/2);
            vertex.Add(new Vector2(x, y));
        }

        sortVertex();
    }

    protected override void Start() {
        State = MonsterState.MOVE;
    }

    protected override void Update() {
        stateMachine();
    }

    /// <summary>
    /// sort vertex to polygon path
    /// </summary>
    private void sortVertex() {
        List<Vector2> q1q2 = new List<Vector2>();
        List<Vector2> q3q4 = new List<Vector2>();

        int c = vertex.Count;
        for (int i = 0; i < c; i++) {
            if (vertex[i].y >= 0) q1q2.Add(vertex[i]);
            else q3q4.Add(vertex[i]);
        }

        q1q2.Sort((x, y) => { return -x.x.CompareTo(y.x); });
        q3q4.Sort((x, y) => { return x.x.CompareTo(y.x); });

        vertex = q1q2;

        c = q3q4.Count;
        for (int i = 0; i < c; i++)
            vertex.Add(q3q4[i]);
        
    }

    public override void Move() {

        Vector2 pos = transform.position;

        for (int i = 0; i < vertex.Count; i++) {
            if (pos == vertex[i]) {
                targetCornor = ++targetCornor % vertex.Count;
                break;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, vertex[targetCornor], Time.deltaTime * Speed);
    }

}
