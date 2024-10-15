using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    [SerializeField]
    public Tile startTile;
    public Tile endTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //배열 뒤집는 연산 하기 아까우니까 처음부터 역으로 계산하려 했는데 이럼 안 될 수도 있음 그런 경우: 길을 찾고 전부 이동하는 게 아닐 수도 있다.  
    public List<Tile> FindNext(Tile startTile, Tile endTile)
    {
        Tile currentTIle=startTile;
        List<Tile> openTiles = new List<Tile>();
        HashSet<Tile> closedTiles = new HashSet<Tile>();
        Grid.instance.resetF();
       openTiles.Add(currentTIle);
        bool fin = false;
        while (openTiles.Count>0&&!closedTiles.Contains(endTile))
        {
            closedTiles.Add(currentTIle);
            

            //시작점 상하좌우를 openlist에 넣는다. 
            for (int i = -1; i <= 1; i+=2)
            {
                Tile tempTIle = Grid.instance.getTile(currentTIle.getX() + i, currentTIle.getY());

                if (tempTIle != null&&!closedTiles.Contains(tempTIle))
                {
                    if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost) //만약 오픈리스트에 존재하지 않거나/원래 있던 g코스트보다 더 낮은 경우 즉: 저장 된 경우보다 이동 거리가 짧은 경우
                    {
                        tempTIle.gCost = currentTIle.gCost + 1;    //교체한다! 
                        tempTIle.hCost = Mathf.Abs(endTile.getX() - tempTIle.getX()) + Mathf.Abs(endTile.getY() - tempTIle.getY());
                        if(!openTiles.Contains(tempTIle))
                            openTiles.Add(tempTIle);
                        tempTIle.parent = currentTIle; //부모로 지정
                    }
                }
             
            }
            for (int j = -1; j <= 1; j += 2)
            {
                Tile tempTIle = Grid.instance.getTile(currentTIle.getX(), currentTIle.getY() + j);
                if (tempTIle != null && !closedTiles.Contains(tempTIle))
                {

                    if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost)
                    {
                        tempTIle.gCost = currentTIle.gCost + 1;
                        tempTIle.hCost = Mathf.Abs(endTile.getX() - tempTIle.getX()) + Mathf.Abs(endTile.getY() - tempTIle.getY());
                        if (!openTiles.Contains(tempTIle))
                            openTiles.Add(tempTIle);
                        tempTIle.parent = currentTIle;
                    }
                }
            }
            openTiles.Remove(currentTIle);
            openTiles.Sort(CompareT);

            if (openTiles.Count ==0)
                break;
            currentTIle= openTiles[0];
            
       
            if (currentTIle == endTile)
                break;      
        }
        List<Tile> route = new List<Tile>(); 
        closedTiles.Add(currentTIle);
        while(currentTIle.parent!= null)
        {
             Debug.Log($"Processing tile: X = {currentTIle.getX()}, Y = {currentTIle.getY()}Gcost={currentTIle.gCost}hcost={currentTIle.hCost}fcost={currentTIle.fCost}");
            route.Add(currentTIle);
            currentTIle= currentTIle.parent;
        }
        route.Reverse();
        Grid.instance.resetF();
        if(closedTiles.Contains(endTile))
            return route;
        return null;
    }
    int CompareT(Tile a,Tile b)
    {
        if(a.fCost<b.fCost)
            return -1;
        else if(a.fCost>b.fCost) 
            return 1;
        else
        {
            if (a.gCost < b.gCost) //만약 f가중치가 같으면 g로 정해줘야한다... 중요하다...
                return -1;
            else if (a.gCost > b.gCost)
                return 1;
        } 

        return 0;
    }
    
    int GetDistance(Tile tile) {
        return 10; 
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
