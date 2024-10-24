using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Pathfinder 
{

  
    public enum PathMode
    {
        mA,mM,pA,pM
    }
    // Start is called before the first frame update
 
    //BFS 사용
   public HashSet<Tile> Range(Tile startTile,int cnt,PathMode mode)
    {

        Tile currentTIle = startTile;
        Queue<Tile> openTiles = new Queue<Tile>();
        HashSet<Tile> closedTiles = new HashSet<Tile>();
        GameManager.instance.resetF();
        openTiles.Enqueue(currentTIle);
        //Debug.Log(currentTIle);
        currentTIle.gCost = 0;
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };
        while (openTiles.Count > 0)
        {
            currentTIle = openTiles.Dequeue();
            closedTiles.Add(currentTIle);
            if (currentTIle.gCost ==cnt) { continue; }
            for (int i = 0; i < 4; i++)
            {
               
                if (currentTIle.getDir(i))
                    continue;
                Tile tempTIle = GameManager.instance.getTile(currentTIle.getX() + dx[i], currentTIle.getY() + dy[i]);
               
                //g를 cnt 대용으로 씀
                if (tempTIle != null && !closedTiles.Contains(tempTIle))
                {
                    if (tempTIle.state == Tile.TileState.Occupied)
                    {
                        switch (mode)
                        {
                            case PathMode.pM:
                                if (tempTIle.on.GetComponent<FMonster>() != null)
                                {
                                  
                                    continue;
                                }
                                break;
                            case PathMode.mM:
                                {
                                    if (tempTIle.on.GetComponent<FPlayer>() != null)
                                        continue;
                                }
                                break;
                        }
                        
                        
                    }
                    if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost) //cnt가 더 짧을 때
                    {
                        tempTIle.gCost = currentTIle.gCost + 1;    //교체한다! 
                        if (!openTiles.Contains(tempTIle))
                            openTiles.Enqueue(tempTIle);

                    }

                }


            }
           
        }
        GameManager.instance.resetF();
        Debug.Log($"총개수: {closedTiles.Count}");
        return closedTiles;
    }
    public List<Tile> FindNext(Tile startTile, Tile endTile, PathMode mode)
    {
        Tile currentTIle=startTile;
        List<Tile> openTiles = new List<Tile>();
        HashSet<Tile> closedTiles = new HashSet<Tile>();
        GameManager.instance.resetF();
       openTiles.Add(currentTIle);
        int[] dx = { 0, 0, 1, -1 }; 
        int[] dy = { 1, -1, 0, 0 }; 
        while (openTiles.Count>0&&!closedTiles.Contains(endTile))
        {
            closedTiles.Add(currentTIle);
            
            for(int i = 0; i < 4; i++)
            {
                if (currentTIle.getDir(i))
                    continue;
                Tile tempTIle = GameManager.instance.getTile(currentTIle.getX() + dx[i], currentTIle.getY() + dy[i]);
               
              if (tempTIle != null && !closedTiles.Contains(tempTIle))
                    {
                    if (tempTIle.state == Tile.TileState.Occupied)
                    {
                        switch (mode)
                        {
                            case PathMode.pM:
                                if (tempTIle.on.GetComponent<Monster>() != null)
                                {
                                    Debug.Log("체크");
                                    continue;
                                }
                                break;
                            case PathMode.mM:
                                {
                                    //if (tempTIle.on.GetComponent<Player>() != null)
                                       // continue;
                                }
                                break;
                        }


                    }


                    if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost) //만약 오픈리스트에 존재하지 않거나/원래 있던 g코스트보다 더 낮은 경우 즉: 저장 된 경우보다 이동 거리가 짧은 경우
                        {
                            tempTIle.gCost = currentTIle.gCost + 1;    //교체한다! 
                            tempTIle.hCost = Mathf.Abs(endTile.getX() - tempTIle.getX()) + Mathf.Abs(endTile.getY() - tempTIle.getY());
                            if (!openTiles.Contains(tempTIle))
                                openTiles.Add(tempTIle);
                            tempTIle.parent = currentTIle; //부모로 지정
                        }
                    }

                
            }

            ////시작점 상하좌우를 openlist에 넣는다. 
            //for (int i = -1; i <= 1; i+=2)
            //{
            //    Tile tempTIle = Grid.instance.getTile(currentTIle.getX() + i, currentTIle.getY());

            //    if (tempTIle != null&&!closedTiles.Contains(tempTIle))
            //    {
            //        if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost) //만약 오픈리스트에 존재하지 않거나/원래 있던 g코스트보다 더 낮은 경우 즉: 저장 된 경우보다 이동 거리가 짧은 경우
            //        {
            //            tempTIle.gCost = currentTIle.gCost + 1;    //교체한다! 
            //            tempTIle.hCost = Mathf.Abs(endTile.getX() - tempTIle.getX()) + Mathf.Abs(endTile.getY() - tempTIle.getY());
            //            if(!openTiles.Contains(tempTIle))
            //                openTiles.Add(tempTIle);
            //            tempTIle.parent = currentTIle; //부모로 지정
            //        }
            //    }
             
            //}
            //for (int j = -1; j <= 1; j += 2)
            //{
            //    Tile tempTIle = Grid.instance.getTile(currentTIle.getX(), currentTIle.getY() + j);
            //    if (tempTIle != null && !closedTiles.Contains(tempTIle))
            //    {

            //        if (!openTiles.Contains(tempTIle) || currentTIle.gCost + 1 < tempTIle.gCost)
            //        {
            //            tempTIle.gCost = currentTIle.gCost + 1;
            //            tempTIle.hCost = Mathf.Abs(endTile.getX() - tempTIle.getX()) + Mathf.Abs(endTile.getY() - tempTIle.getY());
            //            if (!openTiles.Contains(tempTIle))
            //                openTiles.Add(tempTIle);
            //            tempTIle.parent = currentTIle;
            //        }
            //    }
            //}
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
        if(openTiles.Count == 0)
        {
            Debug.Log($"시작: X = {startTile.getX()}, Y = {startTile.getY()}");
            Debug.Log($"끝: X = {endTile.getX()}, Y = {endTile.getY()}");
            Debug.Log(route + "탐색 실패" + endTile);
            
        }
        while(currentTIle.parent!= null)
        {
             Debug.Log($"Processing tile: X = {currentTIle.getX()}, Y = {currentTIle.getY()}Gcost={currentTIle.gCost}hcost={currentTIle.hCost}fcost={currentTIle.fCost}");
            route.Add(currentTIle);
            currentTIle= currentTIle.parent;
        }
        route.Reverse();
        GameManager.instance.resetF();
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
    public List<Tile> findPlayer(Tile mon, List<FPlayer> players)
    {
        List<Tile> list = new List<Tile>();
        foreach (FPlayer player in players) { 
            list.Add(player.unitTIle);
        }
        List<Tile> result = new List<Tile>();
        foreach (Tile p in list) {
            List<Tile> temp = FindNext(mon, p, PathMode.mM);
            if (temp.Count > 0 && (result.Count==0 ||temp.Count < result.Count))
                result = temp;
        }
     
        return result;
    }
    
    int GetDistance(Tile tile) {
        return 10; 
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
