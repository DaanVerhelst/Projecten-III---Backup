package be.grasmaaier.kolveniershof.database

import androidx.lifecycle.LiveData
import androidx.room.*

@Dao
interface DagDao {
    @Query("SELECT * FROM dag_atelier_client_table WHERE datum = :datum AND client = :persoonId")
    fun getAteliers(persoonId: Long, datum: String): LiveData<DatabaseDagAtelierClient>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertAteliers(vararg ateliers: DatabaseDagAtelierClient)
}