package be.grasmaaier.kolveniershof.database

import androidx.lifecycle.LiveData
import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query

@Dao
interface PersoonDao {
    @Query("select * from client_table")
    fun getClients(): LiveData<List<DatabaseClient>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertAll(vararg clients: DatabaseClient)
}