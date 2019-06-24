
using Mono.Data.SqliteClient;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DbHelper : MonoBehaviour
{

    public ScriptableCardList cardList;
    public MageList mageList;
        
    
    

    private SqliteConnection con ;

    private void Awake()
    {

        string androidUurl = "URI = file:" + Application.persistentDataPath + Path.DirectorySeparatorChar + "ArcaneWars_SaveGame.db";
        con = new SqliteConnection(androidUurl);

        con.Open();



        string sql = string.Format("PRAGMA foreign_keys = ON;");
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
        
        


        if (!CheckDB())
        {
            SetupDataBase();
        }


    }

    private void SetupDataBase()
    {
        CreatePlayer();
        CreatetableCard();
        CreatetableMage();
        CreateDeckTable();
        //var id = CreateDeck("Deck");
        //SetActiveDeck(id);

        CreateDeckHasCardsTable();

        //for (int i = 0; i < 8; i++)
        //{
        //  AddCardInSlot(cardList.cards[i].UUID, i);
        //}
    }

    public void EraseSave()
    {
        con.Close();

        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);

    }

    public bool CheckDB()
    {
        string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='player';";
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        var reader = command.ExecuteReader();
        if (!reader.Read()) return false;
        return reader.GetString(0).Equals("player");
    }

    private void CreatetableCard()
    {
        string sql = string.Format("CREATE TABLE card(uuid varchar(50),title varchar(50))");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();

        /*
        using (SqliteTransaction tr = (SqliteTransaction)con.BeginTransaction())
        {
            foreach (var c in cardList.cards)
            {
                sql = string.Format("INSERT INTO card(uuid,title) VALUES(\"{0}\",\"{1}\")", c.UUID, c.title);
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            tr.Commit();
        }
        */
    }

    public void AddCard(string uuid, string title)
    {
        string sql = string.Format("INSERT INTO card(uuid,title) VALUES(\"{0}\",\"{1}\")", uuid, title);
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private void CreatetableMage()
    {
        string sql = string.Format("CREATE TABLE mage(uuid varchar(50),title varchar(50),active int(1))");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();

        //AddMage(mageList.mages[0].UUID, mageList.mages[0].title);
        //SetActiveMage(mageList.mages[0].UUID);
        /*
        using (SqliteTransaction tr = (SqliteTransaction)con.BeginTransaction())
        {
            bool active = true;
            foreach (var c in mageList.mages)
            {
                sql = string.Format("INSERT INTO mage(uuid,title,active) VALUES(\"{0}\",\"{1}\",{2})", c.UUID, c.title, (active)?1:0);
                command.CommandText = sql;
                command.Transaction = tr;
                command.ExecuteNonQuery();
                active = false;
                break;
            }
            tr.Commit();
        }
        */
    }

    public void AddMage(string uuid, string title)
    {
        string sql = string.Format("INSERT INTO mage(uuid,title,active) VALUES(@uuid,@title,0)");
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;

        SqliteParameter uuidParam = new SqliteParameter("@uuid", System.Data.DbType.String);
        uuidParam.Value = uuid;

        SqliteParameter titleParam = new SqliteParameter("@title", System.Data.DbType.String);
        titleParam.Value = title;

        command.Parameters.Add(uuidParam);
        command.Parameters.Add(titleParam);

        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void SetActiveMage(string uuid)
    {
        string sql = string.Format("UPDATE mage  SET active=0");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("UPDATE mage  SET active=1 WHERE uuid=@uuid");
        command.CommandText = sql;

        SqliteParameter uuidParam = new SqliteParameter("@uuid", System.Data.DbType.String);
        uuidParam.Value = uuid;

        command.Parameters.Add(uuidParam);
        command.Prepare();
        command.ExecuteNonQuery();
    }


    public void UpdateDeckTitle(long id,string title)
    {
        string sql = string.Format("UPDATE deck  SET title=@title WHERE rowid={0}",id);

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        
        SqliteParameter titleParam = new SqliteParameter("@title", System.Data.DbType.String);
        titleParam.Value = title;

        command.Parameters.Add(titleParam);
        command.Prepare();
        command.ExecuteNonQuery();
    }


    public void SetActiveDeck(long id)
    {
        string sql = string.Format("SELECT rowid FROM mage WHERE active=1");
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        var reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no active mage  found in DB"));
        var mage = reader.GetInt64(0);

        sql = string.Format("UPDATE deck  SET active=0 WHERE mage={0}",mage);

        command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();


        sql = string.Format("UPDATE deck  SET active=1 WHERE rowid=@id");
        command.CommandText = sql;

        SqliteParameter idParam = new SqliteParameter("@id", System.Data.DbType.Int64);
        idParam.Value = id;

        command.Parameters.Add(idParam);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    private void CreateDeckTable()
    {
        string sql = string.Format("CREATE TABLE deck(title varchar(50),mage long,active int(1))");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public int DeckCount(long deckid)
    {
        string sql = string.Format("SELECT count(rowid) FROM deck_has_card WHERE deck=\"{0}\"", deckid);
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;

        var count = Convert.ToInt32(command.ExecuteScalar());

        return count;
    }


    public bool IsActiveDeckCompleted()
    {
        return DeckCount(GetActiveDeck().ID) == 8;
    }

    private void CreateDeckHasCardsTable()
    {
        string sql = string.Format("CREATE TABLE deck_has_card(deck int,card int,slot int)");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("CREATE UNIQUE INDEX idx_deckhascard_slot ON deck_has_card(deck,slot)");
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("CREATE UNIQUE INDEX idx_deckhascard_card ON deck_has_card(deck,card)");
        command.CommandText = sql;
        command.ExecuteNonQuery();

    }

    public void AddCardInSlot(string uuid,int slot)
    {

        var deck = GetActiveDeck().ID;
        var card = GetCardIDFromUUID(uuid);
        var oldSlot = GetCardSlotFromID(deck,card);
        ScriptableCard oldCard = null;
        try
        {
            oldCard = GetCardFromSlot(slot);
        }
        catch (Exception e) { }

        AddCardInSlot(deck, card, slot);

        if(oldCard!=null && oldSlot>=0)
            AddCardInSlot(deck, GetCardIDFromUUID(oldCard.UUID), oldSlot);



    }

    private void AddCardInSlot(long deck, long card, int slot)
    {
        string sql = string.Format("INSERT OR REPLACE INTO deck_has_card(deck,card,slot) VALUES(@deck,@card,@slot)");
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;

        SqliteParameter deckParam = new SqliteParameter("@deck", System.Data.DbType.Int64);
        deckParam.Value = deck;

        SqliteParameter cardParam = new SqliteParameter("@card", System.Data.DbType.Int64);
        cardParam.Value = card;

        SqliteParameter slotParam = new SqliteParameter("@slot", System.Data.DbType.Int64);
        slotParam.Value = slot;

        command.Parameters.Add(deckParam);
        command.Parameters.Add(cardParam);
        command.Parameters.Add(slotParam);

        command.Prepare();
        command.ExecuteNonQuery();
    }

    private long GetCardIDFromUUID(string uuid)
    {
        string sql = string.Format("SELECT rowid FROM card WHERE uuid=\"{0}\"",uuid);

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no card  found in DB with uuid={0}",uuid));

        return reader.GetInt64(0);
    }

    private int GetCardSlotFromID(long deck, long id)
    {
        string sql = string.Format("SELECT slot FROM deck_has_card WHERE deck={0} and card={1}", deck,id);

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) return -1;

        return reader.GetInt32(0);
    }

    public List<ScriptableCard> GetCardsFromDeck(Deck deck)
    {
        var cards = new List<ScriptableCard>(cardList.cards);
        var ret = new List<ScriptableCard>();

        string sql = string.Format("select card.rowid,card.uuid,card.title from card INNER JOIN deck_has_card ON card.rowid=deck_has_card.card WHERE deck_has_card.deck={0}", deck.ID);

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();


        while (reader.Read())
        {

            var uuid = reader.GetString(1);
            var title = reader.GetString(2);

            var card = cards.Find(c => c.UUID.Equals(uuid));

            if (card == null) throw new KeyNotFoundException(string.Format("Card from DB not found in Game [{0}] - [{1}]", uuid, title));
            ret.Add(card);
        }

        return ret;
    }

    public long CreateDeck(string title)
    {
        string sql = string.Format("SELECT rowid FROM mage WHERE active=1");
        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        var reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no active mage  found in DB"));
        var mage = reader.GetInt64(0);

        sql = string.Format("INSERT INTO deck(title,mage,active) VALUES(@title,@mage,0)");
        command.CommandText = sql;

        SqliteParameter titleParam = new SqliteParameter("@title", System.Data.DbType.String);
        titleParam.Value = title;

        SqliteParameter mageParam = new SqliteParameter("@mage", System.Data.DbType.Int64);
        mageParam.Value = mage;

        command.Parameters.Add(titleParam);
        command.Parameters.Add(mageParam);

        command.Prepare();
        command.ExecuteNonQuery();

        sql = "SELECT last_insert_rowid()";
        command.CommandText = sql;

        return (long)command.ExecuteScalar();

    }



    private void CreatePlayer()
    {
        string sql = string.Format("CREATE TABLE player(config varchar(30),value varchar(30))");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "tutorial", "false");
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "gold", 5100);
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "gold", 0);
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "xp", 0);
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "chest", 0);
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "ikey", 0);
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = string.Format("INSERT INTO player(config,value) VALUES(\"{0}\",\"{1}\")", "firstTime", "true");
        command.CommandText = sql;
        command.ExecuteNonQuery();

    }

    public bool HaveCompletedTutorial
    {
        get { return bool.Parse(GetStringConfig("tutorial")); }
        set { SetStringConfig("tutorial", value.ToString()); }
    }

    public bool FirstTime
    {
        get { return bool.Parse(GetStringConfig("firstTime")); }
        set { SetStringConfig("firstTime", value.ToString()); }
    }

    public int Gold
    {
        get
        {
            return int.Parse(GetStringConfig("gold"));
        }
        set
        {
            SetStringConfig("gold",value.ToString());
        }
    }

    public int XP
    {
        get
        {
            return int.Parse(GetStringConfig("xp"));
        }
        set
        {
            SetStringConfig("xp", value.ToString());
        }
    }

    public int Chest
    {
        get
        {
            return int.Parse(GetStringConfig("chest"));
        }
        set
        {
            SetStringConfig("chest", value.ToString());
        }
    }

    public int Key
    {
        get
        {
            return int.Parse(GetStringConfig("ikey"));
        }
        set
        {
            SetStringConfig("ikey", value.ToString());
        }
    }

    private string GetStringConfig(string config)
    {
        string sql = string.Format("SELECT value FROM player WHERE config=@config");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;

        SqliteParameter param = new SqliteParameter("@config", System.Data.DbType.String);
        param.Value = config;

        command.Parameters.Add(param);

        command.Prepare();

        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("{0} not found in DB",config));
        
        return reader.GetString(0);
    }

    private void SetStringConfig(string config, string value)
    {
        string sql = string.Format("UPDATE player  SET value=@value WHERE config=@config");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        command.CommandText = sql;

        SqliteParameter configParam = new SqliteParameter("@config", System.Data.DbType.String);
        configParam.Value = config;

        SqliteParameter valueParam = new SqliteParameter("@value", System.Data.DbType.String);
        valueParam.Value = value;

        command.Parameters.Add(configParam);
        command.Parameters.Add(valueParam);

        command.Prepare();
        command.ExecuteNonQuery();
    }

    private void OnDestroy()
    {
        con.Close();
    }

    public bool HaveMage(string uuid)
    {
        string sql = string.Format("SELECT * FROM mage WHERE uuid=@uuid");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;

        SqliteParameter uuidParam = new SqliteParameter("@uuid",System.Data.DbType.String);
        uuidParam.Value = uuid;

        command.Parameters.Add(uuidParam);

        command.Prepare();

        SqliteDataReader reader = command.ExecuteReader();

        return reader.HasRows;
    }

    public MageData GetActiveMage()
    {
        string sql = string.Format("SELECT uuid FROM mage WHERE active=1");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no active mage  found in DB"));

        return new List<MageData>(mageList.mages).Find(m=>m.UUID.Equals(reader.GetString(0)));
    }

    private long GetActiveMageID()
    {
        string sql = string.Format("SELECT rowid FROM mage WHERE active=1");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no active mage  found in DB"));

        return reader.GetInt64(0);
    }

    public Deck GetActiveDeck()
    {
        string sql = string.Format("SELECT rowid,title FROM deck WHERE active=1 and mage={0}", GetActiveMageID());

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no active deck  found in DB"));

        Deck deck = new Deck();
        deck.ID = reader.GetInt64(0);
        deck.title = reader.GetString(1);
        //deck.mage = GetActiveMage();

        return deck;
    }

    public Deck[] GetDecks()
    {
        string sql = string.Format("SELECT rowid,title FROM deck WHERE mage={0}", GetActiveMageID());

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        var decks = new List<Deck>();

        while (reader.Read())
        {
            Deck deck = new Deck();
            deck.ID = reader.GetInt64(0);
            deck.title = reader.GetString(1);
            decks.Add(deck);
        }


        //deck.mage = GetActiveMage();

        return decks.ToArray(); ;
    }

    public ScriptableCard GetCardFromSlot(int i)
    {
        var deck = GetActiveDeck().ID;
        return GetCardFromSlot(deck,i);
    }

    public ScriptableCard GetCardFromSlot(long deck, int i)
    {
       
        string sql = string.Format("SELECT card FROM deck_has_card WHERE deck={0} and slot={1}", deck,i);

        SqliteCommand command = (SqliteCommand)con.CreateCommand();

        command.CommandText = sql;
        SqliteDataReader reader = command.ExecuteReader();

        if (!reader.Read()) return null;

        
        var cardID = reader.GetInt64(0);

        sql = string.Format("SELECT uuid FROM card WHERE rowid={0}", cardID);

        command.CommandText = sql;
        reader = command.ExecuteReader();

        if (!reader.Read()) throw new KeyNotFoundException(string.Format("no card found in with rowid {0} in DB", cardID));

        var cardUUID = reader.GetString(0);

        return new List<ScriptableCard>(cardList.cards).Find(c=>c.UUID.Equals(cardUUID));
    }



    public List<ScriptableCard> GetPlayerCards()
    {
        
        var cards = new List<ScriptableCard>(cardList.cards);
        var ret = new List<ScriptableCard>();
        
        string sql = string.Format("SELECT * FROM card");

        SqliteCommand command = (SqliteCommand)con.CreateCommand();
        
        command.CommandText = sql;
        SqliteDataReader reader =  command.ExecuteReader();
        

        while (reader.Read())
        {
            
            var uuid = reader.GetString(0);
            var title = reader.GetString(1);
            
            var card = cards.Find(c=>c.UUID.Equals(uuid));
            
            if (card == null) throw new KeyNotFoundException(string.Format("Card from DB not found in Game [{0}] - [{1}]",uuid,title));
            ret.Add(card);
        }

        return ret;
    }
}
