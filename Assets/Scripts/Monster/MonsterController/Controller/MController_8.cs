using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MController_8 : MController_2 {

    //攻城獅王

    //path
    public int VertexCount;
    public int SideRange = 10;
    public int SideRangeOffset = 3;
    public Vector2 Center = new Vector2(0, 0);
    

    private List<Vector2> vertex = new List<Vector2>();
    private int targetVertex;

    protected override void Awake() {
        base.Awake();

        //generate vertex
        for (int i = 0; i < ((float)VertexCount) / 4 + 1; i++) {
            int x = Random.Range(SideRange / 2 - SideRangeOffset, SideRange / 2 + SideRangeOffset);
            int y = Random.Range(-SideRange / 2, SideRange / 2);
            vertex.Add(new Vector2(x, y));
        }

        for (int i = 0; i < ((float)VertexCount) / 4 + 1; i++)
        {
            int x = Random.Range(-SideRange / 2 - SideRangeOffset, -SideRange / 2 + SideRangeOffset);
            int y = Random.Range(-SideRange / 2, SideRange / 2);
            vertex.Add(new Vector2(x, y));
        }

        for (int i = 0; i < ((float)VertexCount) / 4 + 1; i++)
        {
            int x = Random.Range(-SideRange / 2, SideRange / 2);
            int y = Random.Range(SideRange / 2 - SideRangeOffset, SideRange / 2 + SideRangeOffset);
            vertex.Add(new Vector2(x, y));
        }

        for (int i = 0; i < ((float)VertexCount) / 4 + 1; i++)
        {
            int x = Random.Range(-SideRange / 2, SideRange / 2);
            int y = Random.Range(-SideRange / 2 - SideRangeOffset, -SideRange / 2 + SideRangeOffset);
            vertex.Add(new Vector2(x, y));
        }

        int c = vertex.Count;
        for (int i = 0; i < c; i++)
            vertex[i] += Center;

        //according to arctan, sort vertex to circle path 
        vertex.Sort((a, b) => { return Mathf.Atan2(a.y - Center.y, a.x - Center.x).CompareTo(Mathf.Atan2(b.y - Center.y, b.x - Center.x)); });
        
    }

    protected override void Start() {
        State = MonsterState.MOVE;

        int c = vertex.Count;
        float dist = 1000000;
        for (int i = 0; i < c; i++) {
            if (Vector2.Distance(transform.position, vertex[i]) < dist) {
                dist = Vector2.Distance(transform.position, vertex[i]);
                targetVertex = i;
            }
        }

        Debug.Log(GetComponent<SpriteRenderer>().sprite.rect);
        Debug.Log(GetComponent<SpriteRenderer>().sprite.pivot);
    }

    protected override void Update() {
        stateMachine();
    }

    public override void Move() {

        Vector2 pos = transform.position;

        for (int i = 0; i < vertex.Count; i++) {
            if (pos == vertex[i]) {
                targetVertex = ++targetVertex % vertex.Count;
                break;
            }
        }

        moveToTarget();
    }

    private void moveToTarget() {
        float atan = Mathf.Atan2(vertex[targetVertex].y - Center.y, vertex[targetVertex].x - Center.x) * Mathf.Rad2Deg;

        Vector2 target = (atan > -45 && atan <= 45) || (atan > 135 || atan <= -135) ?
            transform.position.y == vertex[targetVertex].y ? vertex[targetVertex] : new Vector2(transform.position.x, vertex[targetVertex].y) :
            transform.position.x == vertex[targetVertex].x ? vertex[targetVertex] : new Vector2(vertex[targetVertex].x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * Speed);

    }

}
