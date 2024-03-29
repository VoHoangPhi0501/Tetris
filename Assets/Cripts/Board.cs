
using UnityEngine;
using UnityEngine.Tilemaps;
public class Board : MonoBehaviour
{   public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get;private set; }
    public Vector3Int spawnPosition;
    public Vector2Int boardsize = new Vector2Int(10,20);
    public RectInt Bounds
    {
        get
        {
            Vector2Int positision = new Vector2Int(-boardsize.x/2,-boardsize.y/2);
            return new RectInt(positision, this.boardsize);
        }
    }
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for(int i =0;i<this.tetrominoes.Length;i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }
    private void Start()
    {
        Spawnpiece();
    }
    public void Spawnpiece()
    {
        int random = Random.Range(0,this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this,this.spawnPosition, data);
        Set(this.activePiece);
    }
    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition,null);
        }
    }
    public bool isvalidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
            if (this.tilemap.HasTile(tilePosition)){
                return false;
            }
          
        }
        return true;
    }
    public void clearline()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while(row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else row++;
        }
    }
    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col =  bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position  = new Vector3Int(col, row,0);
            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }
    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }
        while (row < bounds.yMax)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);
                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }
}
