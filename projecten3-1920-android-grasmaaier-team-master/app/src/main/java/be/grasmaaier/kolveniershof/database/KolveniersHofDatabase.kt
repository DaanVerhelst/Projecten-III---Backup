package be.grasmaaier.kolveniershof.database

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase

@Database(entities = [DatabaseClient::class, DatabaseDagAtelierClient::class], version = 1, exportSchema = false)
abstract class KolveniersHofDatabase : RoomDatabase(){
    abstract val clientsDao: PersoonDao
    abstract val dagDao: DagDao

    companion object {
        @Volatile
        private var INSTANCE: KolveniersHofDatabase? = null

        fun getInstance(context: Context): KolveniersHofDatabase {
            synchronized(this) {
                var instance = INSTANCE
                if (instance == null) {
                    instance = Room.databaseBuilder(
                        context.applicationContext,
                        KolveniersHofDatabase::class.java,
                        "databasekolveniershof"
                    ).fallbackToDestructiveMigration().build()
                    INSTANCE = instance
                }
                return instance
            }
        }
    }
}